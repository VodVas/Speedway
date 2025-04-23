using UnityEngine;
using System;

public class MineSpawner : Spawner<Detonator>
{
    private Vector3 _cachedPosition;

    protected override Type GetObjectTypeToSpawn() => typeof(Detonator);

    protected override Vector3 GetSpawnPosition() => _cachedPosition;

    public void StartSpawn(Vector3 spawnPosition)
    {
        _cachedPosition = spawnPosition;

        SpawnObject();
    }

    protected override Detonator CreateObject()
    {
        var mine = base.CreateObject();

        return mine;
    }
}