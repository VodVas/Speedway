using System;
using UnityEngine;

//public class LootSpheresSpawner : Spawner<LootPaintSphere>
//{
//    [SerializeField] private SpheresMaterialsRegistry _materialsRegistry;

//    private Rarity _currentRarity;
//    private Vector3 _cachedPosition;

//    public LootPaintSphere SpawnLootSphere(Rarity rarity, Vector3 spawnPos)
//    {
//        _currentRarity = rarity;
//        _cachedPosition = spawnPos;

//        LootPaintSphere sphere = SpawnObject();
//        return sphere;
//    }

//    protected override Vector3 GetSpawnPosition()
//    {
//        return _cachedPosition;
//    }

//    protected override Type GetObjectTypeToSpawn()
//    {
//        return typeof(LootPaintSphere);
//    }

//    protected override void OnGetFromPool(LootPaintSphere obj)
//    {
//        base.OnGetFromPool(obj);

//        if (_materialsRegistry != null)
//        {
//            PaintLootItem paintItem = _materialsRegistry.GetRandomPaint(_currentRarity);

//            if (paintItem != null)
//            {
//                obj.SetMaterial(paintItem.PaintMaterial);
//                obj.SetPaintId(paintItem.PaintId);
//            }
//        }
//        else
//        {
//            Debug.LogWarning("[LootSpheresSpawner] _materialsRegistry is null!", this);
//        }
//    }
//}