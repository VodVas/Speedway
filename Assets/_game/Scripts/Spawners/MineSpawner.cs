using System;
using UnityEngine;

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