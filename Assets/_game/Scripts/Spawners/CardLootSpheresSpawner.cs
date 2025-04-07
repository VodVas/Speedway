using System;
using UnityEngine;

public class CardLootSpheresSpawner : MultiPoolLootboxSpawner<LootPaintSphere>
{
    [SerializeField] private SpheresMaterialsRegistry _materialsRegistry;
    private Rarity _currentRarity;

    public LootPaintSphere SpawnSphereForCard(Rarity rarity, Vector3 position, int cardIndex)
    {
        _currentRarity = rarity;
        SetSpawnPosition(position);
        return GetFromCardPool(cardIndex);
    }

    protected override void OnInitializeCardObject(LootPaintSphere obj, int cardIndex)
    {
        if (!_materialsRegistry) return;

        var paintItem = _materialsRegistry.GetRandomPaint(_currentRarity);
        if (paintItem == null) return;

        obj.SetMaterial(paintItem.PaintMaterial);
        obj.SetPaintId(paintItem.PaintId);
    }

    protected override Type GetObjectTypeToSpawn() => typeof(LootPaintSphere);
    protected override Vector3 GetSpawnPosition() => Vector3.zero;
}