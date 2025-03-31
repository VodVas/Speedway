using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Loot System/LootDatabase")]
public class LootDatabase : ScriptableObject
{
    [SerializeField] private List<PaintLootItem> _commonItems = new();
    [SerializeField] private List<PaintLootItem> _rareItems = new();
    [SerializeField] private List<PaintLootItem> _uniqueItems = new();
    [SerializeField] private List<PaintLootItem> _legendaryItems = new();
    [SerializeField] private List<PaintLootItem> _epicItems = new();

    private Dictionary<Rarity, List<PaintLootItem>> _rarityMap;

    public void Initialize()
    {
        _rarityMap = new Dictionary<Rarity, List<PaintLootItem>>
        {
            { Rarity.Common, _commonItems },
            { Rarity.Rare, _rareItems },
            { Rarity.Unique, _uniqueItems },
            { Rarity.Legendary, _legendaryItems },
            { Rarity.Epic, _epicItems }
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

    public Rarity GetRandomRarity()
    {
        float value = Random.value;
        if (value < 0.5f) return Rarity.Common;
        if (value < 0.7f) return Rarity.Rare;
        if (value < 0.8f) return Rarity.Unique;
        return value < 0.9f ? Rarity.Legendary : Rarity.Epic;
    }
}