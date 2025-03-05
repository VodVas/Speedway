public class Old
{
    #region RaceCarSelector

    //public class RaceCarSelector : MonoBehaviour
    //{
    //    [SerializeField] private List<RaceCarItem> _allCarsInRace;
    //    [SerializeField] private UiCarBinder _uiCarBinder = null;
    //    [SerializeField] private Transform _carPosition;

    //    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
    //    [Inject] private DriftScoreUIDisplayer _driftScoreUIDisplayer;

    //    private RaceStartTimeCounter _raceTimer;
    //    private Racer _playerRacer;

    //    private void Start()
    //    {
    //        if (_allCarsInRace == null || _allCarsInRace.Count == 0)
    //        {
    //            Debug.LogWarning("[RaceCarSelector] Список машин пуст!");
    //            return;
    //        }

    //        ActivateLastUsedCar();
    //    }

    //    public Racer GetPlayerRacer()
    //    {
    //        return _playerRacer;
    //    }

    //    private void ActivateLastUsedCar()
    //    {
    //        int lastCarId = YandexGame.savesData.GetLastUsedCarId();

    //        if (lastCarId < 0)
    //        {
    //            Debug.LogWarning("[RaceCarSelector] LastUsedCarId не задан, включаем первую машину по умолчанию.");
    //            ActivateCar(0);
    //            return;
    //        }

    //        bool foundCar = false;

    //        for (int i = 0; i < _allCarsInRace.Count; i++)
    //        {
    //            RaceCarItem item = _allCarsInRace[i];
    //            if (item == null || item.carObject == null)
    //                continue;

    //            if (item.carId == lastCarId)
    //            {
    //                ActivateCar(i);
    //                foundCar = true;
    //                break;
    //            }
    //        }

    //        if (!foundCar)
    //        {
    //            Debug.LogWarning($"[RaceCarSelector] Машина с id={lastCarId} не найдена в списке!");

    //            ActivateCar(0);
    //        }
    //    }

    //    //private void ActivateLastUsedCar()
    //    //{
    //    //    int lastCarId = YandexGame.savesData.GetLastUsedCarId();

    //    //    if (lastCarId < 0)
    //    //    {
    //    //        lastCarId = 1;
    //    //        Debug.LogWarning("[RaceCarSelector] LastUsedCarId не найден, используем Car_1 по умолчанию.");
    //    //    }

    //    //    string resourceName = $"Cars/Player/Car_{lastCarId}";
    //    //    GameObject carPrefab = Resources.Load<GameObject>(resourceName);

    //    //    if (carPrefab == null)
    //    //    {
    //    //        Debug.LogWarning($"[RaceCarSelector] Не найден Resource: {resourceName}. Используем Car_1 вместо этого!");
    //    //        carPrefab = Resources.Load<GameObject>("Cars/Player/Car_1");
    //    //    }

    //    //    ActivateCar(carPrefab);
    //    //}

    //    private void ActivateCar(int index)
    //    {
    //        if (index < 0 || index >= _allCarsInRace.Count)
    //        {
    //            Debug.LogError($"[RaceCarSelector] Неверный индекс машины: {index}");
    //            enabled = false;
    //            return;
    //        }

    //        RaceCarItem item = _allCarsInRace[index];

    //        if (item == null || item.carObject == null)
    //        {
    //            Debug.LogError($"[RaceCarSelector] RaceCarItem или его prefab carObject не назначен для index={index}");
    //            enabled = false;
    //            return;
    //        }

    //        GameObject carInstance = Instantiate(item.carObject, _carPosition.position, Quaternion.identity);
    //        carInstance.SetActive(true);

    //        PlayerComponentsEnabler playerEnabler = carInstance.GetComponent<PlayerComponentsEnabler>();
    //        if (playerEnabler != null)
    //        {
    //            playerEnabler.Initialize(_raceTimer);
    //        }
    //        else
    //        {
    //            Debug.LogError("PlayerComponentsEnabler not found on player car!");
    //        }

    //        if (!carInstance.TryGetComponent(out Health playerHealth))
    //        {
    //            Debug.LogError("[RaceCarSelector] Health component не найден на машине!");
    //            return;
    //        }

    //        if (!carInstance.TryGetComponent(out ArcadeVehicleController controller))
    //        {
    //            Debug.LogError("[RaceCarSelector] ArcadeVehicleController не найден на машине!");
    //            return;
    //        }

    //        if (_healthBarDisplay != null)
    //        {
    //            _healthBarDisplay.Initialize(playerHealth);
    //        }
    //        else
    //        {
    //            Debug.LogError("[RaceCarSelector] _healthBarDisplay не назначен в инспекторе!");
    //        }

    //        CarUpgrades carUpgrades = carInstance.GetComponent<CarUpgrades>();

    //        if (carUpgrades != null)
    //        {
    //            carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);

    //            carUpgrades.ApplyPurchasedStats(
    //                YandexGame.savesData.HasCarUpgrade,
    //                controller,
    //                playerHealth
    //            );
    //        }
    //        else
    //        {
    //            Debug.LogWarning("[RaceCarSelector] CarUpgrades не найден на инстанцированной машине!");
    //        }

    //        CarModifications carModifications = carInstance.GetComponent<CarModifications>();

    //        if (carModifications != null)
    //        {
    //            carModifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
    //            carModifications.ApplyPurchasedMods(
    //                YandexGame.savesData.GetCarModificationCount,
    //                controller,
    //                playerHealth
    //            );
    //        }
    //        else
    //        {
    //            Debug.LogWarning("[RaceCarSelector] CarModifications не найден на машине!");
    //        }

    //        _playerRacer = carInstance.GetComponent<Racer>();

    //        if (_uiCarBinder != null)
    //        {
    //            var rigidbody = carInstance.GetComponent<Rigidbody>();
    //            var health = carInstance.GetComponent<Health>();
    //            var carTransform = carInstance.transform;

    //            _uiCarBinder.BindPlayerCar(rigidbody, health, carTransform);
    //        }

    //        if (carInstance.TryGetComponent(out ArcadeVehicleController driftCar) && _driftScoreUIDisplayer != null)
    //        {
    //            _driftScoreUIDisplayer.SetPlayerCar(driftCar);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("[RaceCarSelector] Либо ArcadeVehicleController не найден повторно, либо _driftScoreUIDisplayer не назначен!");
    //        }
    //    }
    //}

    #endregion

    #region AiStuckHelper












    //public class AiStuckHelper : MonoBehaviour
    //{
    //    [SerializeField] private ArcadeAiVehicleController[] _vehicles = default;
    //    [SerializeField] private Transform[] _respawnPoints = default;
    //    [SerializeField] private float _offsetY = 5f;
    //    [SerializeField] private float _minSpeed = 20f;
    //    [SerializeField] private float _stuckTimeout = 5f;
    //    [SerializeField] private float _maxHeightToStuck = -10f;
    //    [SerializeField] private float _checkInterval = 0.5f;

    //    private float[] _stuckTimers;
    //    private Dictionary<ArcadeAiVehicleController, WaypointProgressTracker> _trackerMap;
    //    private WaitForSeconds _wait;

    //    private void Awake()
    //    {
    //        _wait = new WaitForSeconds(_checkInterval);

    //        if (_vehicles == null || _vehicles.Length == 0)
    //        {
    //            Debug.LogError($"AiStuckHelper: поле _vehicles не заполнено или пустое.", this);
    //            enabled = false;
    //            return;
    //        }

    //        if (_respawnPoints == null || _respawnPoints.Length == 0)
    //        {
    //            Debug.LogWarning($"AiStuckHelper: в поле _respawnPoints нет точек респауна!", this);
    //        }

    //        _stuckTimers = new float[_vehicles.Length];

    //        for (int i = 0; i < _vehicles.Length; i++)
    //        {
    //            _stuckTimers[i] = 0f;
    //        }

    //        _trackerMap = new Dictionary<ArcadeAiVehicleController, WaypointProgressTracker>(_vehicles.Length);

    //        for (int i = 0; i < _vehicles.Length; i++)
    //        {
    //            ArcadeAiVehicleController ai = _vehicles[i];

    //            if (ai == null) continue;

    //            WaypointProgressTracker tracker = ai.GetComponent<WaypointProgressTracker>();

    //            if (tracker != null)
    //            {
    //                _trackerMap[ai] = tracker;
    //            }
    //        }
    //    }

    //    private void Start()
    //    {
    //        StartCoroutine(CheckStuckRoutine());
    //    }

    //    private IEnumerator CheckStuckRoutine()
    //    {
    //        while (true)
    //        {
    //            for (int i = 0; i < _vehicles.Length; i++)
    //            {
    //                ArcadeAiVehicleController vehicle = _vehicles[i];

    //                if (vehicle == null) continue;

    //                if (vehicle.carBody.position.y < _maxHeightToStuck)
    //                {
    //                    TeleportStuckVehicle(vehicle);
    //                    _stuckTimers[i] = 0f;

    //                    continue;
    //                }

    //                float speed = vehicle.carBody.velocity.magnitude;

    //                if (speed < _minSpeed)
    //                {
    //                    _stuckTimers[i] += _checkInterval;

    //                    if (_stuckTimers[i] >= _stuckTimeout)
    //                    {
    //                        TeleportStuckVehicle(vehicle);
    //                        _stuckTimers[i] = 0f;
    //                    }
    //                }
    //                else
    //                {
    //                    _stuckTimers[i] = 0f;
    //                }
    //            }

    //            yield return _wait;
    //        }
    //    }

    //    private void TeleportStuckVehicle(ArcadeAiVehicleController vehicle)
    //    {
    //        Transform nearest = FindNearestRespawnPoint(vehicle.transform.position);

    //        vehicle.rb.velocity = Vector3.zero;
    //        vehicle.rb.angularVelocity = Vector3.zero;
    //        vehicle.carBody.velocity = Vector3.zero;
    //        vehicle.carBody.angularVelocity = Vector3.zero;
    //        vehicle.rb.useGravity = false;

    //        Quaternion stuckOrientation;

    //        if (_trackerMap.TryGetValue(vehicle, out WaypointProgressTracker tracker) && tracker != null)
    //        {
    //            if (tracker.Circuit != null)
    //            {
    //                float dist = tracker.progressDistance;
    //                WaypointCircuit.RoutePoint routePoint = tracker.Circuit.GetRoutePoint(dist);
    //                Vector3 forwardDir = routePoint.direction;

    //                if (forwardDir.sqrMagnitude < Mathf.Epsilon)
    //                {
    //                    stuckOrientation = nearest.rotation * Quaternion.Euler(45, 0, 0);
    //                }
    //                else
    //                {
    //                    stuckOrientation = Quaternion.LookRotation(forwardDir, Vector3.up);
    //                }
    //            }
    //            else
    //            {
    //                stuckOrientation = nearest.rotation * Quaternion.Euler(45, 0, 0);
    //            }
    //        }
    //        else
    //        {
    //            stuckOrientation = nearest.rotation * Quaternion.Euler(45, 0, 0);
    //        }

    //        vehicle.transform.position = nearest.position + Vector3.up * _offsetY;
    //        vehicle.carBody.position = nearest.position + Vector3.up * _offsetY;
    //        vehicle.carBody.rotation = stuckOrientation;

    //        //StartCoroutine(EnableGravityAfterDelay(vehicle));
    //    }

    //    private IEnumerator EnableGravityAfterDelay(ArcadeAiVehicleController vehicle)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        vehicle.rb.useGravity = true;
    //    }

    //    private Transform FindNearestRespawnPoint(Vector3 position)
    //    {
    //        Transform nearest = null;
    //        float minDistSqr = float.MaxValue;

    //        for (int i = 0; i < _respawnPoints.Length; i++)
    //        {
    //            Transform respawn = _respawnPoints[i];
    //            if (respawn == null)
    //            {
    //                continue;
    //            }

    //            float distSqr = (respawn.position - position).sqrMagnitude;

    //            if (distSqr < minDistSqr)
    //            {
    //                minDistSqr = distSqr;
    //                nearest = respawn;
    //            }
    //        }

    //        return nearest;
    //    }
    //}

    #endregion

    #region RaceProgresstracker

    //public class RaceProgressTracker : MonoBehaviour
    //{
    //    private const string NoCheckpointsError = "RaceProgressTracker: список чекпоинтов пуст!";
    //    private const string NoPlayerFoundError = "RaceProgressTracker: Racer игрока не найден!";
    //    private const string CheckpointIndexError = "RaceProgressTracker: checkpointIndex должен быть >= 0!";

    //    [SerializeField] private Transform[] _checkpoints = null;
    //    [SerializeField] private Racer[] _racers = null;
    //    [SerializeField] private TextMeshProUGUI _playerPositionText = null;
    //    [SerializeField] private TextMeshProUGUI _playerLapsText = null;
    //    [SerializeField] private int _playerId = 6;
    //    [SerializeField] private int _totalLaps = 3;
    //    [SerializeField] private PlayerCarInstantiator _raceCarSelector = null;

    //    private RaceProgressPositionUI _raceProgressPosition;
    //    private RaceProgressUILaps _raceProgressUILaps;
    //    private RaceProgressInitializer _initializer;
    //    private RaceProgressPositionSorter _positionSorter;
    //    private RaceProgressCheckpointLogic _checkpointLogic;
    //    private RaceProgressFinisher _finisher;
    //    private bool _raceFinished = false;
    //    private Racer _playerRacer = null;

    //    private void Awake()
    //    {
    //        if (ValidateSerializedData() == false)
    //        {
    //            enabled = false;
    //            return;
    //        }

    //        _initializer = new RaceProgressInitializer(_racers, _playerId, _raceCarSelector);
    //        _raceProgressUILaps = new RaceProgressUILaps(_playerLapsText);
    //        _positionSorter = new RaceProgressPositionSorter();
    //        _finisher = new RaceProgressFinisher();
    //        _raceProgressPosition = new RaceProgressPositionUI(_playerPositionText);
    //        _checkpointLogic = new RaceProgressCheckpointLogic(_totalLaps);
    //    }

    //    private void Start()
    //    {
    //        _initializer.InsertPlayerCarIntoRacers();
    //        ValidateCheckpointsOnStart();
    //        _initializer.InitializeRacersPositions();

    //        _playerRacer = _initializer.FindPlayerRacer();

    //        if (_playerRacer == null)
    //        {
    //            Debug.LogWarning(NoPlayerFoundError, this);
    //        }

    //        _raceProgressPosition.UpdatePlayerUI(_playerRacer);
    //    }

    //    public void HandleTriggerEnter(Racer racer, int checkpointIndex)
    //    {
    //        if (_raceFinished || racer == null || racer.HasFinished)
    //        {
    //            return;
    //        }

    //        if (checkpointIndex < 0)
    //        {
    //            Debug.LogError(CheckpointIndexError, this);
    //            enabled = false;
    //            return;
    //        }

    //        bool isPlayer = ReferenceEquals(racer, _playerRacer);

    //        _checkpointLogic.ProcessCheckpoint(_checkpoints.Length, racer, checkpointIndex, isPlayer, out bool lapCompleted);

    //        if (lapCompleted && racer.LapsCompleted >= _totalLaps)
    //        {
    //            racer.SetFinished(true);
    //            if (isPlayer)
    //            {
    //                EndRace(racer);
    //            }
    //            else
    //            {
    //                DisableRacerObject(racer);
    //            }
    //        }

    //        UpdatePositionsAround();
    //        UpdateLapCounter();
    //    }

    //    private void UpdateLapCounter()
    //    {
    //        if (_playerRacer != null)
    //        {
    //            int currentLap = _playerRacer.LapsCompleted + 1;
    //            _raceProgressUILaps.UpdateLapCounter(currentLap, _totalLaps);
    //        }
    //    }

    //    private void UpdatePositionsAround()
    //    {
    //        _positionSorter.SortRacers(ref _racers);

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
    //                    _raceProgressPosition.UpdatePlayerUI(_playerRacer);
    //                }
    //            }
    //        }
    //    }

    //    private void EndRace(Racer finishingRacer)
    //    {
    //        _raceFinished = true;

    //        _finisher.PrintFinalResults(_racers, finishingRacer);

    //        for (int i = 0; i < _racers.Length; i++)
    //        {
    //            if (_racers[i] != null)
    //            {
    //                DisableRacerObject(_racers[i]);
    //            }
    //        }
    //    }

    //    private void DisableRacerObject(Racer racer)
    //    {
    //        if (racer != null)
    //        {
    //            racer.gameObject.SetActive(false);
    //        }
    //    }

    //    private bool ValidateSerializedData()
    //    {
    //        if (_playerId < 0)
    //        {
    //            Debug.LogError("RaceProgressTracker: PlayerId не может быть отрицательным", this);
    //            return false;
    //        }

    //        if (_totalLaps < 1)
    //        {
    //            Debug.LogError("RaceProgressTracker: количество кругов должно быть >= 1", this);
    //            return false;
    //        }

    //        if (_raceCarSelector == null)
    //        {
    //            Debug.LogError("RaceProgressTracker: RaceCarManager не назначен!", this);
    //            return false;
    //        }

    //        return true;
    //    }

    //    private void ValidateCheckpointsOnStart()
    //    {
    //        if (_checkpoints == null || _checkpoints.Length < 1)
    //        {
    //            Debug.LogError(NoCheckpointsError, this);
    //            enabled = false;
    //        }
    //    }
    //}

    #endregion

    #region EnemyInst
    //public class EnemyCarInstantiator : MonoBehaviour
    //{
    //    [SerializeField] private enum EnemyCarType
    //    {
    //        EnemyCrossroad,
    //        EnemyElvis,
    //        EnemyHotRod,
    //        EnemyMustang,
    //        EnemyNewsVan,
    //        EnemyRedneck,
    //        EnemyTub
    //    }

    //    [Tooltip("Количество врагов, которых нужно заспавнить")]
    //    [SerializeField] private int _enemyCountToSpawn = 3;

    //    [Tooltip("Выбор типа каждого врага")]
    //    [SerializeField] private List<EnemyCarType> _enemyTypes = new List<EnemyCarType>();

    //    [Tooltip("Точки спавна для каждого врага")]
    //    [SerializeField] private List<Transform> _enemySpawnPoints = new List<Transform>();

    //    [Inject] private AiStuckHelper _aiStuckHelper;
    //    [Inject] private DeadCarRespawner _deadCarRespawner;
    //    [Inject] private RaceProgressTracker _raceProgressTracker;
    //    [Inject] private Container _container;

    //    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    //    private void OnValidate()
    //    {
    //        // Синхронизация размеров списков _enemyTypes и _enemySpawnPoints
    //        if (_enemyTypes.Count != _enemyCountToSpawn)
    //        {
    //            _enemyTypes.Clear();
    //            for (int i = 0; i < _enemyCountToSpawn; i++)
    //            {
    //                _enemyTypes.Add(EnemyCarType.EnemyCrossroad); // По умолчанию добавляем первый тип
    //            }
    //        }

    //        if (_enemySpawnPoints.Count != _enemyCountToSpawn)
    //        {
    //            _enemySpawnPoints.Clear();
    //            for (int i = 0; i < _enemyCountToSpawn; i++)
    //            {
    //                _enemySpawnPoints.Add(null); // По умолчанию добавляем null
    //            }
    //        }
    //    }

    //    private void Start()
    //    {
    //        SpawnEnemyCars();
    //    }

    //    private void SpawnEnemyCars()
    //    {
    //        if (_enemyTypes.Count == 0 || _enemySpawnPoints.Count == 0)
    //        {
    //            Debug.LogError("EnemyCarInstantiator: Не указаны типы врагов или точки спавна!");
    //            return;
    //        }

    //        for (int i = 0; i < _enemyCountToSpawn; i++)
    //        {
    //            EnemyCarType enemyType = _enemyTypes[i];
    //            Transform spawnPoint = _enemySpawnPoints[i];

    //            if (spawnPoint == null)
    //            {
    //                Debug.LogWarning($"EnemyCarInstantiator: Точка спавна для врага {i + 1} не указана! Используется позиция (0,0,0).");
    //                spawnPoint = transform;
    //            }

    //            string enemyName = enemyType.ToString();
    //            GameObject enemyPrefab = LoadEnemyCarPrefab(enemyName);

    //            if (enemyPrefab == null)
    //            {
    //                Debug.LogWarning($"EnemyCarInstantiator: Не удалось загрузить префаб для {enemyName}!");
    //                continue;
    //            }

    //            Vector3 spawnPos = spawnPoint.position;
    //            Quaternion spawnRot = spawnPoint.rotation;

    //            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, spawnRot);
    //            newEnemy.SetActive(false);

    //            InjectAllComponents(newEnemy);
    //            InitializeEnemyVehicleSystems(newEnemy);
    //            BindEnemyToSystems(newEnemy);
    //            InitializeWaypointTrackers(newEnemy);

    //            newEnemy.SetActive(true);
    //            _spawnedEnemies.Add(newEnemy);
    //        }
    //    }

    //    private void InitializeWaypointTrackers(GameObject enemyInstance)
    //    {
    //        var trackers = enemyInstance.GetComponentsInChildren<WaypointProgressTracker>();
    //        foreach (var tracker in trackers)
    //        {
    //            tracker.InitializeCircuit();
    //        }
    //    }

    //    private GameObject LoadEnemyCarPrefab(string enemyName)
    //    {
    //        string path = $"Cars/Enemy/{enemyName}";
    //        GameObject prefab = Resources.Load<GameObject>(path);

    //        if (prefab == null)
    //        {
    //            Debug.LogError($"EnemyCarInstantiator: Префаб {enemyName} не найден по пути {path}!");
    //        }

    //        return prefab;
    //    }

    //    private void InjectAllComponents(GameObject enemyInstance)
    //    {
    //        var injectables = enemyInstance.GetComponentsInChildren<MonoBehaviour>();
    //        foreach (var component in injectables)
    //        {
    //            try
    //            {
    //                AttributeInjector.Inject(component, _container);
    //            }
    //            catch (Exception e)
    //            {
    //                Debug.LogError($"EnemyCarInstantiator: Injection failed for {component.GetType().Name}: {e.Message}");
    //            }
    //        }
    //    }

    //    private void InitializeEnemyVehicleSystems(GameObject enemyInstance)
    //    {
    //        if (!enemyInstance.TryGetComponent(out Health _))
    //        {
    //            Debug.LogWarning("EnemyCarInstantiator: У врага отсутствует компонент Health!");
    //        }

    //        if (!enemyInstance.TryGetComponent(out ArcadeAiVehicleController _))
    //        {
    //            Debug.LogWarning("EnemyCarInstantiator: У врага отсутствует ArcadeAiVehicleController!");
    //        }
    //    }

    //    private void BindEnemyToSystems(GameObject enemyInstance)
    //    {
    //        if (enemyInstance.TryGetComponent(out Vehicle vehicle))
    //        {
    //            _deadCarRespawner.AddVehicle(vehicle);
    //        }

    //        if (enemyInstance.TryGetComponent(out ArcadeAiVehicleController aiController))
    //        {
    //            _aiStuckHelper.AddVehicle(aiController);
    //        }

    //        if (enemyInstance.TryGetComponent(out Racer enemyRacer))
    //        {
    //            _raceProgressTracker.AddRacer(enemyRacer);
    //        }
    //    }
    //}
    #endregion

    #region GaragePool
    //using System.Collections;
    //using System.Collections.Generic;
    //using UnityEngine;
    //using YG;

    //public class GarageCarInstancePool : MonoBehaviour
    //{
    //    [SerializeField] private List<GameObject> _carPrefabs = null;
    //    [SerializeField] private ComponentsCleaner _physicsCleaner = null;

    //    [field: SerializeField] public Transform GarageSpawnPoint { get; private set; }

    //    private readonly List<GameObject> _spawnedCars = new List<GameObject>();

    //    public List<GameObject> SpawnedCars => _spawnedCars;

    //    private void Awake()
    //    {
    //        if (_carPrefabs == null)
    //        {
    //            Debug.LogError("[GarageCarInstancePool] Car prefabs list is null!", this);
    //            enabled = false;
    //            return;
    //        }
    //        if (GarageSpawnPoint == null)
    //        {
    //            Debug.LogError("[GarageCarInstancePool] GarageSpawnPoint is not assigned!", this);
    //            enabled = false;
    //            return;
    //        }
    //        if (_physicsCleaner == null)
    //        {
    //            Debug.LogError("[GarageCarInstancePool] CarPhysicsCleaner is not assigned!", this);
    //            enabled = false;
    //            return;
    //        }
    //    }

    //    public IEnumerator SpawnPurchasedCars()
    //    {
    //        _spawnedCars.Clear();

    //        for (int i = 0; i < _carPrefabs.Count; i++)
    //        {
    //            GameObject prefab = _carPrefabs[i];

    //            if (prefab == null)
    //            {
    //                Debug.LogWarning($"[GarageCarInstancePool] Prefab at index {i} is null!", this);
    //                continue;
    //            }

    //            CarData data = prefab.GetComponent<CarData>();
    //            if (data == null)
    //            {
    //                Debug.LogWarning($"[GarageCarInstancePool] Missing CarData on prefab '{prefab.name}'!", this);
    //                continue;
    //            }

    //            if (YandexGame.savesData.HasCar(data.Id))
    //            {
    //                GameObject instance = Instantiate(prefab, GarageSpawnPoint.position, GarageSpawnPoint.rotation, GarageSpawnPoint);

    //                _physicsCleaner.RemoveAllPhysicsComponents(instance);
    //                instance.SetActive(false);
    //                _spawnedCars.Add(instance);
    //            }

    //            yield return null;
    //        }
    //    }
    //}
    #endregion

    #region CarShopPool
    //using System.Collections;
    //using System.Collections.Generic;
    //using UnityEngine;

    //public class CarInstancePool : MonoBehaviour
    //{
    //    [SerializeField] private List<GameObject> _carPrefabs = null;
    //    [SerializeField] private ComponentsCleaner _physicsCleaner = null;

    //    [field: SerializeField] public Transform SpawnPoint { get; private set; }

    //    private readonly List<GameObject> _spawnedInstances = new List<GameObject>();

    //    public List<GameObject> SpawnedInstances => _spawnedInstances;
    //    public GameObject GetCarInstance(int index) => _spawnedInstances[index];

    //    private void Awake()
    //    {
    //        if (_carPrefabs == null)
    //        {
    //            Debug.LogError("[CarInstancePool] _carPrefabs is null!", this);
    //            enabled = false;
    //            return;
    //        }
    //        if (SpawnPoint == null)
    //        {
    //            Debug.LogError("[CarInstancePool] SpawnPoint is not assigned!", this);
    //            enabled = false;
    //            return;
    //        }
    //        if (_physicsCleaner == null)
    //        {
    //            Debug.LogError("[CarInstancePool] CarPhysicsCleaner is not assigned!", this);
    //            enabled = false;
    //            return;
    //        }
    //    }

    //    public IEnumerator SpawnAllCars()
    //    {
    //        _spawnedInstances.Clear();

    //        for (int i = 0; i < _carPrefabs.Count; i++)
    //        {
    //            GameObject prefab = _carPrefabs[i];
    //            if (prefab == null)
    //            {
    //                Debug.LogWarning($"[CarInstancePool] Prefab at index {i} is null!", this);
    //                continue;
    //            }

    //            GameObject instance = Instantiate(prefab, SpawnPoint);

    //            _physicsCleaner.RemoveAllPhysicsComponents(instance);
    //            instance.SetActive(false);
    //            _spawnedInstances.Add(instance);

    //            yield return null;
    //        }
    //    }
    //}
    #endregion

    #region ShopValodator
    //public class CarPurchaseValidator
    //{
    //    private readonly List<int> _availableCarIndices = new List<int>();
    //    private readonly List<GameObject> _allCars;

    //    public IReadOnlyList<int> AvailableCarIndices => _availableCarIndices;

    //    public CarPurchaseValidator(List<GameObject> allCars)
    //    {
    //        _allCars = allCars ?? new List<GameObject>();
    //        RecalculateAvailability();
    //    }

    //    public void RecalculateAvailability()
    //    {
    //        _availableCarIndices.Clear();

    //        for (int i = 0; i < _allCars.Count; i++)
    //        {
    //            CarData data = _allCars[i].GetComponent<CarData>();
    //            if (data == null)
    //            {
    //                Debug.LogError("[CarPurchaseValidator] CarData не найден на заспавненном объекте!", _allCars[i]);
    //                continue;
    //            }

    //            if (!YandexGame.savesData.HasCar(data.Id))
    //            {
    //                _availableCarIndices.Add(i);
    //            }
    //        }
    //    }

    //    public bool TryBuyCar(int currentSelectionIndex)
    //    {
    //        if (currentSelectionIndex < 0 || currentSelectionIndex >= _availableCarIndices.Count)
    //        {
    //            Debug.LogError("[CarPurchaseValidator] Некорректный индекс для покупки.");
    //            return false;
    //        }

    //        int realIndex = _availableCarIndices[currentSelectionIndex];
    //        CarData carData = _allCars[realIndex].GetComponent<CarData>();

    //        if (carData == null)
    //        {
    //            Debug.LogError("[CarPurchaseValidator] Данные машины не найдены!");
    //            return false;
    //        }

    //        bool success = YandexGame.savesData.TrySpendMoney(carData.Price);

    //        if (!success)
    //        {
    //            Debug.Log("[CarPurchaseValidator] Недостаточно средств для покупки!");
    //        }
    //        return success;
    //    }
    //}
    #endregion

    #region ShopCycler
    //public class CarSelectionCycler
    //{
    //    private readonly CarInstancePool _pool;
    //    private readonly CarPurchaseValidator _validator;

    //    public int CurrentIndex { get; private set; } = 0;


    //    public CarSelectionCycler(CarInstancePool pool, CarPurchaseValidator validator)
    //    {
    //        _pool = pool;
    //        _validator = validator;

    //        if (_validator.AvailableCarIndices.Count == 0)
    //        {
    //            CurrentIndex = -1;
    //        }
    //    }

    //    public void SwitchCar(int direction)
    //    {
    //        if (_validator.AvailableCarIndices.Count == 0)
    //            return;

    //        SetCarActive(false);
    //        CurrentIndex = (CurrentIndex + direction + _validator.AvailableCarIndices.Count) % _validator.AvailableCarIndices.Count;
    //        SetCarActive(true);
    //    }

    //    public void SetCarActive(bool state)
    //    {
    //        if (CurrentIndex < 0 || CurrentIndex >= _validator.AvailableCarIndices.Count)
    //            return;

    //        int realIndex = _validator.AvailableCarIndices[CurrentIndex];
    //        GameObject instance = _pool.GetCarInstance(realIndex);
    //        instance.transform.position = _pool.SpawnPoint.position;
    //        instance.SetActive(state);
    //    }

    //    public CarData GetCurrentCarData()
    //    {
    //        if (CurrentIndex < 0 || _validator.AvailableCarIndices.Count == 0)
    //            return null;

    //        int realIndex = _validator.AvailableCarIndices[CurrentIndex];
    //        return _pool.GetCarInstance(realIndex).GetComponent<CarData>();
    //    }

    //    public void RevalidateCurrentIndex()
    //    {
    //        if (_validator.AvailableCarIndices.Count == 0)
    //        {
    //            CurrentIndex = -1;

    //            return;
    //        }
    //        if (CurrentIndex >= _validator.AvailableCarIndices.Count)
    //        {
    //            CurrentIndex = _validator.AvailableCarIndices.Count - 1;
    //        }
    //    }
    //}
    #endregion

    #region CarShop
    //public class CarShop : MonoBehaviour
    //{
    //    [SerializeField] private CarInstancePool _carInstancePool = null;

    //    private CarShopUI _carShopUI;
    //    private CarPurchaseValidator _purchaseValidator;
    //    private CarSelectionCycler _selectionCycler;

    //    private void Awake()
    //    {
    //        _carShopUI = GetComponent<CarShopUI>();

    //        if (_carInstancePool == null || _carShopUI == null)
    //        {
    //            Debug.LogError("[CarShopUIMediator] Не назначены компоненты!", this);
    //            enabled = false;
    //            return;
    //        }
    //    }

    //    private IEnumerator Start()
    //    {
    //        yield return StartCoroutine(_carInstancePool.SpawnAllCars());

    //        _purchaseValidator = new CarPurchaseValidator(_carInstancePool.SpawnedInstances);

    //        if (_purchaseValidator.AvailableCarIndices.Count > 0)
    //        {
    //            _selectionCycler = new CarSelectionCycler(_carInstancePool, _purchaseValidator);

    //            _selectionCycler.SetCarActive(true);
    //        }
    //        else
    //        {
    //            Debug.Log("[CarShopUIMediator] Все машины уже куплены или недоступны.");
    //        }

    //        UpdateUI();
    //    }

    //    public void BuyCurrentCar()
    //    {
    //        if (_purchaseValidator == null || _selectionCycler == null)
    //            return;

    //        if (_purchaseValidator.AvailableCarIndices.Count == 0)
    //        {
    //            Debug.Log("[CarShopUIMediator] Нет доступных машин для покупки!");
    //            return;
    //        }

    //        bool canBuy = _purchaseValidator.TryBuyCar(_selectionCycler.CurrentIndex);

    //        if (canBuy)
    //        {
    //            CarData data = _selectionCycler.GetCurrentCarData();
    //            _selectionCycler.SetCarActive(false);
    //            YandexGame.savesData.AddCar(data.Id);
    //            YandexGame.SaveProgress();

    //            _purchaseValidator.RecalculateAvailability();
    //            _selectionCycler.RevalidateCurrentIndex();

    //            if (_purchaseValidator.AvailableCarIndices.Count > 0)
    //            {
    //                _selectionCycler.SetCarActive(true);
    //            }
    //            else
    //            {
    //                Debug.Log("[CarShopUIMediator] Все машины куплены, больше ничего нет.");
    //            }

    //            UpdateUI();
    //        }
    //        else
    //        {
    //            Debug.Log("[CarShopUIMediator] Недостаточно денег или ошибка в покупке.");
    //        }

    //        UpdateUI();
    //    }


    //    //public void BuyCurrentCar()
    //    //{
    //    //    if (_purchaseValidator == null || _selectionCycler == null)
    //    //        return;

    //    //    if (_purchaseValidator.AvailableCarIndices.Count == 0)
    //    //    {
    //    //        Debug.Log("[CarShopUIMediator] Нет доступных машин для покупки!");
    //    //        return;
    //    //    }

    //    //    bool canBuy = _purchaseValidator.TryBuyCar(_selectionCycler.CurrentIndex);

    //    //    if (canBuy)
    //    //    {
    //    //        CarData data = _selectionCycler.GetCurrentCarData();
    //    //        YandexGame.savesData.AddCar(data.Id);
    //    //        YandexGame.SaveProgress();

    //    //        Debug.Log($"[CarShopUIMediator] Машина '{data.CarName}' куплена за {data.Price}.");

    //    //        _purchaseValidator.RecalculateAvailability();
    //    //        _selectionCycler.RevalidateCurrentIndex();

    //    //        if (_purchaseValidator.AvailableCarIndices.Count > 0)
    //    //        {
    //    //            _selectionCycler.SetCarActive(true);
    //    //        }
    //    //        else
    //    //        {
    //    //            Debug.Log("[CarShopUIMediator] Все машины куплены, больше ничего нет.");
    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        Debug.Log("[CarShopUIMediator] Недостаточно денег или ошибка в покупке.");
    //    //    }

    //    //    UpdateUI();
    //    //}

    //    public void SwitchNextCar()
    //    {
    //        if (_purchaseValidator == null || _selectionCycler == null)
    //            return;

    //        if (_purchaseValidator.AvailableCarIndices.Count > 1)
    //        {
    //            _selectionCycler.SwitchCar(1);
    //        }
    //        else
    //        {
    //            Debug.Log("[CarShopUIMediator] Нет других машин, чтобы переключиться вперёд.");
    //        }

    //        UpdateUI();
    //    }

    //    public void SwitchPreviousCar()
    //    {
    //        if (_purchaseValidator == null || _selectionCycler == null)
    //            return;

    //        if (_purchaseValidator.AvailableCarIndices.Count > 1)
    //        {
    //            _selectionCycler.SwitchCar(-1);
    //        }
    //        else
    //        {
    //            Debug.Log("[CarShopUIMediator] Нет других машин, чтобы переключиться назад.");
    //        }

    //        UpdateUI();
    //    }

    //    private void UpdateUI()
    //    {
    //        if (_carShopUI == null)
    //            return;

    //        if (_purchaseValidator == null || _purchaseValidator.AvailableCarIndices.Count == 0)
    //        {
    //            _carShopUI.DisplayNoCarsAvailable();
    //            return;
    //        }

    //        CarData currentCar = _selectionCycler?.GetCurrentCarData();

    //        if (currentCar == null)
    //        {
    //            _carShopUI.DisplayCarNotFound();
    //            return;
    //        }

    //        _carShopUI.DisplayCarData(currentCar);
    //        _carShopUI.UpdatePlayerMoney(YandexGame.savesData.Money);
    //    }
    //}
    #endregion

    #region DynamicAddAIStuckHelper

    //public void AddVehicle(ArcadeAiVehicleController newVehicle)
    //{
    //    if (newVehicle == null)
    //        return;

    //    if (_vehicles == null)
    //    {
    //        _vehicles = new ArcadeAiVehicleController[0];
    //        _trackerMap = new Dictionary<ArcadeAiVehicleController, WaypointProgressTracker>();
    //    }

    //    int oldLength = (_vehicles == null) ? 0 : _vehicles.Length;

    //    ArcadeAiVehicleController[] newArray = new ArcadeAiVehicleController[oldLength + 1];

    //    for (int i = 0; i < oldLength; i++)
    //    {
    //        newArray[i] = _vehicles[i];
    //    }

    //    newArray[oldLength] = newVehicle;
    //    _vehicles = newArray;

    //    float[] newStuckTimers = new float[_vehicles.Length];

    //    for (int i = 0; i < oldLength; i++)
    //    {
    //        newStuckTimers[i] = _stuckTimers[i];
    //    }

    //    newStuckTimers[oldLength] = 0f;
    //    _stuckTimers = newStuckTimers;

    //    WaypointProgressTracker tracker = newVehicle.GetComponent<WaypointProgressTracker>();

    //    if (tracker != null && !_trackerMap.ContainsKey(newVehicle))
    //    {
    //        _trackerMap.Add(newVehicle, tracker);
    //    }

    //    if (oldLength == 0 && newArray.Length > 0)
    //    {
    //        StartCoroutine(CheckStuckRoutine());
    //    }
    //}

    #endregion

    #region Finisher
        //public class RaceProgressFinisher
    //{
    //    public void PrintFinalResults(Racer[] racers, Racer finishingRacer)
    //    {
    //        if (racers == null || finishingRacer == null)
    //        {
    //            Debug.LogWarning("[RaceFlowFinisher] Неверные данные для вывода результатов.");
    //            return;
    //        }

    //        Debug.Log("Гонка завершена! Итоговые результаты:");

    //        for (int i = 0; i < racers.Length; i++)
    //        {
    //            Racer racer = racers[i];

    //            if (racer == null)
    //            {
    //                continue;
    //            }

    //            Debug.Log($"Место: {i + 1} | RacerID: {racer.RacerId} | Круги: {racer.LapsCompleted}");
    //        }

    //        Debug.Log(
    //            $"Игрок с ID {finishingRacer.RacerId} закончил гонку на месте {finishingRacer.Position}."
    //        );
    //    }
    //}
    #endregion
}