using UnityEngine;

public class DeathMatchKillManager : MonoBehaviour
{
    [SerializeField] private RacerDataContainer _racerData;
    [SerializeField] private ScoreboardView _scoreboardView;
    [SerializeField] private MatchRules _matchRules;
    [SerializeField] private ResultsProcessor _resultsProcessor;
    [SerializeField] private PlayerCarSelector _carSelector;

    private void Start()
    {
        _carSelector.CarActivated += OnCarActivated;

        _racerData.Initialize();
        _matchRules.Configure(_racerData, _resultsProcessor);
        _scoreboardView.Initialize(_racerData);
    }

    private void OnDestroy() => _racerData.Dispose();

    private void OnCarActivated()
    {
        _racerData.RefreshActiveStates();
        _scoreboardView.UpdateView();
    }

    private void OnDisable()
    {
        _carSelector.CarActivated -= OnCarActivated;
        _racerData.Dispose();
    }
}






//public class DeathMatchKillManager : MonoBehaviour
//{
//    [SerializeField] private TextMeshProUGUI _scoreboardUI;
//    [SerializeField] private PlayerCarSelector _carSelector;
//    [SerializeField] private RaceRewardHandler _rewardHandler;
//    [SerializeField] private int _requiredKills = 10;
//    [SerializeField] private DamageHandler[] _allDamageHandlers;

//    private const int TOTAL_RACERS = 6;
//    private int _playerRacerId = 6;
//    private int[] _racerIds;
//    private int[] _kills;
//    private string[] _names;
//    private Racer[] _racers;
//    private Racer _playerRacer;
//    private bool[] _isActive;
//    private System.Text.StringBuilder _sb;
//    private bool _initialized;
//    private bool _matchEnded;

//    private struct RacerScore : System.IComparable<RacerScore>
//    {
//        public Racer Racer;
//        public int Kills;
//        public int Id;

//        public int CompareTo(RacerScore other)
//        {
//            int killComparison = other.Kills.CompareTo(Kills);
//            return killComparison != 0 ? killComparison : other.Id.CompareTo(Id);
//        }
//    }

//    private void Awake()
//    {
//        InitializeArrays();
//        _carSelector.CarActivated += OnCarActivated;
//    }

//    private void InitializeArrays()
//    {
//        _racerIds = new int[TOTAL_RACERS];
//        _kills = new int[TOTAL_RACERS];
//        _names = new string[TOTAL_RACERS];
//        _racers = new Racer[TOTAL_RACERS];
//        _isActive = new bool[TOTAL_RACERS];
//        _sb = new System.Text.StringBuilder(TOTAL_RACERS * 20);
//    }

//    private void OnDestroy()
//    {
//        UnsubscribeEvents();
//        _carSelector.CarActivated -= OnCarActivated;
//    }

//    private void UnsubscribeEvents()
//    {
//        for (int i = 0; i < TOTAL_RACERS; i++)
//        {
//            if (_allDamageHandlers[i] != null)
//            {
//                _allDamageHandlers[i].VehicleKilled -= OnVehicleKilled;
//            }
//        }
//    }

//    private void OnCarActivated()
//    {
//        if (_initialized) return;

//        _rewardHandler.SetBossBattleMode(false);
//        RefreshActiveStates();
//        CacheRacersData();
//        SubscribeToEvents();
//        UpdateScoreboard();
//        _initialized = true;
//    }

//    private void RefreshActiveStates()
//    {
//        for (int i = 0; i < TOTAL_RACERS; i++)
//        {
//            _isActive[i] = _allDamageHandlers[i] != null
//                        && _allDamageHandlers[i].isActiveAndEnabled;
//        }
//    }

//    private void CacheRacersData()
//    {
//        _playerRacer = null;

//        for (int i = 0; i < TOTAL_RACERS; i++)
//        {
//            if (!_isActive[i])
//            {
//                _racers[i] = null;
//                continue;
//            }

//            Vehicle vehicle = _allDamageHandlers[i].GetComponent<Vehicle>();
//            Racer racer = vehicle?.GetComponent<Racer>();

//            if (racer != null)
//            {
//                _racers[i] = racer;
//                _racerIds[i] = racer.RacerId;
//                _kills[i] = 0;
//                _names[i] = string.IsNullOrEmpty(racer.Name)
//                    ? $"Racer {racer.RacerId}"
//                    : racer.Name;

//                // Поиск игрока по ID
//                if (racer.RacerId == _playerRacerId)
//                {
//                    _playerRacer = racer;
//                }
//            }
//        }
//    }

//    private void SubscribeToEvents()
//    {
//        for (int i = 0; i < TOTAL_RACERS; i++)
//        {
//            if (_isActive[i])
//            {
//                _allDamageHandlers[i].VehicleKilled -= OnVehicleKilled;
//                _allDamageHandlers[i].VehicleKilled += OnVehicleKilled;
//            }
//        }
//    }

//    private void OnVehicleKilled(Vehicle victim, IWeapon killerWeapon)
//    {
//        if (_matchEnded) return;

//        RefreshActiveStates();

//        if (killerWeapon?.OwnerVehicle == null)
//        {
//            UpdateScoreboard();
//            return;
//        }

//        Racer killer = FindCachedRacer(killerWeapon.OwnerVehicle);
//        if (killer != null)
//        {
//            IncrementKills(killer.RacerId);
//            CheckMatchCompletion(killer);
//            UpdateScoreboard();
//        }
//    }

//    private Racer FindCachedRacer(Vehicle vehicle)
//    {
//        for (int i = 0; i < TOTAL_RACERS; i++)
//        {
//            if (_racers[i] != null
//            && _racers[i].gameObject == vehicle.gameObject)
//            {
//                return _racers[i];
//            }
//        }
//        return null;
//    }

//    private void IncrementKills(int racerId)
//    {
//        for (int i = 0; i < TOTAL_RACERS; i++)
//        {
//            if (_racers[i] != null
//            && _racerIds[i] == racerId)
//            {
//                _kills[i]++;
//                break;
//            }
//        }
//    }

//    private void CheckMatchCompletion(Racer killer)
//    {
//        int killerIndex = GetRacerIndex(killer.RacerId);
//        if (killerIndex != -1 && _kills[killerIndex] >= _requiredKills)
//        {
//            _matchEnded = true;
//            ProcessMatchResults();
//            Time.timeScale = 0f;
//        }
//    }

//    private int GetRacerIndex(int racerId)
//    {
//        for (int i = 0; i < TOTAL_RACERS; i++)
//        {
//            if (_racerIds[i] == racerId) return i;
//        }
//        return -1;
//    }

//    private void ProcessMatchResults()
//    {
//        RacerScore[] scores = new RacerScore[TOTAL_RACERS];
//        int validCount = 0;

//        for (int i = 0; i < TOTAL_RACERS; i++)
//        {
//            if (_racers[i] != null && _isActive[i])
//            {
//                scores[validCount++] = new RacerScore
//                {
//                    Racer = _racers[i],
//                    Kills = _kills[i],
//                    Id = _racerIds[i]
//                };
//            }
//        }

//        System.Array.Sort(scores, 0, validCount);

//        Racer[] finalResults = new Racer[TOTAL_RACERS];
//        int resultsIndex = 0;

//        for (int i = 0; i < validCount; i++)
//        {
//            finalResults[resultsIndex] = scores[i].Racer;
//            finalResults[resultsIndex].SetPosition(resultsIndex + 1);
//            resultsIndex++;
//        }

//        while (resultsIndex < TOTAL_RACERS)
//        {
//            finalResults[resultsIndex] = CreateDummyRacer(resultsIndex + 1);
//            resultsIndex++;
//        }

//        if (finalResults.Length > 0 && _playerRacer != null)
//        {
//            _rewardHandler.HandleRaceFinish(finalResults, _playerRacer);
//        }
//        else
//        {
//            Debug.LogError("Player racer not found!");
//        }
//    }

//    private Racer CreateDummyRacer(int position)
//    {
//        GameObject dummyObj = new GameObject($"DummyPos{position}")
//        {
//            tag = "Untagged",
//            layer = LayerMask.NameToLayer("Default")
//        };

//        var dummy = dummyObj.AddComponent<Racer>();
//        dummy.SetPosition(position);
//        dummy.RacerId = -position;
//        dummy.name = $"Dummy Racer {position}";
//        return dummy;
//    }

//    private void UpdateScoreboard()
//    {
//        if (_scoreboardUI == null) return;

//        _sb.Clear();
//        for (int i = 0; i < TOTAL_RACERS; i++)
//        {
//            if (_isActive[i]
//            && _racers[i] != null
//            && !string.IsNullOrEmpty(_names[i]))
//            {
//                _sb.Append(_names[i]);
//                _sb.Append(" — ");
//                _sb.Append(_kills[i]);
//                _sb.Append('\n');
//            }
//        }

//        _scoreboardUI.SetText(_sb);
//    }
//}