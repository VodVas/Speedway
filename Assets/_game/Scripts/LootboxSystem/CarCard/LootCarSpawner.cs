using System;
using UnityEngine;

public class LootCarSpawner : Spawner<LootCar>
{
    private Vector3 _cachedPosition;

    public LootCar SpawnLootCar(Vector3 spawnPosition)
    {
        _cachedPosition = spawnPosition;

        LootCar car = SpawnObject();
        return car;
    }

    protected override Vector3 GetSpawnPosition()
    {
        return _cachedPosition;
    }

    protected override Type GetObjectTypeToSpawn()
    {
        return typeof(LootCar);
    }
}