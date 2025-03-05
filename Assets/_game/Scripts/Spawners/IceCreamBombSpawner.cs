using System;
using UnityEngine;

public class IceCreamBombSpawner : Spawner<IceCreamBomb>
{
    protected override Type GetObjectTypeToSpawn()
    {
        return typeof(IceCreamBomb);
    }

    protected override Vector3 GetSpawnPosition()
    {
        return transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            SpawnObject();
        }
    }
}