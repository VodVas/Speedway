using Reflex.Attributes;
using System;
using UnityEngine;

public class CardLootSpheresSpawner : MultiPoolLootboxSpawner<LootPaintSphere>
{
    [Inject] private PaintLootDatabase _paintDatabase;

    private Rarity _currentRarity;
    private Vector3 _cachedSpawnPosition;
    private bool _isInitialized;

    private void Awake()
    {
        _isInitialized = _paintDatabase != null;

        if (!_isInitialized)
        {
            Debug.LogError("[CardLootSpheresSpawner] PaintLootDatabase is not injected!", this);
            enabled = false;
            return;
        }
    }

    public LootPaintSphere SpawnSphereForCard(Rarity rarity, Vector3 position, int cardIndex)
    {
        if (!_isInitialized) return null;

        _currentRarity = rarity;
        _cachedSpawnPosition = position;
        SetSpawnPosition(position);

        return GetFromCardPool(cardIndex);
    }

    protected override void OnInitializeCardObject(LootPaintSphere obj, int cardIndex)
    {
        if (!_isInitialized || obj == null) return;

        var paintItem = _paintDatabase.GetRandomItem(_currentRarity);

        if (paintItem != null && paintItem.PaintMaterial != null)
        {
            obj.SetMaterial(paintItem.PaintMaterial);
            obj.SetPaintId(paintItem.PaintId);
        }
        else
        {
            Debug.LogWarning($"[CardLootSpheresSpawner] Failed to get paint item for rarity {_currentRarity}", this);
            enabled |= false;
            return;
        }
    }

    private static readonly Type _sphereType = typeof(LootPaintSphere);

    protected override Type GetObjectTypeToSpawn() => _sphereType;

    protected override Vector3 GetSpawnPosition() => _cachedSpawnPosition;
}






//public class CardLootSpheresSpawner : MultiPoolLootboxSpawner<LootPaintSphere>
//{
//    [Inject] private PaintLootDatabase _paintDatabase;
//    private Rarity _currentRarity;
//    private PaintLootItemSO _currentItem;

//    public LootPaintSphere SpawnSphereForCard(Rarity rarity, Vector3 position, int cardIndex)
//    {
//        _currentRarity = rarity;
//        SetSpawnPosition(position);
//        return GetFromCardPool(cardIndex);
//    }

//    protected override void OnInitializeCardObject(LootPaintSphere obj, int cardIndex)
//    {
//        if (_paintDatabase == null)
//        {
//            Debug.LogError("[CardLootSpheresSpawner] Paint database not injected!");
//            return;
//        }

//        // Получаем случайный элемент из базы данных
//        var paintItem = _paintDatabase.GetRandomItem(_currentRarity);
//        if (paintItem == null) return;

//        // Устанавливаем материал и ID напрямую из PaintLootItemSO
//        obj.SetMaterial(paintItem.PaintMaterial);
//        obj.SetPaintId(paintItem.PaintId);

//        Debug.Log($"[CardLootSpheresSpawner] Set paint ID {paintItem.PaintId} for sphere on card {cardIndex}");
//    }

//    protected override Type GetObjectTypeToSpawn() => typeof(LootPaintSphere);
//    protected override Vector3 GetSpawnPosition() => Vector3.zero;
//}

//public class CardLootSpheresSpawner : MultiPoolLootboxSpawner<LootPaintSphere>
//{
//    [SerializeField] private SpheresMaterialsRegistry _materialsRegistry;
//    private Rarity _currentRarity;

//    public LootPaintSphere SpawnSphereForCard(Rarity rarity, Vector3 position, int cardIndex)
//    {
//        _currentRarity = rarity;
//        SetSpawnPosition(position);
//        return GetFromCardPool(cardIndex);
//    }

//    protected override void OnInitializeCardObject(LootPaintSphere obj, int cardIndex)
//    {
//        if (!_materialsRegistry) return;

//        var paintItem = _materialsRegistry.GetRandomPaint(_currentRarity);
//        if (paintItem == null) return;

//        obj.SetMaterial(paintItem.PaintMaterial);
//        obj.SetPaintId(paintItem.PaintId);
//    }

//    protected override Type GetObjectTypeToSpawn() => typeof(LootPaintSphere);
//    protected override Vector3 GetSpawnPosition() => Vector3.zero;
//}