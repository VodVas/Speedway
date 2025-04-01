using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Loot System/LootDatabase")]
public class PaintLootDatabase : ScriptableObject
{
    [SerializeField] private List<PaintLootItem> _commonItems = new();
    [SerializeField] private List<PaintLootItem> _rareItems = new();
    [SerializeField] private List<PaintLootItem> _uniqueItems = new();
    [SerializeField] private List<PaintLootItem> _legendaryItems = new();

    [field: SerializeField] public List<PaintLootItem> EpicItems { get; private set; } = new();

    private Dictionary<Rarity, List<PaintLootItem>> _rarityMap;

    public void Initialize()
    {
        _rarityMap = new Dictionary<Rarity, List<PaintLootItem>>
        {
            { Rarity.Common, _commonItems },
            { Rarity.Rare, _rareItems },
            { Rarity.Unique, _uniqueItems },
            { Rarity.Legendary, _legendaryItems },
            { Rarity.Epic, EpicItems }
        };
    }

    public PaintLootItem GetRandomItem(Rarity rarity)
    {
        if (_rarityMap.TryGetValue(rarity, out List<PaintLootItem> items) && items.Count > 0)
        {
            return items[Random.Range(0, items.Count)];
        }
        return null;
    }
}