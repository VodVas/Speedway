using UnityEngine;
using TMPro;
using Reflex.Attributes;
using YG;

public class RaceProgressTracker : MonoBehaviour
{
    private const string NoCheckpointsError = "RaceProgressTracker: —писок чекпоинтов пуст!";
    private const string NoPlayerFoundError = "RaceProgressTracker: Racer игрока не найден!";
    private const string CheckpointIndexError = "RaceProgressTracker: checkpointIndex должен быть >= 0!";

    [SerializeField] private Transform[] _checkpoints;
    [SerializeField] private Racer[] _racers;
    [SerializeField] private int _playerRacerId = 6;
    [SerializeField] private int _totalLaps = 3;

    [Header("Desktop settings")]
    [SerializeField] private TextMeshProUGUI _desktopPlayerPositionText;
    [SerializeField] private TextMeshProUGUI _desktopPlayerLapsText;

    [Header("Mobile settings")]
    [SerializeField] private TextMeshProUGUI _mobilePlayerPositionText;
    [SerializeField] private TextMeshProUGUI _mobilePlayerLapsText;

    [Inject] private PlayerCarSelector _raceCarSelector;
    [Inject] private RaceRewardHandler _rewardHandler;

    private RaceProgressPositionUI _raceProgressPosition;
    private RaceProgressUILaps _raceProgressUILaps;
    private RaceProgressInitializer _initializer;
    private RaceProgressPositionSorter _positionSorter;
    private RaceProgressCheckpointLogic _checkpointLogic;
    private Racer _playerRacer;
    private bool _raceFinished;
    private TextMeshProUGUI _currentLapsText;
    private TextMeshProUGUI _currentPositionText;

    private void Awake()
    {
        if (!ValidateSerializedData())
        {
            enabled = false;
            return;
        }

        SetupPlatformSpecificUI();
        InitializeSystems();
    }

    private void Start()
    {
        _initializer.InsertPlayerCarIntoRacers();
        ValidateCheckpointsOnStart();
        _initializer.InitializeRacersPositions();

        if (!_rewardHandler.ValidateRewardsSize(_racers.Length))
        {
            enabled = false;
            return;
        }

        _playerRacer = _initializer.FindPlayerRacer();

        if (_playerRacer == null)
        {
            Debug.LogWarning(NoPlayerFoundError, this);
            enabled = false;
            return;
        }

        UpdatePositionAndLapUI();
    }

    private void SetupPlatformSpecificUI()
    {
        bool isMobile = YandexGame.EnvironmentData.isMobile;
        _currentLapsText = isMobile ? _mobilePlayerLapsText : _desktopPlayerLapsText;
        _currentPositionText = isMobile ? _mobilePlayerPositionText : _desktopPlayerPositionText;
    }

    private void InitializeSystems()
    {
        _initializer = new RaceProgressInitializer(_racers, _playerRacerId, _raceCarSelector);
        _raceProgressUILaps = new RaceProgressUILaps(_currentLapsText);
        _positionSorter = new RaceProgressPositionSorter();
        _raceProgressPosition = new RaceProgressPositionUI(_currentPositionText);
        _checkpointLogic = new RaceProgressCheckpointLogic(_totalLaps);
    }

    public void HandleTriggerEnter(Racer racer, int checkpointIndex)
    {
        if (_raceFinished || racer == null || racer.HasFinished) return;

        if (checkpointIndex < 0)
        {
            Debug.LogError(CheckpointIndexError, this);
            enabled = false;
            return;
        }

        bool isPlayer = ReferenceEquals(racer, _playerRacer);
        _checkpointLogic.ProcessCheckpoint(_checkpoints.Length, racer, checkpointIndex, isPlayer, out bool lapCompleted);

        if (lapCompleted && racer.LapsCompleted >= _totalLaps)
        {
            HandleRaceFinish(racer, isPlayer);
        }

        if (isPlayer) UpdatePositionAndLapUI();
    }

    private void UpdatePositionAndLapUI()
    {
        UpdatePositionsAround();
        _raceProgressUILaps.UpdateLapCounter(_playerRacer.LapsCompleted + 1, _totalLaps);
    }

    private void HandleRaceFinish(Racer racer, bool isPlayer)
    {
        racer.SetFinished(true);
        if (isPlayer) EndRace();
        else DisableRacerObject(racer);
    }

    private void EndRace()
    {
        _raceFinished = true;
        _rewardHandler.HandleRaceFinish(_racers, _playerRacer);
        DisableAllRacers();
    }

    private void UpdatePositionsAround()
    {
        _positionSorter.SortRacers(ref _racers);

        for (int i = 0; i < _racers.Length; i++)
        {
            if (_racers[i] == null) continue;

            int newPosition = i + 1;
            if (_racers[i].Position != newPosition)
            {
                _racers[i].UpdatePreviousPosition();
                _racers[i].SetPosition(newPosition);

                if (ReferenceEquals(_racers[i], _playerRacer))
                {
                    _raceProgressPosition.UpdatePlayerUI(_racers[i], _racers.Length);
                }
            }
        }
    }

    private void DisableAllRacers()
    {
        for (int i = 0; i < _racers.Length; i++)
        {
            if (_racers[i] != null) DisableRacerObject(_racers[i]);
        }
    }

    private void DisableRacerObject(Racer racer)
    {
        if (racer != null && racer.gameObject != null)
            racer.gameObject.SetActive(false);
    }

    private bool ValidateSerializedData()
    {
        bool isValid = true;

        if (_playerRacerId < 0)
        {
            Debug.LogError("RaceProgressTracker: PlayerId должен быть больше нул€", this);
            isValid = false;
        }

        if (_totalLaps < 1)
        {
            Debug.LogError("RaceProgressTracker:  оличество кругов должно быть >= 1", this);
            isValid = false;
        }

        if (_raceCarSelector == null)
        {
            Debug.LogError("RaceProgressTracker: RaceCarManager не назначен!", this);
            isValid = false;
        }

        if (_checkpoints == null || _checkpoints.Length == 0)
        {
            Debug.LogError(NoCheckpointsError, this);
            isValid = false;
        }

        return isValid;
    }

    private void ValidateCheckpointsOnStart()
    {
        if (_checkpoints == null || _checkpoints.Length == 0)
        {
            Debug.LogError(NoCheckpointsError, this);
            enabled = false;
        }
    }
}