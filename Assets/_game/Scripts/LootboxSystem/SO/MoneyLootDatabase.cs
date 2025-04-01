using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot System/Money Loot Database")]
public class MoneyLootDatabase : ScriptableObject
{
    [SerializeField] private MoneyLootItem _commonItem;
    [SerializeField] private MoneyLootItem _rareItem ;
    [SerializeField] private MoneyLootItem _uniqueItem;
    [SerializeField] private MoneyLootItem _legendaryItem;
    [field: SerializeField] public MoneyLootItem EpicItem { get; private set; }

    private Dictionary<Rarity, MoneyLootItem> _rarityMap;

    public void Initialize()
    {
        _rarityMap = new Dictionary<Rarity, MoneyLootItem>
        {
            { Rarity.Common, _commonItem },
            { Rarity.Rare, _rareItem },
            { Rarity.Unique, _uniqueItem },
            { Rarity.Legendary, _legendaryItem },
            { Rarity.Epic, EpicItem }
        };
    }

    public MoneyLootItem GetRandomItem(Rarity rarity)
    {
        if (_rarityMap.TryGetValue(rarity, out MoneyLootItem item))
        {
            return item;
        }

        return null;
    }
}