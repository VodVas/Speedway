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
}