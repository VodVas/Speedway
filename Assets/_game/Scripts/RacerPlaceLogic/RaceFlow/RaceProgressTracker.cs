using UnityEngine;
using TMPro;
using Zenject;

public class RaceProgressTracker : MonoBehaviour
{
    private const string NoCheckpointsError = "RaceFlow: список чекпоинтов пуст!";
    private const string NoPlayerFoundError = "RaceFlow: Racer игрока не найден!";
    private const string CheckpointIndexError = "RaceFlow: checkpointIndex должен быть >= 0!";

    [SerializeField] private Transform[] _checkpoints = null;
    [SerializeField] private Racer[] _racers = null;
    [SerializeField] private TextMeshProUGUI _playerPositionText = null;
    [SerializeField] private TextMeshProUGUI _playerLapsText = null;
    [SerializeField] private int _playerId = 6;
    [SerializeField] private int _totalLaps = 3;

    [Inject] private RaceCarSelector _raceCarSelector = null;
    private RaceProgressPositionUI _raceProgressPosition;
    private RaceProgressUILaps _raceProgressUILaps;
    private RaceProgressInitializer _initializer;
    private RaceProgressPositionSorter _positionSorter;
    private RaceProgressCheckpointLogic _checkpointLogic;
    private RaceProgressFinisher _finisher;
    private bool _raceFinished = false;
    private Racer _playerRacer = null;

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
        _finisher = new RaceProgressFinisher();
        _raceProgressPosition = new RaceProgressPositionUI(_playerPositionText);
        _checkpointLogic = new RaceProgressCheckpointLogic(_totalLaps);
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

        _raceProgressPosition.UpdatePlayerUI(_playerRacer);
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
                    _raceProgressPosition.UpdatePlayerUI(_playerRacer);
                }
            }
        }
    }

    private void EndRace(Racer finishingRacer)
    {
        _raceFinished = true;

        _finisher.PrintFinalResults(_racers, finishingRacer);

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
            Debug.LogError("RaceFlow: PlayerId не может быть отрицательным", this);
            return false;
        }

        if (_totalLaps < 1)
        {
            Debug.LogError("RaceFlow: количество кругов должно быть >= 1", this);
            return false;
        }

        if (_raceCarSelector == null)
        {
            Debug.LogError("RaceFlow: RaceCarManager (внедрённый) не назначен!", this);
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