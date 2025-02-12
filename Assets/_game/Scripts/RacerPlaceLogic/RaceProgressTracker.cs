using UnityEngine;
using TMPro;
using System;

public class RaceProgressTracker : MonoBehaviour
{
    private const string ErrorNoCheckpoints = "RaceManager: checkpoints is empty!.";
    private const string ErrorNoPlayerFound = "RaceManager: player not found";

    [SerializeField] private Transform[] _checkpoints = null;
    [SerializeField] private Racer[] _racers = null;
    [SerializeField] private TextMeshProUGUI _playerPositionText = null;
    [SerializeField] private int PlayerId = 6;
    [SerializeField] private int _totalLaps = 3;

    private Racer _playerRacer = null;
    private bool _raceFinished = false;

    private void Awake()
    {
        ValidateCheckpoints();
        InitializeRacers();
        FindPlayerRacer();
        UpdatePlayerUI();
    }

    public void HandleTriggerEnter(Racer racer, int checkpointIndex)
    {
        if (_raceFinished || racer == null || racer.HasFinished)
        {
            return;
        }

        if (checkpointIndex < 0)
        {
            Debug.LogError("RaceManager: checkpointIndex must be greater than zero!", this);
            enabled = false;
            return;
        }

        int totalCp = _checkpoints.Length;
        bool isPlayer = ReferenceEquals(racer, _playerRacer);

        if (isPlayer)
        {
            int expectedCheckpointIndex = (racer.LastCheckpoint + 1) % totalCp;

            if (checkpointIndex != expectedCheckpointIndex)
            {
                Debug.Log($"Игнорируем пересечение {checkpointIndex}, ждали {expectedCheckpointIndex}");
                return;
            }
        }

        racer.UpdateLastCheckpoint(checkpointIndex);

        if (checkpointIndex == _checkpoints.Length - 1)
        {
            racer.CompleteLap();

            if (racer.LapsCompleted >= _totalLaps)
            {
                racer.SetFinished(true);

                if (isPlayer)
                {
                    EndRace(racer);
                }
                else
                {
                    DisableRacer(racer);
                }
            }
        }

        UpdatePositionsAround(racer);
    }

    private void UpdatePositionsAround(Racer updatedRacer)
    {
        Array.Sort(_racers, (x, y) =>
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;

            int compByLaps = y.LapsCompleted.CompareTo(x.LapsCompleted);

            if (compByLaps != 0)
            {
                return compByLaps;
            }

            return y.LastCheckpoint.CompareTo(x.LastCheckpoint);
        });

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
                    UpdatePlayerUI();
                }
            }
        }
    }

    private void EndRace(Racer finishingRacer)
    {
        _raceFinished = true;

        Debug.Log("Гонка завершена! Итоговые результаты:");

        for (int i = 0; i < _racers.Length; i++)
        {
            Racer racer = _racers[i];
            if (racer == null)
                continue;

            Debug.Log($"Место: {i + 1} | RacerID: {racer.RacerId} | Круги: {racer.LapsCompleted}");
        }

        Debug.Log($"Игрок с ID {finishingRacer.RacerId} закончил гонку на месте {finishingRacer.Position}.");

        foreach (var racer in _racers)
        {
            if (racer != null)
            {
                DisableRacer(racer);
            }
        }
    }

    private void UpdatePlayerUI()
    {
        if (_playerRacer == null || _playerPositionText == null)
        {
            return;
        }

        string positionText = $"Ваша позиция: {_playerRacer.Position}";
        _playerPositionText.text = positionText;
    }

    private void InitializeRacers()
    {
        if (_racers == null)
        {
            return;
        }

        int count = _racers.Length;
        for (int i = 0; i < count; i++)
        {
            Racer racer = _racers[i];
            if (racer == null)
            {
                continue;
            }

            int startPosition = count - i;
            racer.SetPosition(startPosition);
            racer.UpdatePreviousPosition();
        }
    }

    private void FindPlayerRacer()
    {
        if (_racers == null)
        {
            return;
        }

        bool foundPlayer = false;
        for (int i = 0; i < _racers.Length; i++)
        {
            Racer racer = _racers[i];

            if (racer == null)
            {
                continue;
            }

            if (racer.RacerId == PlayerId)
            {
                _playerRacer = racer;
                foundPlayer = true;
                break;
            }
        }

        if (!foundPlayer)
        {
            Debug.LogWarning(ErrorNoPlayerFound, this);
        }
    }

    private void ValidateCheckpoints()
    {
        if (_checkpoints == null || _checkpoints.Length < 1)
        {
            Debug.LogError(ErrorNoCheckpoints, this);
            enabled = false;
        }
    }

    private void DisableRacer(Racer racer)
    {
        if (racer == null)
        {
            return;
        }

        racer.gameObject.SetActive(false);
    }
}









//public class RaceProgressTracker : MonoBehaviour
//{
//    private const string ErrorNoCheckpoints = "RaceManager: checkpoints is empty!.";
//    private const string ErrorNoPlayerFound = "RaceManager: player not found";

//    [SerializeField] private Transform[] _checkpoints = null;
//    [SerializeField] private Racer[] _racers = null;
//    [SerializeField] private TextMeshProUGUI _playerPositionText = null;
//    [SerializeField] private int PlayerId = 6;
//    [SerializeField] private int _totalLaps = 3;

//    private Racer _playerRacer = null;
//    private bool _raceFinished = false;

//    private void Awake()
//    {
//        ValidateCheckpoints();
//        InitializeRacers();
//        FindPlayerRacer();
//        UpdatePlayerUI();
//    }

//    public void HandleTriggerEnter(Racer racer, int checkpointIndex)
//    {
//        if (_raceFinished || racer == null || racer.HasFinished)
//        {
//            return;
//        }

//        if (checkpointIndex < 0)
//        {
//            Debug.LogError("RaceManager: checkpointIndex must be greater than zero!", this);
//            enabled = false;
//            return;
//        }

//        int totalCp = _checkpoints.Length;
//        bool isPlayer = ReferenceEquals(racer, _playerRacer);

//        if (isPlayer)
//        {
//            int expectedCheckpointIndex = (racer.LastCheckpoint + 1) % totalCp;

//            if (checkpointIndex != expectedCheckpointIndex)
//            {
//                Debug.Log($"Игнорируем пересечение {checkpointIndex}, ждали {expectedCheckpointIndex}");
//                return;
//            }
//        }

//        racer.UpdateLastCheckpoint(checkpointIndex);

//        if (checkpointIndex == _checkpoints.Length - 1)
//        {
//            racer.CompleteLap();

//            if (racer.LapsCompleted >= _totalLaps)
//            {
//                racer.SetFinished(true);

//                if (isPlayer)
//                {
//                    EndRace(racer);
//                }
//            }
//        }

//        UpdatePositionsAround(racer);
//    }

//    private void UpdatePositionsAround(Racer updatedRacer)
//    {
//        Array.Sort(_racers, (x, y) =>
//        {
//            if (x == null && y == null) return 0;
//            if (x == null) return 1;
//            if (y == null) return -1;

//            int compByLaps = y.LapsCompleted.CompareTo(x.LapsCompleted);

//            if (compByLaps != 0)
//            {
//                return compByLaps;
//            }

//            return y.LastCheckpoint.CompareTo(x.LastCheckpoint);
//        });

//        for (int i = 0; i < _racers.Length; i++)
//        {
//            Racer currentRacer = _racers[i];

//            if (currentRacer == null)
//            {
//                continue;
//            }

//            int newPosition = i + 1;

//            if (currentRacer.Position != newPosition)
//            {
//                currentRacer.UpdatePreviousPosition();
//                currentRacer.SetPosition(newPosition);

//                if (ReferenceEquals(currentRacer, _playerRacer))
//                {
//                    UpdatePlayerUI();
//                }
//            }
//        }
//    }

//    private void EndRace(Racer finishingRacer)
//    {
//        _raceFinished = true;

//        Debug.Log("Гонка завершена! Итоговые результаты:");

//        for (int i = 0; i < _racers.Length; i++)
//        {
//            Racer racer = _racers[i];
//            if (racer == null)
//                continue;

//            Debug.Log($"Место: {i + 1} | RacerID: {racer.RacerId} | Круги: {racer.LapsCompleted}");
//        }

//        Debug.Log($"Игрок с ID {finishingRacer.RacerId} закончил гонку на месте {finishingRacer.Position}.");
//    }

//    private void UpdatePlayerUI()
//    {
//        if (_playerRacer == null || _playerPositionText == null)
//        {
//            return;
//        }

//        string positionText = $"Ваша позиция: {_playerRacer.Position}";
//        _playerPositionText.text = positionText;
//    }

//    private void InitializeRacers()
//    {
//        if (_racers == null)
//        {
//            return;
//        }

//        int count = _racers.Length;
//        for (int i = 0; i < count; i++)
//        {
//            Racer racer = _racers[i];
//            if (racer == null)
//            {
//                continue;
//            }

//            int startPosition = count - i;
//            racer.SetPosition(startPosition);
//            racer.UpdatePreviousPosition();
//        }
//    }

//    private void FindPlayerRacer()
//    {
//        if (_racers == null)
//        {
//            return;
//        }

//        bool foundPlayer = false;
//        for (int i = 0; i < _racers.Length; i++)
//        {
//            Racer racer = _racers[i];

//            if (racer == null)
//            {
//                continue;
//            }

//            if (racer.RacerId == PlayerId)
//            {
//                _playerRacer = racer;
//                foundPlayer = true;
//                break;
//            }
//        }

//        if (!foundPlayer)
//        {
//            Debug.LogWarning(ErrorNoPlayerFound, this);
//        }
//    }

//    private void ValidateCheckpoints()
//    {
//        if (_checkpoints == null || _checkpoints.Length < 1)
//        {
//            Debug.LogError(ErrorNoCheckpoints, this);
//            enabled = false;
//        }
//    }
//}