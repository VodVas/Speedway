using System;
using UnityEngine;

public class CrashPartsCreator : Spawner<VehiclePartsExploder>
{
    private Vector3 _cachedPosition;
    private Type _cachedEnemyType;

    protected override Type GetObjectTypeToSpawn()
    {
        return _cachedEnemyType != null ? _cachedEnemyType : typeof(VehiclePartsExploder);
    }

    protected override Vector3 GetSpawnPosition()
    {
        return _cachedPosition;
    }

    public void StartSpawn(Vector3 deathPosition, Type vehicleType)
    {
        _cachedPosition = deathPosition;
        _cachedEnemyType = vehicleType;

        SpawnObject();
    }
}