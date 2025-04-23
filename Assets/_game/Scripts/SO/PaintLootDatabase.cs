using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Apocalypse/Loot System/Paint Loot Database")]
public class PaintLootDatabase : ScriptableObject
{
    [SerializeField] private List<PaintLootItemSO> _commonItems = new();
    [SerializeField] private List<PaintLootItemSO> _rareItems = new();
    [SerializeField] private List<PaintLootItemSO> _uniqueItems = new();
    [SerializeField] private List<PaintLootItemSO> _legendaryItems = new();

    [field: SerializeField] public List<PaintLootItemSO> EpicItems { get; private set; } = new();

    private Dictionary<Rarity, List<PaintLootItemSO>> _rarityMap;
    private Dictionary<int, Material> _materialsCache;

    public void Initialize()
    {
        _rarityMap = new Dictionary<Rarity, List<PaintLootItemSO>>
        {
            { Rarity.Common, _commonItems },
            { Rarity.Rare, _rareItems },
            { Rarity.Unique, _uniqueItems },
            { Rarity.Legendary, _legendaryItems },
            { Rarity.Epic, EpicItems }
        };

        InitializeMaterialsCache();
    }

    public PaintLootItemSO GetRandomItem(Rarity rarity)
    {
        if (_rarityMap == null) Initialize();

        if (_rarityMap.TryGetValue(rarity, out List<PaintLootItemSO> items) && items.Count > 0)
        {
            return items[Random.Range(0, items.Count)];
        }
        return null;
    }

    public bool TryGetMaterial(int paintId, out Material material)
    {
        if (_materialsCache == null) Initialize();

        return _materialsCache.TryGetValue(paintId, out material);
    }

    private void InitializeMaterialsCache()
    {
        _materialsCache = new Dictionary<int, Material>();

        AddItemsToCache(_commonItems);
        AddItemsToCache(_rareItems);
        AddItemsToCache(_uniqueItems);
        AddItemsToCache(_legendaryItems);
        AddItemsToCache(EpicItems);
    }

    private void AddItemsToCache(List<PaintLootItemSO> items)
    {
        foreach (var item in items)
        {
            if (item != null && item.PaintMaterial != null && !_materialsCache.ContainsKey(item.PaintId))
            {
                _materialsCache.Add(item.PaintId, item.PaintMaterial);
            }
        }
    }
}