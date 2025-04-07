using System;
using UnityEngine;

public class CardLootCarSpawner : MultiPoolLootboxSpawner<LootCar>
{
    public LootCar SpawnCarForCard(Vector3 position, int cardIndex)
    {
        SetSpawnPosition(position);
        return GetFromCardPool(cardIndex);
    }

    protected override Vector3 GetSpawnPosition() => Vector3.zero;
    protected override Type GetObjectTypeToSpawn() => typeof(LootCar);
}