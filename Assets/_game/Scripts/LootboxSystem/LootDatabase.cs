using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Loot System/Loot Database")]
public class LootDatabase : ScriptableObject
{
    [SerializeField] private List<LootItem> _commonItems = new();
    [SerializeField] private List<LootItem> _rareItems = new();
    [SerializeField] private List<LootItem> _uniqueItems = new();
    [SerializeField] private List<LootItem> _legendaryItems = new();
    [SerializeField] private List<LootItem> _epicItems = new();

    private Dictionary<Rarity, List<LootItem>> _rarityMap;

    public void Initialize()
    {
        _rarityMap = new Dictionary<Rarity, List<LootItem>>
        {
            { Rarity.Common, _commonItems },
            { Rarity.Rare, _rareItems },
            { Rarity.Unique, _uniqueItems },
            { Rarity.Legendary, _legendaryItems },
            { Rarity.Epic, _epicItems }
        };
    }

    public LootItem GetRandomItem(Rarity rarity)
    {
        if (_rarityMap.TryGetValue(rarity, out List<LootItem> items) && items.Count > 0)
        {
            return items[Random.Range(0, items.Count)];
        }
        return null;
    }

    public Rarity GetRandomRarity()
    {
        float value = Random.value;
        if (value < 0.5f) return Rarity.Common;
        if (value < 0.75f) return Rarity.Rare;
        if (value < 0.9f) return Rarity.Unique;
        return value < 0.98f ? Rarity.Legendary : Rarity.Epic;
    }
}