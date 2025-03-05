using UnityEngine;
using TMPro;
using Reflex.Attributes;

public class RaceProgressTracker : MonoBehaviour
{
    private const string NoCheckpointsError = "RaceProgressTracker: список чекпоинтов пуст!";
    private const string NoPlayerFoundError = "RaceProgressTracker: Racer игрока не найден!";
    private const string CheckpointIndexError = "RaceProgressTracker: checkpointIndex должен быть >= 0!";

    [SerializeField] private Transform[] _checkpoints = null;
    [SerializeField] private Racer[] _racers = null;
    [SerializeField] private int _playerId = 6;
    [SerializeField] private int _totalLaps = 3;

    [Header("Rewards")]
    [SerializeField] private int[] _positionRewards;
    [SerializeField] private GameObject _rewardCanvas;
    [SerializeField] private GameObject[] _playerUI;

    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI _playerPositionText = null;
    [SerializeField] private TextMeshProUGUI _playerLapsText = null;
    [SerializeField] private TextMeshProUGUI[] _resultTexts = null;

    [Inject] private PlayerCarSelector _raceCarSelector = null;
    private RaceProgressPositionUI _raceProgressPosition;
    private RaceProgressUILaps _raceProgressUILaps;
    private RaceProgressInitializer _initializer;
    private RaceProgressPositionSorter _positionSorter;
    private RaceProgressCheckpointLogic _checkpointLogic;
    private RaceProgressFinisher _finisher;
    private bool _raceFinished = false;
    private Racer _playerRacer = null;
    private bool _rewardGiven;

    private void Awake()
    {
        if (ValidateSerializedData() == false)
        {
            enabled = false;
            return;
        }

        _initializer = new RaceProgressInitializer(_racers, _playerId, _raceCarSelector);
        _raceProgressUILaps = new RaceProgressUILaps(_playerLapsText);
        _positionSorter = new RaceProgressPositionSorter();
        _raceProgressPosition = new RaceProgressPositionUI(_playerPositionText);
        _checkpointLogic = new RaceProgressCheckpointLogic(_totalLaps);
        _finisher = new RaceProgressFinisher(_resultTexts, _positionRewards);
    }

    private void Start()
    {
        _initializer.InsertPlayerCarIntoRacers();
        ValidateCheckpointsOnStart();
        _initializer.InitializeRacersPositions();

        _playerRacer = _initializer.FindPlayerRacer();

        if (_playerRacer == null)
        {
            Debug.LogWarning(NoPlayerFoundError, this);
        }

        _raceProgressPosition.UpdatePlayerUI(_playerRacer, _racers.Length);
    }

    public void HandleTriggerEnter(Racer racer, int checkpointIndex)
    {
        if (_raceFinished || racer == null || racer.HasFinished)
        {
            return;
        }

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
            racer.SetFinished(true);
            if (isPlayer)
            {
                EndRace(racer);
            }
            else
            {
                DisableRacerObject(racer);
            }
        }

        UpdatePositionsAround();
        UpdateLapCounter();
    }

    private void UpdateLapCounter()
    {
        if (_playerRacer != null)
        {
            int currentLap = _playerRacer.LapsCompleted + 1;
            _raceProgressUILaps.UpdateLapCounter(currentLap, _totalLaps);
        }
    }

    private void UpdatePositionsAround()
    {
        _positionSorter.SortRacers(ref _racers);

        for (int i = 0; i < _racers.Length; i++)
        {
            Racer currentRacer = _racers[i];

            if (currentRacer == null)
            {
                continue;
            }

            int newPosition = i + 1;

            if (currentRacer.Position != newPosition)
            {
                currentRacer.UpdatePreviousPosition();
                currentRacer.SetPosition(newPosition);

                if (ReferenceEquals(currentRacer, _playerRacer))
                {
                    _raceProgressPosition.UpdatePlayerUI(_playerRacer, _racers.Length);
                }
            }
        }
    }

    private void EndRace(Racer finishingRacer)
    {
        _raceFinished = true;

        for (int i = 0; i < _playerUI.Length; i++)
        {
            _playerUI[i].SetActive(false);
        }

        _rewardCanvas.SetActive(true);

        _finisher.PrintFinalResults(_racers, finishingRacer);

        if (!_rewardGiven && finishingRacer != null)
        {
            int reward = _finisher.GetRewardForPosition(finishingRacer.Position);
            YG.YandexGame.savesData.AddMoney(reward);
            _rewardGiven = true;
        }

        for (int i = 0; i < _racers.Length; i++)
        {
            if (_racers[i] != null)
            {
                DisableRacerObject(_racers[i]);
            }
        }
    }

    private void DisableRacerObject(Racer racer)
    {
        if (racer != null)
        {
            racer.gameObject.SetActive(false);
        }
    }

    private bool ValidateSerializedData()
    {
        if (_playerId < 0)
        {
            Debug.LogError("RaceProgressTracker: PlayerId не может быть отрицательным", this);
            return false;
        }

        if (_totalLaps < 1)
        {
            Debug.LogError("RaceProgressTracker: количество кругов должно быть >= 1", this);
            return false;
        }

        if (_raceCarSelector == null)
        {
            Debug.LogError("RaceProgressTracker: RaceCarManager не назначен!", this);
            return false;
        }

        if (_resultTexts == null || _resultTexts.Length == 0)
        {
            Debug.LogError("RaceProgressTracker: Results texts not assigned!", this);
            return false;
        }

        if (_positionRewards == null || _positionRewards.Length == 0)
        {
            Debug.LogError("Position rewards not configured!", this);
            return false;
        }

        return true;
    }

    private void ValidateCheckpointsOnStart()
    {
        if (_checkpoints == null || _checkpoints.Length < 1)
        {
            Debug.LogError(NoCheckpointsError, this);
            enabled = false;
        }
    }
}