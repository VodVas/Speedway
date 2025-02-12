using UnityEngine;
using TMPro;
using System;
using Zenject;

//public class RaceProgressTracker : MonoBehaviour
//{
//    private const string ErrorNoCheckpoints = "RaceManager: checkpoints is empty!";
//    private const string ErrorNoPlayerFound = "RaceManager: player not found";

//    [SerializeField] private Transform[] _checkpoints = null;
//    [SerializeField] private Racer[] _racers = null;
//    [SerializeField] private TextMeshProUGUI _playerPositionText = null;
//    [SerializeField] private int PlayerId = 6;
//    [SerializeField] private int _totalLaps = 3;

//    [Inject] private RaceCarManager _raceCarManager;
//    private Racer _playerRacer = null;
//    private bool _raceFinished = false;

//    private void Start()
//    {
//        InsertPlayerCarIntoRacers();
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
//            Debug.LogError("RaceManager: checkpointIndex must be >= 0!", this);
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
//                else
//                {
//                    DisableRacer(racer);
//                }
//            }
//        }

//        UpdatePositionsAround(racer);
//    }

//    private void InsertPlayerCarIntoRacers()
//    {
//        Racer racer = _raceCarManager.GetPlayerRacer();

//        if (racer == null)
//        {
//            Debug.LogWarning("[RaceProgressTracker] Нет активной машины игрока (Racer)!");
//            return;
//        }

//        racer.RacerId = PlayerId;

//        if (_racers == null || _racers.Length == 0)
//        {
//            _racers = new Racer[1];
//            _racers[0] = racer;
//            return;
//        }

//        for (int i = 0; i < _racers.Length; i++)
//        {
//            if (_racers[i] == null)
//            {
//                _racers[i] = racer;
//                return;
//            }

//            if (_racers[i].RacerId == PlayerId)
//            {
//                _racers[i] = racer;
//                return;
//            }
//        }
//    }

//    private void ExpandArray()
//    {
//        Racer racer = _raceCarManager.GetPlayerRacer();

//        int oldLength = _racers.Length;
//        Racer[] newArray = new Racer[oldLength + 1];

//        for (int i = 0; i < oldLength; i++)
//        {
//            newArray[i] = _racers[i];
//        }

//        newArray[oldLength] = racer;
//        _racers = newArray;
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
//                continue;

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

//        foreach (var racer in _racers)
//        {
//            if (racer != null)
//            {
//                DisableRacer(racer);
//            }
//        }
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

//    private void DisableRacer(Racer racer)
//    {
//        if (racer == null)
//        {
//            return;
//        }
//        racer.gameObject.SetActive(false);
//    }
//}