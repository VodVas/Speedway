using UnityEngine;
using System.Collections.Generic;

public class SpheresMaterialsRegistry : MonoBehaviour
{
    [Header("Paint Loot")]
    [SerializeField] private PaintLootItem[] _commonPaints;
    [SerializeField] private PaintLootItem[] _rarePaints;
    [SerializeField] private PaintLootItem[] _uniquePaints;
    [SerializeField] private PaintLootItem[] _legendaryPaints;
    [SerializeField] private PaintLootItem[] _epicPaints;

    private Dictionary<int, Material> _materialCache;
    private PaintLootItem[][] _paintsByRarity;

    private void Awake()
    {
        InitializeCache();
    }

    public Material GetRandomMaterial(Rarity rarity)
    {
        PaintLootItem item = GetRandomPaint(rarity);
        return item?.PaintMaterial;
    }

    private void InitializeCache()
    {
        _materialCache = new Dictionary<int, Material>(256);
        _paintsByRarity = new PaintLootItem[5][]
        {
            _commonPaints,
            _rarePaints,
            _uniquePaints,
            _legendaryPaints,
            _epicPaints
        };

        int totalMaterials = 0;
        for (int i = 0; i < _paintsByRarity.Length; i++)
        {
            PaintLootItem[] paints = _paintsByRarity[i];
            if (paints == null)
            {
                Debug.LogWarning($"[SpheresMaterialsRegistry] Paint array for rarity {(Rarity)i} is null!");
                continue;
            }

            for (int j = 0; j < paints.Length; j++)
            {
                PaintLootItem item = paints[j];
                if (item == null)
                {
                    Debug.LogWarning($"[SpheresMaterialsRegistry] Paint item at index {j} for rarity {(Rarity)i} is null!");
                    continue;
                }

                if (item.PaintMaterial == null)
                {
                    Debug.LogWarning($"[SpheresMaterialsRegistry] Paint material for item {item.PaintId} is null!");
                    continue;
                }

                if (!_materialCache.ContainsKey(item.PaintId))
                {
                    _materialCache.Add(item.PaintId, item.PaintMaterial);
                    totalMaterials++;
                }
            }
        }

        Debug.Log($"[SpheresMaterialsRegistry] Initialized cache with {totalMaterials} materials");
    }

    public PaintLootItem GetRandomPaint(Rarity rarity)
    {
        PaintLootItem[] paints = _paintsByRarity[(int)rarity];
        if (paints == null || paints.Length == 0) return null;

        return paints[Random.Range(0, paints.Length)];
    }

    public bool TryGetMaterial(int paintId, out Material material)
    {
        bool result = _materialCache.TryGetValue(paintId, out material);
        if (!result)
        {
            Debug.LogWarning($"[SpheresMaterialsRegistry] Failed to get material for paint ID: {paintId}");
        }
        return result;
    }
}

//public class SpheresMaterialsRegistry : MonoBehaviour
//{
//    [Header("Materials by Rarity")]
//    [SerializeField] private Material[] _commonMaterials;
//    [SerializeField] private Material[] _rareMaterials;
//    [SerializeField] private Material[] _uniqueMaterials;
//    [SerializeField] private Material[] _legendaryMaterials;
//    [SerializeField] private Material[] _epicMaterials;

//    public Material GetRandomMaterial(Rarity rarity)
//    {
//        Material[] arrayToUse = null;

//        switch (rarity)
//        {
//            case Rarity.Common:
//                arrayToUse = _commonMaterials;
//                break;
//            case Rarity.Rare:
//                arrayToUse = _rareMaterials;
//                break;
//            case Rarity.Unique:
//                arrayToUse = _uniqueMaterials;
//                break;
//            case Rarity.Legendary:
//                arrayToUse = _legendaryMaterials;
//                break;
//            case Rarity.Epic:
//                arrayToUse = _epicMaterials;
//                break;
//        }

//        if (arrayToUse == null || arrayToUse.Length == 0)
//        {
//            Debug.LogWarning($"[SpheresMaterialsRegistry] No materials array for rarity {rarity}");
//            return null;
//        }

//        int randomIndex = Random.Range(0, arrayToUse.Length);
//        return arrayToUse[randomIndex];
//    }
//}