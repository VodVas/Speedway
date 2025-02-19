﻿public class OldScripts
{
    #region EnemySpawner

    //public class EnemySpawner : Spawner<Enemy>
    //{
    //    [SerializeField] private float _spawnDelay = 2.0f;

    //    //[Inject] private SafeZoneRegistry _safeZoneRegistry;

    //    private Vector3 _currentSpawnPosition;
    //    private WaitForSeconds _wait;

    //    private void Awake()
    //    {
    //        _wait = new WaitForSeconds(_spawnDelay);
    //    }

    //    protected override void Start()
    //    {
    //        base.Start();
    //        StartCoroutine(DelaySpawn());
    //    }

    //    protected override Type GetObjectTypeToSpawn()
    //    {
    //        return typeof(Crossroad);
    //    }

    //    protected override Vector3 GetSpawnPosition()
    //    {
    //        return _currentSpawnPosition;
    //    }

    //    protected override void HandleObjectDeath(ITerminatable deadObject)
    //    {
    //        base.HandleObjectDeath(deadObject);

    //        var enemy = (Enemy)deadObject;

    //       // StartCoroutine(RespawnCoroutine(enemy.transform.position));
    //    }

    //    private void StartSpawn(Vector3 spawnPosition)
    //    {
    //        _currentSpawnPosition = spawnPosition;

    //        SpawnObject();
    //    }

    //    private IEnumerator DelaySpawn()
    //    {
    //        yield return _wait;

    //        StartSpawn(transform.position);
    //    }

    //    //private IEnumerator RespawnCoroutine(Vector3 destroyedEnemyPosition)
    //    //{
    //    //    yield return new WaitForSeconds(1f);

    //    //    Transform nearestSafeZone = _safeZoneRegistry.GetNearestSafeZone(destroyedEnemyPosition);

    //    //    if (nearestSafeZone != null)
    //    //    {
    //    //        StartSpawn(nearestSafeZone.position);
    //    //    }
    //    //}
    //}


    //public class EnemyFactoryInstaller : MonoInstaller
    //{
    //    [SerializeField] private BUGgy _bug;
    //    [SerializeField] private Crossroad _crossroad;
    //    [SerializeField] private HotRod _hotRod;

    //    public override void InstallBindings()
    //    {
    //        if (_bug == null || _crossroad == null || _hotRod == null)
    //        {
    //            Debug.LogError("prefab/s are not assigned.");

    //            return;
    //        }

    //        Dictionary<Type, Enemy> enemyPrefab = new Dictionary<Type, Enemy>
    //    {
    //        { typeof(BUGgy), _bug },
    //        { typeof(Crossroad), _crossroad },
    //        { typeof(HotRod), _hotRod },

    //    };

    //        Container.Bind<IFactory<Enemy>>().To<Factory<Enemy>>().AsSingle().WithArguments(enemyPrefab);
    //    }
    //}

    #endregion

    #region SafeZone

    //public class SafeZoneRegistryInstaller : MonoInstaller
    //{
    //    [SerializeField] private SafeZoneRegistry _registry;

    //    public override void InstallBindings()
    //    {
    //        Container.Bind<SafeZoneRegistry>().FromInstance(_registry).AsSingle().NonLazy();
    //    }
    //}

    //public class SafeZoneRegistry : MonoBehaviour
    //{
    //    [SerializeField] private Transform[] _safeZones;

    //    private void Awake()
    //    {
    //        if (_safeZones.Count() == 0)
    //        {
    //            Debug.LogWarning("_safeZones is empty");
    //            enabled = false;
    //        }
    //    }

    //    public Transform GetNearestSafeZone(Vector3 destroyedEnemyPosition)
    //    {
    //        Transform nearestZone = null;
    //        float minDistance = float.MaxValue;

    //        foreach (var zone in _safeZones)
    //        {
    //            if (zone == null)
    //            {
    //                continue;
    //            }

    //            float distance = Vector3.Distance(destroyedEnemyPosition, zone.position);

    //            if (distance < minDistance)
    //            {
    //                minDistance = distance;
    //                nearestZone = zone;
    //            }
    //        }

    //        return nearestZone;
    //    }
    //}

    #endregion

    #region PlayerDamageHandler

    //private Player _player;

    //public event Action<Player> Died;

    //protected override void Awake()
    //{
    //    base.Awake();

    //    _player = GetComponent<Player>();
    //}

    //private void OnEnable()
    //{
    //    ResetHealth();
    //}

    //protected override void OnDeath()
    //{
    //    Died?.Invoke(_player);
    //}

    #endregion

    #region ArcadeVehicleControllerBase

    //public class ArcadeVehicleControllerBase : MonoBehaviour
    //{
    //    public enum groundCheck { rayCast, sphereCaste };
    //    public enum MovementMode { Velocity, AngularVelocity };
    //    public MovementMode movementMode;
    //    public groundCheck GroundCheck;
    //    public LayerMask drivableSurface;

    //    public float MaxSpeed, acceleration, turn, gravity = 7f, downforce = 5f;
    //    [Tooltip("if true : can turn vehicle in air")]
    //    public bool AirControl = false;
    //    [Tooltip("if true : vehicle will drift instead of brake while holding space")]
    //    public bool kartLike = false;
    //    [Tooltip("turn more while drifting (while holding space) only if kart Like is true")]
    //    public float driftMultiplier = 1.5f;

    //    public Rigidbody rb, carBody;

    //    public RaycastHit hit;
    //    public AnimationCurve frictionCurve;
    //    public AnimationCurve turnCurve;
    //    public PhysicMaterial frictionMaterial;

    //    [Header("Visuals")]
    //    public Transform BodyMesh;
    //    public Transform[] FrontWheels = new Transform[2];
    //    public Transform[] RearWheels = new Transform[2];
    //    [SerializeField] private ParticleSystem speedParticles;
    //    [SerializeField] private float speedThreshold = 10f;

    //    [HideInInspector]
    //    public Vector3 carVelocity;

    //    [Range(0, 10)]
    //    public float BodyTilt;

    //    [Header("Audio settings")]
    //    public AudioSource engineSound;
    //    [Range(0, 1)]
    //    public float minPitch;
    //    [Range(1, 3)]
    //    public float MaxPitch;
    //    public AudioSource SkidSound;

    //    [HideInInspector]
    //    public float skidWidth;

    //    private float radius, horizontalInput, verticalInput;
    //    private Vector3 origin;

    //    [Header("Boost")]
    //    private float currentMaxSpeed;
    //    private float speedBoostEndTime;

    //    private float carVelocityZ;
    //    [SerializeField] private CinemachineImpulseSource impulseSource;

    //    private void Start()
    //    {
    //        currentMaxSpeed = MaxSpeed;
    //        radius = rb.GetComponent<SphereCollider>().radius;

    //        if (movementMode == MovementMode.AngularVelocity)
    //        {
    //            Physics.defaultMaxAngularSpeed = 100;
    //        }
    //    }

    //    private void Update()
    //    {
    //        horizontalInput = Input.GetAxis("Horizontal");
    //        verticalInput = Input.GetAxis("Vertical");
    //        Visuals();
    //        AudioManager();
    //    }

    //    public void AudioManager()
    //    {
    //        engineSound.pitch = Mathf.Lerp(minPitch, MaxPitch, Mathf.Abs(carVelocity.z) / MaxSpeed);

    //        if (Mathf.Abs(carVelocity.x) > 10 && IsGrounded())
    //        {
    //            SkidSound.mute = false;
    //        }
    //        else
    //        {
    //            SkidSound.mute = true;
    //        }
    //    }

    //    void FixedUpdate()
    //    {
    //        if (Time.time < speedBoostEndTime)
    //        {
    //        }
    //        else
    //        {
    //            currentMaxSpeed = MaxSpeed;
    //        }

    //        carVelocity = carBody.transform.InverseTransformDirection(carBody.velocity);



    //        if (carVelocity.z > speedThreshold)
    //        {
    //            if (!speedParticles.isPlaying)
    //            {
    //                speedParticles.Play();
    //            }
    //        }
    //        else
    //        {
    //            if (speedParticles.isPlaying)
    //            {
    //                speedParticles.Stop();
    //            }
    //        }

    //        if (Mathf.Abs(carVelocity.x) > 0)
    //        {
    //            frictionMaterial.dynamicFriction = frictionCurve.Evaluate(Mathf.Abs(carVelocity.x / 100));
    //        }

    //        if (IsGrounded())
    //        {
    //            float sign = Mathf.Sign(carVelocity.z);
    //            float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);

    //            if (kartLike && Input.GetAxis("Jump") > 0.1f)
    //            {
    //                TurnMultiplyer *= driftMultiplier;
    //            }

    //            if (verticalInput > 0.1f || carVelocity.z > 1)
    //            {
    //                carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
    //            }
    //            else if (verticalInput < -0.1f || carVelocity.z < -1)
    //            {
    //                carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
    //            }

    //            if (!kartLike)
    //            {
    //                if (Input.GetAxis("Jump") > 0.1f)
    //                {
    //                    rb.constraints = RigidbodyConstraints.FreezeRotationX;
    //                }
    //                else
    //                {
    //                    rb.constraints = RigidbodyConstraints.None;
    //                }
    //            }

    //            if (movementMode == MovementMode.AngularVelocity)
    //            {
    //                if (Mathf.Abs(verticalInput) > 0.1f && Input.GetAxis("Jump") < 0.1f && !kartLike)
    //                {
    //                    rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, carBody.transform.right * verticalInput * currentMaxSpeed / radius, acceleration * Time.deltaTime);
    //                }
    //                else if (Mathf.Abs(verticalInput) > 0.1f && kartLike)
    //                {
    //                    rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, carBody.transform.right * verticalInput * currentMaxSpeed / radius, acceleration * Time.deltaTime);
    //                }
    //            }
    //            else if (movementMode == MovementMode.Velocity)
    //            {
    //                if (Mathf.Abs(verticalInput) > 0.1f && Input.GetAxis("Jump") < 0.1f && !kartLike)
    //                {
    //                    rb.velocity = Vector3.Lerp(rb.velocity, carBody.transform.forward * verticalInput * currentMaxSpeed, acceleration / 10 * Time.deltaTime);
    //                }
    //                else if (Mathf.Abs(verticalInput) > 0.1f && kartLike)
    //                {
    //                    rb.velocity = Vector3.Lerp(rb.velocity, carBody.transform.forward * verticalInput * currentMaxSpeed, acceleration / 10 * Time.deltaTime);
    //                }
    //            }

    //            rb.AddForce(-transform.up * downforce * rb.mass);

    //            carBody.MoveRotation(Quaternion.Slerp(carBody.rotation,
    //            Quaternion.FromToRotation(carBody.transform.up, hit.normal) * carBody.transform.rotation, 0.5f));
    //        }
    //        else
    //        {
    //            if (AirControl)
    //            {
    //                float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);
    //                carBody.AddTorque(Vector3.up * horizontalInput * turn * 100 * TurnMultiplyer);
    //            }

    //            carBody.MoveRotation(Quaternion.Slerp(carBody.rotation,
    //            Quaternion.FromToRotation(carBody.transform.up, Vector3.up) * carBody.transform.rotation, 0.02f));

    //            rb.velocity = Vector3.Lerp(rb.velocity,
    //            rb.velocity + Vector3.down * gravity,
    //            Time.deltaTime * gravity);
    //        }

    //    }

    //    public void ApplySpeedBoost(float amount, float duration)
    //    {
    //        Debug.Log("ApplySpeedBoost");
    //        currentMaxSpeed += amount;
    //        speedBoostEndTime = Time.time + duration;
    //    }

    //    public void Visuals()
    //    {
    //        UpdateFrontWheels();
    //        UpdateRearWheels();

    //        if (carVelocity.z > 1)
    //        {
    //            BodyMesh.localRotation = Quaternion.Slerp(
    //            BodyMesh.localRotation,
    //            Quaternion.Euler(
    //            Mathf.Lerp(0, -5, carVelocity.z / MaxSpeed),
    //            BodyMesh.localRotation.eulerAngles.y,
    //            BodyTilt * horizontalInput),
    //            0.4f * Time.deltaTime / Time.fixedDeltaTime);
    //        }
    //        else
    //        {
    //            BodyMesh.localRotation = Quaternion.Slerp(
    //            BodyMesh.localRotation,
    //            Quaternion.Euler(0, 0, 0),
    //            0.4f * Time.deltaTime / Time.fixedDeltaTime);
    //        }

    //        if (kartLike)
    //        {
    //            if (Input.GetAxis("Jump") > 0.1f)
    //            {
    //                BodyMesh.parent.localRotation = Quaternion.Slerp(
    //                BodyMesh.parent.localRotation,
    //                Quaternion.Euler(
    //                0,
    //                45 * horizontalInput * Mathf.Sign(carVelocity.z),
    //                0),
    //                0.1f * Time.deltaTime / Time.fixedDeltaTime);
    //            }
    //            else
    //            {
    //                BodyMesh.parent.localRotation = Quaternion.Slerp(
    //                BodyMesh.parent.localRotation,
    //                Quaternion.Euler(0, 0, 0),
    //                0.1f * Time.deltaTime / Time.fixedDeltaTime);
    //            }
    //        }
    //    }
    //    protected virtual void UpdateFrontWheels()
    //    {
    //        foreach (Transform FW in FrontWheels)
    //        {
    //            FW.localRotation = Quaternion.Slerp(
    //            FW.localRotation,
    //            Quaternion.Euler(
    //            FW.localRotation.eulerAngles.x,
    //            30 * horizontalInput,
    //            FW.localRotation.eulerAngles.z),
    //            0.7f * Time.deltaTime / Time.fixedDeltaTime);

    //            FW.GetChild(0).localRotation = rb.transform.localRotation;
    //        }
    //    }

    //    protected virtual void UpdateRearWheels()
    //    {
    //        if (RearWheels != null && RearWheels.Length >= 2)
    //        {
    //            RearWheels[0].localRotation = rb.transform.localRotation;
    //            RearWheels[1].localRotation = rb.transform.localRotation;
    //        }
    //    }

    //    public bool IsGrounded()
    //    {
    //        origin = rb.position + rb.GetComponent<SphereCollider>().radius * Vector3.up;
    //        var direction = -transform.up;
    //        var maxdistance = rb.GetComponent<SphereCollider>().radius + 0.2f;

    //        if (GroundCheck == groundCheck.rayCast)
    //        {
    //            if (Physics.Raycast(rb.position, Vector3.down, out hit, maxdistance, drivableSurface))
    //            {
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        else if (GroundCheck == groundCheck.sphereCaste)
    //        {
    //            if (Physics.SphereCast(origin, radius + 0.1f, direction, out hit, maxdistance, drivableSurface))
    //            {
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        else { return false; }
    //    }

    //    private void OnDrawGizmos()
    //    {
    //        radius = rb.GetComponent<SphereCollider>().radius;
    //        float width = 0.02f;

    //        if (!Application.isPlaying)
    //        {
    //            Gizmos.color = Color.yellow;
    //            Gizmos.DrawWireCube(rb.transform.position + ((radius + width) * Vector3.down),
    //            new Vector3(2 * radius, 2 * width, 4 * radius));
    //            if (GetComponent<BoxCollider>())
    //            {
    //                Gizmos.color = Color.red;
    //                Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    //            }
    //        }
    //    }
    //}

    #endregion

    #region AiStuckHelper

    //public class AiStuckHelper : MonoBehaviour
    //{
    //    [SerializeField] private ArcadeAiVehicleController[] _vehicles = default;
    //    [SerializeField] private Transform[] _respawnPoints = default;
    //    [SerializeField] private float _minSpeed = 20f;
    //    [SerializeField] private float _stuckTimeout = 5f;
    //    [SerializeField] private float _offsetY = 5f;
    //    [SerializeField] private float _maxHeightToStuck = -10f;

    //    private float[] _stuckTimers;

    //    private void Awake()
    //    {
    //        if (_vehicles == null)
    //        {
    //            Debug.LogError($"List _vehicles is empty");
    //            enabled = false;
    //            return;
    //        }

    //        _stuckTimers = new float[_vehicles.Length];

    //        for (int i = 0; i < _stuckTimers.Length; i++)
    //        {
    //            _stuckTimers[i] = 0f;
    //        }
    //    }

    //    private void Start()
    //    {
    //        if (_respawnPoints == null || _respawnPoints.Length == 0)
    //        {
    //            Debug.LogWarning($"List _respawnPoints is empty");
    //        }
    //    }

    //    private void Update() // TODO: в конутину и раз в несколько кадров или в FixedUpdate
    //    {
    //        for (int i = 0; i < _vehicles.Length; i++)
    //        {
    //            var vehicle = _vehicles[i];

    //            float speed = vehicle.carBody.velocity.magnitude;

    //            if (vehicle.transform.position.y < _maxHeightToStuck)
    //            {
    //                TeleportStuckVehicle(vehicle);
    //            }

    //            if (speed < _minSpeed)
    //            {
    //                _stuckTimers[i] += Time.deltaTime;

    //                if (_stuckTimers[i] >= _stuckTimeout)
    //                {
    //                    TeleportStuckVehicle(vehicle);

    //                    _stuckTimers[i] = 0f;
    //                }
    //            }
    //            else
    //            {
    //                _stuckTimers[i] = 0f;
    //            }
    //        }
    //    }

    //    private void TeleportStuckVehicle(ArcadeAiVehicleController vehicle)
    //    {
    //        Transform nearest = FindNearestRespawnPoint(vehicle.transform.position);

    //        vehicle.rb.velocity = Vector3.zero;
    //        vehicle.rb.angularVelocity = Vector3.zero;
    //        vehicle.carBody.velocity = Vector3.zero;
    //        vehicle.carBody.angularVelocity = Vector3.zero;

    //        //vehicle.rb.position = nearest.position + Vector3.up * _offsetY;
    //        //vehicle.rb.rotation = nearest.rotation * Quaternion.Euler(180f, 180f, 0f);
    //        vehicle.carBody.position = nearest.position + Vector3.up * _offsetY;
    //        vehicle.carBody.rotation = nearest.rotation * Quaternion.Euler(90, 0, 0);
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

    #region ObjectSwither

    //public class ObjectSwitcher : MonoBehaviour
    //{
    //    [field: SerializeField] public List<GameObject> _objectsToSwitch { get; set; }

    //    public int _currentIndex = -1;

    //    private void Awake()
    //    {
    //        if (_objectsToSwitch == null || _objectsToSwitch.Count == 0)
    //        {
    //            Debug.LogWarning("List is empty");
    //            enabled = false;
    //            return;
    //        }

    //        Initialize();
    //    }

    //    public void SwitchToNextObject()
    //    {
    //        if (_currentIndex >= 0 && _currentIndex < _objectsToSwitch.Count)
    //        {
    //            _objectsToSwitch[_currentIndex].SetActive(false);
    //        }

    //        _currentIndex = (_currentIndex + 1) % _objectsToSwitch.Count;

    //        _objectsToSwitch[_currentIndex].SetActive(true);
    //    }

    //    public void SwitchToPreviousObject()
    //    {
    //        if (_currentIndex >= 0 && _currentIndex < _objectsToSwitch.Count)
    //        {
    //            _objectsToSwitch[_currentIndex].SetActive(false);
    //        }

    //        _currentIndex = (_currentIndex - 1 + _objectsToSwitch.Count) % _objectsToSwitch.Count;

    //        _objectsToSwitch[_currentIndex].SetActive(true);
    //    }

    //    public void Initialize()
    //    {
    //        _currentIndex = -1;

    //        SwitchToNextObject();
    //    }
    //}

    #endregion

    #region CarShop

    //public class CarShop : MonoBehaviour
    //{
    //    [SerializeField] private Wallet _wallet = null;
    //    [SerializeField] private CarCatalog _carCatalog = null;
    //    [SerializeField] private ObjectSwitcher _objectSwitcher = null;

    //    [Header("TextMeshPro References")]
    //    [SerializeField] private TextMeshPro _carNameText = null;
    //    [SerializeField] private TextMeshPro _carPriceText = null;
    //    [SerializeField] private TextMeshPro _playerMoneyText = null;

    //    [Header("Parameters")]
    //    [SerializeField] private TextMeshPro _SpeedText = null;
    //    [SerializeField] private TextMeshPro _AccelerationText = null;
    //    [SerializeField] private TextMeshPro _TurnText = null;
    //    [SerializeField] private TextMeshPro _ArmorText = null;
    //    [SerializeField] private TextMeshPro _WeaponText = null;

    //    public event Action<int> CarPurchased;

    //    private void Awake()
    //    {
    //        if (_wallet == null)
    //        {
    //            Debug.LogError("Shop: Wallet не назначен!", this);
    //            enabled = false;
    //            return;
    //        }

    //        if (_carCatalog == null)
    //        {
    //            Debug.LogError("Shop: CarCatalog не назначен!", this);
    //            enabled = false;
    //            return;
    //        }

    //        if (_objectSwitcher == null)
    //        {
    //            Debug.LogError("Shop: ObjectSwitcher не назначен!", this);
    //            enabled = false;
    //            return;
    //        }

    //        if (_carNameText == null || _carPriceText == null || _playerMoneyText == null || _SpeedText == null || _AccelerationText == null || _TurnText == null || _ArmorText == null || _WeaponText == null)
    //        {
    //            Debug.LogError("Shop: не все поля TextMeshPro заполнены!", this);
    //            enabled = false;
    //            return;
    //        }

    //        _objectSwitcher.Init();
    //        UpdateUI();
    //    }

    //    public void BuyCurrentCar()
    //    {
    //        if (_carCatalog.GetCount() == 0)
    //        {
    //            Debug.Log("Shop: Все машины уже куплены!");
    //            return;
    //        }

    //        int currentIndex = _objectSwitcher.GetCurrentIndex();

    //        CarData data = _carCatalog.GetCarData(currentIndex);

    //        if (data == null)
    //        {
    //            Debug.LogError("Shop: CarData = null или некорректный индекс.", this);
    //            return;
    //        }

    //        int price = data.Price;

    //        if (_wallet.TrySpendMoney(price))
    //        {
    //            Debug.Log($"Shop: Куплена машина '{data.CarName}' за {price}.");

    //            CarPurchased?.Invoke(currentIndex);

    //            _carCatalog.RemoveCarAtIndex(currentIndex);
    //            _objectSwitcher.RemoveCarAtIndex(currentIndex);

    //            UpdateUI();
    //        }
    //        else
    //        {
    //            Debug.Log("Shop: Недостаточно денег!");
    //        }
    //    }

    //    public void SwitchNextCar()
    //    {
    //        if (_carCatalog.GetCount() == 0)
    //        {
    //            Debug.Log("Shop: Все машины уже куплены!");
    //            return;
    //        }

    //        _objectSwitcher.SwitchToNextObject();
    //        UpdateUI();
    //    }

    //    public void SwitchPreviousCar()
    //    {
    //        if (_carCatalog.GetCount() == 0)
    //        {
    //            Debug.Log("Shop: Все машины уже куплены!");
    //            return;
    //        }

    //        _objectSwitcher.SwitchToPreviousObject();
    //        UpdateUI();
    //    }

    //    private void UpdateUI()
    //    {
    //        if (_carCatalog.GetCount() == 0)
    //        {
    //            _carNameText.text = "Машин нет!";
    //            _carPriceText.text = "0";
    //            _SpeedText.text = "0";
    //            _AccelerationText.text = "0";
    //            _TurnText.text = "0";
    //            _ArmorText.text = "0";
    //            _WeaponText.text = "0";
    //            _playerMoneyText.text = _wallet.CurrentMoney.ToString();
    //            return;
    //        }

    //        int idx = _objectSwitcher.GetCurrentIndex();

    //        if (idx < 0 || idx >= _carCatalog.GetCount())
    //        {
    //            Debug.LogError("Shop: текущий индекс недопустим!", this);
    //            return;
    //        }

    //        var carData = _carCatalog.GetCarData(idx);

    //        if (carData != null)
    //        {
    //            _carNameText.text = carData.CarName;
    //            _carPriceText.text = carData.Price.ToString();
    //            _SpeedText.text = carData.Speed.ToString();
    //            _AccelerationText.text = carData.Acceleration.ToString();
    //            _TurnText.text = carData.Turn.ToString();
    //            _ArmorText.text = carData.Armor.ToString();
    //            _WeaponText.text = carData.Weapon.ToString();
    //        }
    //        else
    //        {
    //            _carNameText.text = "Машина не найдена";
    //            _carPriceText.text = "0";
    //        }

    //        _playerMoneyText.text = _wallet.CurrentMoney.ToString();
    //    }
    //}

    #endregion

    #region Waller

    //public class Wallet : MonoBehaviour
    //{
    //    [SerializeField] private int _startMoney = 1500;

    //    private int _currentMoney;

    //    private void Awake()
    //    {
    //        _currentMoney = _startMoney;
    //    }

    //    public int CurrentMoney => _currentMoney;

    //    public bool TrySpendMoney(int amount)
    //    {
    //        if (amount < 0)
    //        {
    //            Debug.LogError("Wallet: сумма для списания не может быть отрицательной!", this);

    //            return false;
    //        }

    //        if (_currentMoney >= amount)
    //        {
    //            _currentMoney -= amount;
    //            return true;
    //        }

    //        return false;
    //    }

    //    public void AddMoney(int amount)
    //    {
    //        if (amount < 0)
    //        {
    //            Debug.LogError("Wallet: сумма для добавления не может быть отрицательной!", this);
    //            return;
    //        }
    //        _currentMoney += amount;
    //    }
    //}

    #endregion

    #region CarShop2

    //public class CarShop : MonoBehaviour
    //{
    //    [SerializeField] private CarCatalog _carCatalog = null;
    //    [SerializeField] private ObjectSwitcher _objectSwitcher = null;

    //    [Inject] private SaveManager _saveManager;
    //    private CarShopUI _carShopUI;

    //    private void Awake()
    //    {
    //        if (_carCatalog == null || _objectSwitcher == null)
    //        {
    //            Debug.LogError("Shop: Не назначены необходимые компоненты!", this);
    //            enabled = false;
    //            return;
    //        }

    //        _carShopUI = GetComponent<CarShopUI>();

    //        if (_carShopUI == null)
    //        {
    //            Debug.LogError("Shop: CarShopUI не найден!", this);
    //            enabled = false;
    //            return;
    //        }

    //        _objectSwitcher.Init();
    //        UpdateUI();
    //    }

    //    public void BuyCurrentCar()
    //    {
    //        if (_carCatalog.GetCount() == 0)
    //        {
    //            Debug.Log("Shop: Все машины уже куплены!");
    //            return;
    //        }

    //        int currentIndex = _objectSwitcher.GetCurrentIndex();
    //        CarData data = _carCatalog.GetCarData(currentIndex);

    //        if (data == null)
    //        {
    //            Debug.LogError("Shop: CarData = null или некорректный индекс.", this);
    //            return;
    //        }

    //        int price = data.Price;

    //        if (_saveManager.TrySpendMoney(price))
    //        {
    //            Debug.Log($"Shop: Куплена машина '{data.CarName}' за {price}.");

    //            _saveManager.AddCar(data.Id);
    //            _carCatalog.RemoveCarAtIndex(currentIndex);
    //            _objectSwitcher.RemoveCarAtIndex(currentIndex);

    //            _saveManager.Save();

    //            UpdateUI();
    //        }
    //        else
    //        {
    //            Debug.Log("Shop: Недостаточно денег!");
    //        }
    //    }

    //    public void SwitchNextCar()
    //    {
    //        if (_carCatalog.GetCount() == 0)
    //        {
    //            Debug.Log("Shop: Все машины уже куплены!");
    //            return;
    //        }

    //        _objectSwitcher.SwitchToNextObject();
    //        UpdateUI();
    //    }

    //    public void SwitchPreviousCar()
    //    {
    //        if (_carCatalog.GetCount() == 0)
    //        {
    //            Debug.Log("Shop: Все машины уже куплены!");
    //            return;
    //        }

    //        _objectSwitcher.SwitchToPreviousObject();
    //        UpdateUI();
    //    }

    //    private void UpdateUI()
    //    {
    //        if (_carCatalog.GetCount() == 0)
    //        {
    //            _carShopUI.DisplayNoCarsAvailable();
    //            return;
    //        }

    //        int idx = _objectSwitcher.GetCurrentIndex();
    //        if (idx < 0 || idx >= _carCatalog.GetCount())
    //        {
    //            Debug.LogError("Shop: текущий индекс недопустим!", this);
    //            return;
    //        }

    //        var carData = _carCatalog.GetCarData(idx);
    //        if (carData != null)
    //        {
    //            _carShopUI.DisplayCarData(carData);
    //        }
    //        else
    //        {
    //            _carShopUI.DisplayCarNotFound();
    //        }

    //        _carShopUI.UpdatePlayerMoney(_saveManager.Money);
    //    }
    //}

    #endregion

    #region PlayerCarController

    //public class ArcadeVehicleController : MonoBehaviour, ISpeedBoostable
    //{
    //    public enum groundCheck { rayCast, sphereCaste };
    //    public enum MovementMode { Velocity, AngularVelocity };

    //    public MovementMode movementMode;
    //    public groundCheck GroundCheck;
    //    public LayerMask drivableSurface;

    //    public float MaxSpeed, acceleration, turn, gravity = 7f, downforce = 5f;
    //    [Tooltip("if true : can turn vehicle in air")]
    //    public bool AirControl = false;
    //    [Tooltip("if true : vehicle will drift instead of brake while holding space")]
    //    public bool kartLike = false;
    //    [Tooltip("turn more while drifting (while holding space) only if kart Like is true")]
    //    public float driftMultiplier = 1.5f;

    //    public Rigidbody rb, carBody;

    //    public RaycastHit hit;
    //    public AnimationCurve frictionCurve;
    //    public AnimationCurve turnCurve;
    //    public PhysicMaterial frictionMaterial;

    //    [Header("Visuals")]
    //    public Transform BodyMesh;
    //    public Transform[] FrontWheels = new Transform[2];
    //    public Transform[] RearWheels = new Transform[2];
    //    //[SerializeField] private ParticleSystem speedParticles;
    //    //[SerializeField] private float speedThreshold = 10f;

    //    [HideInInspector]
    //    public Vector3 carVelocity;

    //    [Range(0, 10)]
    //    public float BodyTilt;

    //    [Header("Audio settings")]
    //    public AudioSource engineSound;
    //    [Range(0, 1)]
    //    public float minPitch;
    //    [Range(1, 3)]
    //    public float MaxPitch;
    //    public AudioSource SkidSound;

    //    [HideInInspector]
    //    public float skidWidth;

    //    private float radius, horizontalInput, verticalInput;
    //    private Vector3 origin;

    //    [Header("Boost")]
    //    private float currentMaxSpeed;
    //    private float speedBoostEndTime;

    //    private float carVelocityZ;

    //    private void Start()
    //    {
    //        currentMaxSpeed = MaxSpeed;

    //        radius = rb.GetComponent<SphereCollider>().radius;

    //        if (movementMode == MovementMode.AngularVelocity)
    //        {
    //            Physics.defaultMaxAngularSpeed = 100;
    //        }
    //    }

    //    private void Update()
    //    {
    //        horizontalInput = Input.GetAxis("Horizontal");
    //        verticalInput = Input.GetAxis("Vertical");
    //        Visuals();
    //        AudioManager();
    //    }

    //    public void AudioManager()
    //    {
    //        engineSound.pitch = Mathf.Lerp(minPitch, MaxPitch, Mathf.Abs(carVelocity.z) / MaxSpeed);

    //        if (Mathf.Abs(carVelocity.x) > 10 && IsGrounded())
    //        {
    //            SkidSound.mute = false;
    //        }
    //        else
    //        {
    //            SkidSound.mute = true;
    //        }
    //    }

    //    void FixedUpdate()
    //    {
    //        if (Time.time < speedBoostEndTime)
    //        {
    //        }
    //        else
    //        {
    //            currentMaxSpeed = MaxSpeed;
    //        }

    //        carVelocity = carBody.transform.InverseTransformDirection(carBody.velocity);



    //        //if (carVelocity.z > speedThreshold)
    //        //{
    //        //    if (!speedParticles.isPlaying)
    //        //    {
    //        //        speedParticles.Play();
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    if (speedParticles.isPlaying)
    //        //    {
    //        //        speedParticles.Stop();
    //        //    }
    //        //}

    //        if (Mathf.Abs(carVelocity.x) > 0)
    //        {
    //            frictionMaterial.dynamicFriction = frictionCurve.Evaluate(Mathf.Abs(carVelocity.x / 100));
    //        }

    //        if (IsGrounded())
    //        {
    //            float sign = Mathf.Sign(carVelocity.z);
    //            float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);

    //            if (kartLike && Input.GetAxis("Jump") > 0.1f)
    //            {
    //                TurnMultiplyer *= driftMultiplier;
    //            }

    //            if (verticalInput > 0.1f || carVelocity.z > 1)
    //            {
    //                carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
    //            }
    //            else if (verticalInput < -0.1f || carVelocity.z < -1)
    //            {
    //                carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
    //            }

    //            if (!kartLike)
    //            {
    //                if (Input.GetAxis("Jump") > 0.1f)
    //                {
    //                    rb.constraints = RigidbodyConstraints.FreezeRotationX;
    //                }
    //                else
    //                {
    //                    rb.constraints = RigidbodyConstraints.None;
    //                }
    //            }

    //            if (movementMode == MovementMode.AngularVelocity)
    //            {
    //                if (Mathf.Abs(verticalInput) > 0.1f && Input.GetAxis("Jump") < 0.1f && !kartLike)
    //                {
    //                    rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, carBody.transform.right * verticalInput * currentMaxSpeed / radius, acceleration * Time.deltaTime);
    //                }
    //                else if (Mathf.Abs(verticalInput) > 0.1f && kartLike)
    //                {
    //                    rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, carBody.transform.right * verticalInput * currentMaxSpeed / radius, acceleration * Time.deltaTime);
    //                }
    //            }
    //            else if (movementMode == MovementMode.Velocity)
    //            {
    //                if (Mathf.Abs(verticalInput) > 0.1f && Input.GetAxis("Jump") < 0.1f && !kartLike)
    //                {
    //                    rb.velocity = Vector3.Lerp(rb.velocity, carBody.transform.forward * verticalInput * currentMaxSpeed, acceleration / 10 * Time.deltaTime);
    //                }
    //                else if (Mathf.Abs(verticalInput) > 0.1f && kartLike)
    //                {
    //                    rb.velocity = Vector3.Lerp(rb.velocity, carBody.transform.forward * verticalInput * currentMaxSpeed, acceleration / 10 * Time.deltaTime);
    //                }
    //            }

    //            rb.AddForce(-transform.up * downforce * rb.mass);

    //            carBody.MoveRotation(Quaternion.Slerp(carBody.rotation,
    //            Quaternion.FromToRotation(carBody.transform.up, hit.normal) * carBody.transform.rotation, 0.5f));
    //        }
    //        else
    //        {
    //            if (AirControl)
    //            {
    //                float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);
    //                carBody.AddTorque(Vector3.up * horizontalInput * turn * 100 * TurnMultiplyer);
    //            }

    //            carBody.MoveRotation(Quaternion.Slerp(carBody.rotation,
    //            Quaternion.FromToRotation(carBody.transform.up, Vector3.up) * carBody.transform.rotation, 0.02f));

    //            rb.velocity = Vector3.Lerp(rb.velocity,
    //            rb.velocity + Vector3.down * gravity,
    //            Time.deltaTime * gravity);
    //        }

    //    }

    //    public void ApplySpeedBoost(float amount, float duration)
    //    {
    //        currentMaxSpeed += amount;
    //        speedBoostEndTime = Time.time + duration;
    //    }

    //    public void Visuals()
    //    {
    //        UpdateFrontWheels();
    //        UpdateRearWheels();

    //        if (carVelocity.z > 1)
    //        {
    //            BodyMesh.localRotation = Quaternion.Slerp(
    //            BodyMesh.localRotation,
    //            Quaternion.Euler(
    //            Mathf.Lerp(0, -5, carVelocity.z / MaxSpeed),
    //            BodyMesh.localRotation.eulerAngles.y,
    //            BodyTilt * horizontalInput),
    //            0.4f * Time.deltaTime / Time.fixedDeltaTime);
    //        }
    //        else
    //        {
    //            BodyMesh.localRotation = Quaternion.Slerp(
    //            BodyMesh.localRotation,
    //            Quaternion.Euler(0, 0, 0),
    //            0.4f * Time.deltaTime / Time.fixedDeltaTime);
    //        }

    //        if (kartLike)
    //        {
    //            if (Input.GetAxis("Jump") > 0.1f)
    //            {
    //                BodyMesh.parent.localRotation = Quaternion.Slerp(
    //                BodyMesh.parent.localRotation,
    //                Quaternion.Euler(
    //                0,
    //                45 * horizontalInput * Mathf.Sign(carVelocity.z),
    //                0),
    //                0.1f * Time.deltaTime / Time.fixedDeltaTime);
    //            }
    //            else
    //            {
    //                BodyMesh.parent.localRotation = Quaternion.Slerp(
    //                BodyMesh.parent.localRotation,
    //                Quaternion.Euler(0, 0, 0),
    //                0.1f * Time.deltaTime / Time.fixedDeltaTime);
    //            }
    //        }
    //    }
    //    protected virtual void UpdateFrontWheels()
    //    {
    //        foreach (Transform FW in FrontWheels)
    //        {
    //            FW.localRotation = Quaternion.Slerp(
    //            FW.localRotation,
    //            Quaternion.Euler(
    //            FW.localRotation.eulerAngles.x,
    //            30 * horizontalInput,
    //            FW.localRotation.eulerAngles.z),
    //            0.7f * Time.deltaTime / Time.fixedDeltaTime);

    //            FW.GetChild(0).localRotation = rb.transform.localRotation;
    //        }
    //    }

    //    protected virtual void UpdateRearWheels()
    //    {
    //        if (RearWheels != null && RearWheels.Length >= 2)
    //        {
    //            RearWheels[0].localRotation = rb.transform.localRotation;
    //            RearWheels[1].localRotation = rb.transform.localRotation;
    //        }
    //    }

    //    public bool IsGrounded()
    //    {
    //        origin = rb.position + rb.GetComponent<SphereCollider>().radius * Vector3.up;
    //        var direction = -transform.up;
    //        var maxdistance = rb.GetComponent<SphereCollider>().radius + 0.2f;

    //        if (GroundCheck == groundCheck.rayCast)
    //        {
    //            if (Physics.Raycast(rb.position, Vector3.down, out hit, maxdistance, drivableSurface))
    //            {
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        else if (GroundCheck == groundCheck.sphereCaste)
    //        {
    //            if (Physics.SphereCast(origin, radius + 0.1f, direction, out hit, maxdistance, drivableSurface))
    //            {
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        else { return false; }
    //    }

    //    private void OnDrawGizmos()
    //    {
    //        radius = rb.GetComponent<SphereCollider>().radius;
    //        float width = 0.02f;

    //        if (!Application.isPlaying)
    //        {
    //            Gizmos.color = Color.yellow;
    //            Gizmos.DrawWireCube(rb.transform.position + ((radius + width) * Vector3.down),
    //            new Vector3(2 * radius, 2 * width, 4 * radius));
    //            if (GetComponent<BoxCollider>())
    //            {
    //                Gizmos.color = Color.red;
    //                Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    //            }
    //        }
    //    }
    //}
    #endregion

    #region RaceProgressTraker

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

    #endregion

    #region GarageCarOverview

    //public class GarageCarOverview : MonoBehaviour
    //{
    //    [SerializeField] private List<GarageCarItem> _garageCars = null;

    //    [Inject] private SaveManager _saveManager = null;

    //    private CarShowcase _carShowcase = null;

    //    public CarUpgrades CurrentCarUpgrades => _carShowcase?.GetCurrentCarUpgrades();

    //    private void Awake()
    //    {
    //        if (_garageCars == null)
    //        {
    //            Debug.LogError("GarageCarOverview: список _garageCars не назначен.", this);
    //            enabled = false;
    //            return;
    //        }

    //        if (_garageCars.Count == 0)
    //        {
    //            Debug.LogError("GarageCarOverview: список _garageCars пуст.", this);
    //            enabled = false;
    //            return;
    //        }

    //        if (_saveManager == null)
    //        {
    //            Debug.LogError("GarageCarOverview: _saveManager не назначен.", this);
    //            enabled = false;
    //            return;
    //        }

    //        HideAllCars();
    //    }

    //    private void Start()
    //    {
    //        _carShowcase = new CarShowcase(_garageCars, _saveManager);

    //        int firstIndex = _carShowcase.FindFirstPurchasedCarIndex();

    //        if (firstIndex < 0)
    //        {
    //            Debug.LogWarning("GarageCarOverview: нет купленных машин.", this);
    //            return;
    //        }

    //        _carShowcase.SetCurrentIndex(firstIndex);

    //        ShowCar(firstIndex);
    //    }

    //    public void ShowNextPurchasedCar()
    //    {
    //        if (_carShowcase == null)
    //            return;

    //        int newIndex = _carShowcase.MoveToNextPurchased();

    //        if (newIndex >= 0)
    //        {
    //            ShowCar(newIndex);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("GarageCarOverview: нет купленных машин для переключения.", this);
    //        }
    //    }

    //    public void ShowPreviousPurchasedCar()
    //    {
    //        if (_carShowcase == null)
    //            return;

    //        int newIndex = _carShowcase.MoveToPreviousPurchased();
    //        if (newIndex >= 0)
    //        {
    //            ShowCar(newIndex);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("GarageCarOverview: нет купленных машин для переключения.", this);
    //        }
    //    }

    //    public CarUpgrades RetrieveCurrentCarUpgrades()
    //    {
    //        return CurrentCarUpgrades;
    //    }

    //    private void ShowCar(int index)
    //    {
    //        HideAllCars();

    //        GarageCarItem carItem = _carShowcase.GetCurrentCarItem();

    //        if (carItem == null)
    //        {
    //            Debug.LogError($"ShowCar: Не удалось получить машину по индексу {index}.", this);
    //            return;
    //        }

    //        if (carItem.carObject != null)
    //        {
    //            carItem.carObject.SetActive(true);
    //        }

    //        if (carItem.carUpgrades != null)
    //        {
    //            carItem.carUpgrades.InitializePurchasedUpgrades(_saveManager.HasCarUpgrade);
    //        }

    //        _carShowcase.SaveLastUsedCarId(carItem.carId);
    //    }

    //    private void HideAllCars()
    //    {
    //        for (int i = 0; i < _garageCars.Count; i++)
    //        {
    //            if (_garageCars[i].carObject != null)
    //            {
    //                _garageCars[i].carObject.SetActive(false);
    //            }
    //        }
    //    }
    //}

    #endregion

    #region "Garage" CarShowcase

    //internal sealed class CarShowcase
    //{
    //    private readonly List<GarageCarItem> _cars;
    //    private readonly SaveManager _saveManager;
    //    private int _currentIndex = -1;

    //    internal CarShowcase(List<GarageCarItem> cars, SaveManager saveManager)
    //    {
    //        if (cars == null)
    //        {
    //            Debug.LogError("CarShowcase: список машин (cars) = null.");
    //            throw new System.ArgumentNullException(nameof(cars));
    //        }
    //        if (cars.Count == 0)
    //        {
    //            Debug.LogError("CarShowcase: список машин (cars) пуст.");
    //            throw new System.ArgumentException("Cars list is empty.");
    //        }
    //        if (saveManager == null)
    //        {
    //            Debug.LogError("CarShowcase: saveManager = null.");
    //            throw new System.ArgumentNullException(nameof(saveManager));
    //        }

    //        _cars = cars;
    //        _saveManager = saveManager;
    //    }

    //    internal int CurrentIndex => _currentIndex;

    //    internal int FindFirstPurchasedCarIndex()
    //    {
    //        for (int i = 0; i < _cars.Count; i++)
    //        {
    //            if (_saveManager.HasCar(_cars[i].carId))
    //            {
    //                return i;
    //            }
    //        }
    //        return -1;
    //    }

    //    internal void SetCurrentIndex(int index)
    //    {
    //        if (index < 0 || index >= _cars.Count)
    //        {
    //            Debug.LogError($"CarShowcase: некорректный индекс {index} для SetCurrentIndex.");
    //            throw new System.ArgumentOutOfRangeException(nameof(index));
    //        }
    //        _currentIndex = index;
    //    }

    //    internal int MoveToNextPurchased()
    //    {
    //        if (_currentIndex < 0)
    //            return -1;

    //        int newIndex = _currentIndex;

    //        do
    //        {
    //            newIndex = (newIndex + 1) % _cars.Count;
    //        }
    //        while (!_saveManager.HasCar(_cars[newIndex].carId) && newIndex != _currentIndex);

    //        if (!_saveManager.HasCar(_cars[newIndex].carId))
    //        {
    //            return -1;
    //        }

    //        _currentIndex = newIndex;

    //        return _currentIndex;
    //    }

    //    internal int MoveToPreviousPurchased()
    //    {
    //        if (_currentIndex < 0)
    //            return -1;

    //        int newIndex = _currentIndex;

    //        do
    //        {
    //            newIndex = (newIndex - 1 + _cars.Count) % _cars.Count;
    //        }
    //        while (!_saveManager.HasCar(_cars[newIndex].carId) && newIndex != _currentIndex);

    //        if (!_saveManager.HasCar(_cars[newIndex].carId))
    //        {
    //            return -1;
    //        }

    //        _currentIndex = newIndex;

    //        return _currentIndex;
    //    }

    //    internal void SaveLastUsedCarId(int carId)
    //    {
    //        _saveManager.LastUsedCarId = carId;
    //        _saveManager.Save();
    //    }

    //    internal CarUpgrades GetCurrentCarUpgrades()
    //    {
    //        if (_currentIndex < 0 || _currentIndex >= _cars.Count)
    //        {
    //            return null;
    //        }
    //        return _cars[_currentIndex].carUpgrades;
    //    }

    //    internal GarageCarItem GetCurrentCarItem()
    //    {
    //        if (_currentIndex < 0 || _currentIndex >= _cars.Count)
    //            return null;

    //        return _cars[_currentIndex];
    //    }
    //}

    #endregion

    #region MoveToGround(Mine)

    //public class MoveToGround : MonoBehaviour
    //{
    //    [SerializeField] private LayerMask _groundLayer;
    //    [SerializeField] private float _maxDistance = 10f;
    //    [SerializeField] private float _speed = 5f;
    //    [SerializeField] private float _offSet = 0.1f;

    //    private bool _isFalling = true;

    //    void Update()
    //    {
    //        if (!_isFalling) return;

    //        FindSurface();
    //    }

    //    private void FindSurface()
    //    {
    //        Vector3 origin = transform.position + Vector3.up * _offSet;

    //        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _maxDistance, _groundLayer))
    //        {
    //            float distanceToGround = hit.distance - _offSet;

    //            transform.position = Vector3.MoveTowards(transform.position, hit.point + Vector3.up * _offSet, _speed * Time.deltaTime);

    //            if (distanceToGround <= _offSet)
    //            {
    //                transform.position = hit.point + Vector3.up * _offSet;
    //                _isFalling = false;
    //            }
    //        }
    //        else
    //        {
    //            transform.position += Vector3.down * _speed * Time.deltaTime;
    //        }
    //    }
    //}

    #endregion

    #region SaveService

    //public class SaveService : IInitializable
    //{
    //    public int Money
    //    {
    //        get => YandexGame.savesData.money;
    //        private set
    //        {
    //            YandexGame.savesData.money = value;
    //            OnMoneyChanged?.Invoke();
    //        }
    //    }

    //    public List<int> PurchasedCarIDs => YandexGame.savesData.purchasedCarIDs;
    //    public List<PurchasedUpgrade> PurchasedUpgrades => YandexGame.savesData.purchasedUpgrades;

    //    public event Action OnMoneyChanged;

    //    public int LastUsedCarId
    //    {
    //        get => YandexGame.savesData.lastUsedCarId;
    //        set => YandexGame.savesData.lastUsedCarId = value;
    //    }

    //    public void Initialize()
    //    {
    //        Debug.Log("[SaveManager] Initialize() → Загружаем сохранения из YandexGame");

    //        YandexGame.LoadProgress();

    //        if (YandexGame.savesData.isFirstSession)
    //        {
    //            // Можно дать стартовые деньги и прочие параметры
    //            YandexGame.savesData.isFirstSession = false;
    //            // Money = 1500; // пример
    //            // SaveProgress();
    //        }
    //    }

    //    #region Money

    //    public bool TrySpendMoney(int amount)
    //    {
    //        if (amount < 0)
    //        {
    //            Debug.LogError("[SaveManager] Нельзя списывать отрицательную сумму!");
    //            return false;
    //        }

    //        if (Money >= amount)
    //        {
    //            Money -= amount;
    //            return true;
    //        }
    //        return false;
    //    }

    //    public void AddMoney(int amount)
    //    {
    //        if (amount < 0)
    //        {
    //            Debug.LogError("[SaveManager] Нельзя добавлять отрицательную сумму!");
    //            return;
    //        }

    //        Money += amount;
    //    }

    //    #endregion

    //    #region Cars

    //    public void AddCar(int carId)
    //    {
    //        if (!PurchasedCarIDs.Contains(carId))
    //        {
    //            PurchasedCarIDs.Add(carId);
    //        }
    //    }

    //    public bool HasCar(int carId)
    //    {
    //        return PurchasedCarIDs.Contains(carId);
    //    }

    //    #endregion

    //    #region Upgrades

    //    public bool HasCarUpgrade(int carId, int upgradeId)
    //    {
    //        for (int i = 0; i < PurchasedUpgrades.Count; i++)
    //        {
    //            if (PurchasedUpgrades[i].carId == carId && PurchasedUpgrades[i].upgradeId == upgradeId)
    //                return true;
    //        }

    //        return false;
    //    }

    //    public void AddCarUpgrade(int carId, int upgradeId)
    //    {
    //        if (!HasCarUpgrade(carId, upgradeId))
    //        {
    //            PurchasedUpgrade record = new PurchasedUpgrade { carId = carId, upgradeId = upgradeId };
    //            PurchasedUpgrades.Add(record);
    //        }
    //    }

    //    #endregion

    //    public void Save()
    //    {
    //        YandexGame.SaveProgress();
    //        Debug.Log("[SaveManager] Сохранение выполнено!");
    //    }
    //}

    #endregion
}