using UnityEngine.Pool;
using UnityEngine;
using System.Collections.Generic;
using Reflex.Attributes;
using System.Collections;

public abstract class MultiPoolLootboxSpawner<T> : Spawner<T> where T : MonoBehaviour, ITerminatable
{
    [Header("Pool Prewarming")]
    [SerializeField] private int[] _prewarmCardIndexes;
    [SerializeField] private int _objectsPerPool = 3;
    [SerializeField] private bool _prewarmOnStart = true;
    [SerializeField] private float _delayBetweenPools = 0.05f;
    [SerializeField] private float _maxFrameTime = 0.016f;

    [Inject] private readonly IFactory<T> _factory;

    private Dictionary<int, ObjectPool<T>> _cardPools = new Dictionary<int, ObjectPool<T>>(8);
    private int _currentCardIndex;
    private Vector3 _spawnPosition;

    protected override void Start()
    {
        if (_prewarmOnStart)
            StartCoroutine(PrewarmPoolsRoutine());
    }

    private void OnDestroyCardObject(T obj)
    {
        if (obj != null)
        {
            Destroy(obj.gameObject);
        }
    }

    private IEnumerator PrewarmPoolsRoutine()
    {
        foreach (var cardIndex in _prewarmCardIndexes)
        {
            var pool = GetOrCreateCardPool(cardIndex);
            var timestamp = Time.realtimeSinceStartup;
            var warmedObjects = new List<T>(_objectsPerPool);

            for (int i = 0; i < _objectsPerPool; i++)
            {
                var obj = pool.Get();
                warmedObjects.Add(obj);

                if (Time.realtimeSinceStartup - timestamp > _maxFrameTime)
                {
                    yield return null;
                    timestamp = Time.realtimeSinceStartup;
                }
            }

            foreach (var obj in warmedObjects)
                pool.Release(obj);

            yield return new WaitForSeconds(_delayBetweenPools);
        }
    }

    private ObjectPool<T> GetOrCreateCardPool(int cardIndex)
    {
        if (!_cardPools.TryGetValue(cardIndex, out var pool))
        {
            pool = new ObjectPool<T>(
                CreateCardObject,
                OnGetFromCardPool,
                OnReleaseToCardPool,
                OnDestroyCardObject,
                false,
                1,
                10
            );
            _cardPools[cardIndex] = pool;
        }
        return pool;
    }

    private void OnGetFromCardPool(T obj)
    {
        obj.transform.position = _spawnPosition;
        obj.gameObject.SetActive(true);
        OnInitializeCardObject(obj, _currentCardIndex);
    }

    private void OnReleaseToCardPool(T obj) => obj.gameObject.SetActive(false);

    private T CreateCardObject()
    {
        T obj = _factory.Create(GetObjectTypeToSpawn(), _spawnPosition);
        if (!obj.TryGetComponent<PoolTracker>(out var tracker))
            throw new MissingComponentException($"Missing PoolTracker on {typeof(T).Name} prefab");

        tracker.SetCardIndex(_currentCardIndex);
        obj.Terminated += HandleTermination;
        return obj;
    }

    private void HandleTermination(ITerminatable terminatable)
    {
        if (terminatable is T obj && obj.TryGetComponent<PoolTracker>(out var tracker))
            if (_cardPools.TryGetValue(tracker.CardIndex, out var pool))
                pool.Release(obj);
    }

    protected virtual void OnInitializeCardObject(T obj, int cardIndex) { }
    protected void SetSpawnPosition(Vector3 position) => _spawnPosition = position;

    public void ClearAllCardPools()
    {
        var keys = _cardPools.Keys.GetEnumerator();
        while (keys.MoveNext())
        {
            if (_cardPools.TryGetValue(keys.Current, out var pool) && pool != null)
                pool.Clear();
        }
        _cardPools.Clear();
    }

    protected override T SpawnObject() => GetFromCardPool(0);
    protected override void HandleObjectDeath(ITerminatable terminateObject) { }


    protected T GetFromCardPool(int cardIndex)
    {
        _currentCardIndex = cardIndex;
        return GetOrCreateCardPool(cardIndex).Get();
    }
}