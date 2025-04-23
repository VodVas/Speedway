using UnityEngine;

public class LastLapRacerEliminator : MonoBehaviour
{
    [SerializeField] private RaceProgressTracker _tracker;

    private Racer[] _racers;
    private int[] _lastCompletedLaps;
    private bool[] _eliminated;
    private int _totalLaps;
    private int _activeRacers;
    private int _lastEliminatedLap = -1;

    private void Awake()
    {
        if (!_tracker) enabled = false;
    }

    private void Start()
    {
        _racers = _tracker.GetRacersArray();
        _totalLaps = _tracker.TotalLaps;
        _activeRacers = _racers.Length;

        InitializeStateArrays();
    }

    private void InitializeStateArrays()
    {
        _lastCompletedLaps = new int[_racers.Length];
        _eliminated = new bool[_racers.Length];
    }

    private void FixedUpdate()
    {
        if (_activeRacers <= 1) return;

        UpdateLapProgress();
        TryEliminateLast();
    }

    private void UpdateLapProgress()
    {
        for (int i = 0; i < _racers.Length; i++)
        {
            if (_eliminated[i] || !_racers[i]) continue;
            _lastCompletedLaps[i] = _racers[i].LapsCompleted;
        }
    }

    private void TryEliminateLast()
    {
        int minLap = FindMinCompletedLap();

        // 1) Не выкидывать в 0-м круге, и не выкидывать в последнем круге
        if (minLap < 1 || minLap >= _totalLaps - 1)
            return;

        // 2) Если уже выкидывали в этом круге — не выкидываем повторно
        if (minLap == _lastEliminatedLap)
            return;

        int lastRacerIndex = FindLastRacerIndex(minLap);
        if (lastRacerIndex == -1)
            return;

        // отмечаем, что в этом круге уже была элиминация
        _lastEliminatedLap = minLap;

        ExecuteElimination(lastRacerIndex);
    }

    private int FindMinCompletedLap()
    {
        int min = int.MaxValue;
        for (int i = 0; i < _racers.Length; i++)
        {
            if (_eliminated[i] || !_racers[i]) continue;
            if (_lastCompletedLaps[i] < min) min = _lastCompletedLaps[i];
        }
        return min;
    }

    private int FindLastRacerIndex(int targetLap)
    {
        int worstPosition = -1;
        int lastIndex = -1;

        for (int i = 0; i < _racers.Length; i++)
        {
            if (_eliminated[i] || !_racers[i]) continue;
            if (_lastCompletedLaps[i] != targetLap) continue;

            if (_racers[i].Position > worstPosition)
            {
                worstPosition = _racers[i].Position;
                lastIndex = i;
            }
        }

        return lastIndex;
    }

    private void ExecuteElimination(int index)
    {
        _eliminated[index] = true;
        _racers[index].gameObject.SetActive(false);
        _activeRacers--;
        Debug.Log($"[Elimination] {_racers[index].Name} выбыл");
    }
}






//public class LastLapRacerEliminator : MonoBehaviour
//{
//    [Header("Ссылка на массив гонщиков (тот же, что в RaceProgressTracker)")]
//    [SerializeField] private Racer[] _racers = null;
//    [Header("Число кругов (должно совпадать с RaceProgressTracker)")]
//    [SerializeField] private int _totalLaps = 3;
//    [SerializeField] private bool _enableElimination = true;

//    private Dictionary<Racer, int> _prevLapCount;
//    private Dictionary<Racer, float> _lapFinishTime;

//    private void Awake()
//    {
//        if (!_enableElimination)
//        {
//            enabled = false;
//            return;
//        }

//        if (_racers == null || _racers.Length == 0)
//        {
//            Debug.LogWarning("[EliminationHandler] Нет гонщиков для выбывания!");
//            enabled = false;
//            return;
//        }

//        if (_totalLaps < 1)
//        {
//            Debug.LogError("[EliminationHandler] Некорректное число кругов!");
//            enabled = false;
//            return;
//        }

//        _prevLapCount = new Dictionary<Racer, int>(_racers.Length);
//        _lapFinishTime = new Dictionary<Racer, float>(_racers.Length);

//        for (int i = 0; i < _racers.Length; i++)
//        {
//            Racer r = _racers[i];

//            if (r != null)
//            {
//                _prevLapCount[r] = r.LapsCompleted;
//                _lapFinishTime[r] = 0f;
//            }
//        }
//    }

//    private void LateUpdate()
//    {
//        for (int i = 0; i < _racers.Length; i++)
//        {
//            Racer racer = _racers[i];
//            if (racer == null)
//                continue;

//            if (racer.HasFinished)
//                continue;

//            int oldLap = _prevLapCount[racer];
//            int newLap = racer.LapsCompleted;

//            if (newLap > oldLap)
//            {
//                _prevLapCount[racer] = newLap;
//                _lapFinishTime[racer] = Time.time;
//            }
//        }

//        EliminateLastIfNeeded();
//    }

//    private void EliminateLastIfNeeded()
//    {
//        int referenceLap = -1;
//        List<Racer> activeRacers = new List<Racer>();

//        for (int i = 0; i < _racers.Length; i++)
//        {
//            Racer racer = _racers[i];

//            if (racer == null)
//                continue;

//            if (racer.HasFinished)
//                continue;

//            if (racer.LapsCompleted >= _totalLaps)
//                continue;

//            activeRacers.Add(racer);

//            if (referenceLap < 0)
//            {
//                referenceLap = racer.LapsCompleted;
//            }
//        }

//        if (activeRacers.Count == 0)
//            return;

//        for (int i = 0; i < activeRacers.Count; i++)
//        {
//            Racer r = activeRacers[i];
//            if (r.LapsCompleted != referenceLap)
//            {
//                return;
//            }
//        }

//        float maxTime = float.MinValue;
//        Racer lastRacer = null;

//        for (int i = 0; i < activeRacers.Count; i++)
//        {
//            Racer racer = activeRacers[i];
//            float t = _lapFinishTime[racer];

//            if (t > maxTime)
//            {
//                maxTime = t;
//                lastRacer = racer;
//            }
//        }

//        if (lastRacer != null)
//        {
//            lastRacer.SetFinished(true);
//            lastRacer.gameObject.SetActive(false);

//            Debug.Log($"[EliminationHandler] {lastRacer.Name} был последним и выбыл из гонки.");
//        }
//    }
//}