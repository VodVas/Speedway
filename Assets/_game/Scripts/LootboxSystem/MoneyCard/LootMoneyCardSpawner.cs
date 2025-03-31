using System;
using UnityEngine;

public class LootMoneyCardSpawner : Spawner<LootMoney>
{
    private Vector3 _cachedPosition;

    public LootMoney SpawnMoneyCard(Vector3 spawnPosition)
    {
        _cachedPosition = spawnPosition;
        LootMoney money = SpawnObject();

        return money;
    }

    protected override Vector3 GetSpawnPosition()
    {
        return _cachedPosition;
    }

    protected override Type GetObjectTypeToSpawn()
    {
        return typeof(LootMoney);
    }
}