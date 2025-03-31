using System;
using UnityEngine;

public class LootSpheresSpawner : Spawner<LootSphere>
{
    [SerializeField] private SpheresMaterialsRegistry _materialsRegistry;

    private Rarity _currentRarity;
    private Vector3 _cachedPosition;

    public LootSphere SpawnLootSphere(Rarity rarity, Vector3 spawnPos)
    {
        _currentRarity = rarity;
        _cachedPosition = spawnPos;

        LootSphere sphere = SpawnObject();
        return sphere;
    }

    protected override Vector3 GetSpawnPosition()
    {
        return _cachedPosition;
    }

    protected override Type GetObjectTypeToSpawn()
    {
        return typeof(LootSphere);
    }

    protected override void OnGetFromPool(LootSphere obj)
    {
        base.OnGetFromPool(obj);

        if (_materialsRegistry != null)
        {
            Material chosenMat = _materialsRegistry.GetRandomMaterial(_currentRarity);
            obj.SetMaterial(chosenMat);
        }
        else
        {
            Debug.LogWarning("[LootSpheresSpawner] _materialsRegistry is null!", this);
        }
    }
}