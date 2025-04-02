using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Loot System/LootDatabase")]
public class PaintLootDatabase : ScriptableObject
{
    [SerializeField] private List<PaintLootItemOld> _commonItems = new();
    [SerializeField] private List<PaintLootItemOld> _rareItems = new();
    [SerializeField] private List<PaintLootItemOld> _uniqueItems = new();
    [SerializeField] private List<PaintLootItemOld> _legendaryItems = new();

    [field: SerializeField] public List<PaintLootItemOld> EpicItems { get; private set; } = new();

    private Dictionary<Rarity, List<PaintLootItemOld>> _rarityMap;

    public void Initialize()
    {
        _rarityMap = new Dictionary<Rarity, List<PaintLootItemOld>>
        {
            { Rarity.Common, _commonItems },
            { Rarity.Rare, _rareItems },
            { Rarity.Unique, _uniqueItems },
            { Rarity.Legendary, _legendaryItems },
            { Rarity.Epic, EpicItems }
        };
    }

    public PaintLootItemOld GetRandomItem(Rarity rarity)
    {
        if (_rarityMap.TryGetValue(rarity, out List<PaintLootItemOld> items) && items.Count > 0)
        {
            return items[Random.Range(0, items.Count)];
        }
        return null;
    }
}