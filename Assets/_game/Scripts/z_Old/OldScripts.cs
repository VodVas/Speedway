public class OldScripts
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
}