using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Loot System/Paint Loot Database")]
public class PaintLootDatabase : ScriptableObject
{
    [SerializeField] private List<PaintLootItemSO> _commonItems = new();
    [SerializeField] private List<PaintLootItemSO> _rareItems = new();
    [SerializeField] private List<PaintLootItemSO> _uniqueItems = new();
    [SerializeField] private List<PaintLootItemSO> _legendaryItems = new();

    [field: SerializeField] public List<PaintLootItemSO> EpicItems { get; private set; } = new();

    private Dictionary<Rarity, List<PaintLootItemSO>> _rarityMap;

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
    }

    public PaintLootItemSO GetRandomItem(Rarity rarity)
    {
        if (_rarityMap.TryGetValue(rarity, out List<PaintLootItemSO> items) && items.Count > 0)
        {
            return items[Random.Range(0, items.Count)];
        }
        return null;
    }
}