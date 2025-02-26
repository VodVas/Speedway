using Reflex.Attributes;
using System;
using UnityEngine;

//public class MineSpawner : Spawner<Detonator>
//{
//    [Inject] private Detonator _minePrefab;
//    private Vector3 _cachedPosition;

//    public void Initialize(Detonator minePrefab)
//    {
//        Debug.Log(" MineSpawner - Initialize");
//        _minePrefab = minePrefab;
//    }

//    public void StartSpawn(Vector3 spawnPosition)
//    {
//        _cachedPosition = spawnPosition;

//        SpawnObject();
//    }

//    protected override Type GetObjectTypeToSpawn()
//    {
//        return typeof(Detonator);
//    }

//    protected override Vector3 GetSpawnPosition()
//    {
//        return _cachedPosition;
//    }

//    protected override Detonator CreateObject()
//    {
//        if (_minePrefab == null)
//        {
//            Debug.LogError("[MineSpawner] Mine prefab is not assigned!");
//            return null;
//        }

//        Vector3 spawnPos = GetSpawnPosition();
//        Detonator mine = Instantiate(_minePrefab, spawnPos, Quaternion.identity);
//        return mine;
//    }
//}







public class MineSpawner : Spawner<Detonator>
{
    private Vector3 _cachedPosition;

    protected override Type GetObjectTypeToSpawn()
    {
        return typeof(Detonator);
    }

    protected override Vector3 GetSpawnPosition()
    {
        return _cachedPosition;
    }

    public void StartSpawn(Vector3 spawnPosition)
    {
        _cachedPosition = spawnPosition;

        SpawnObject();
    }
}