using UnityEngine.Pool;
using UnityEngine;
using System;
using Reflex.Attributes;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ITerminatable
{
    [Inject] private readonly IFactory<T> _factory;

    private ObjectPool<T> _objectPool;

    protected virtual void Start()
    {
        _objectPool = new ObjectPool<T>(CreateObject, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, false, 10, 100);
    }

    private void OnDisable()
    {
        _objectPool?.Clear();
    }

    protected abstract Vector3 GetSpawnPosition();

    protected abstract Type GetObjectTypeToSpawn();

    protected virtual T CreateObject()
    {
        Type objectType = GetObjectTypeToSpawn();
        Vector3 position = GetSpawnPosition();

        T obj = _factory.Create(objectType, position);

        return obj;
    }

    protected virtual void OnGetFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.Terminated += HandleObjectDeath;

        if (obj is IResettable resettable)
        {
            resettable.ResetState();
        }
    }

    protected virtual void OnReleaseToPool(T obj)
    {
        obj.Terminated -= HandleObjectDeath;
        obj.gameObject.SetActive(false);
    }

    protected virtual void HandleObjectDeath(ITerminatable terminateObject)
    {
        if (terminateObject is T obj && obj != null)
        {
            _objectPool.Release(obj);
        }

        //_objectPool.Release((T)terminateObject);
    }

    protected virtual T SpawnObject()
    {
        T spawnedObject = _objectPool.Get();
        spawnedObject.transform.position = GetSpawnPosition();

        return spawnedObject;
    }

    private void OnDestroyPoolObject(T obj)
    {
        if (obj != null)
        {
            Destroy(obj.gameObject);
        }
    }
}