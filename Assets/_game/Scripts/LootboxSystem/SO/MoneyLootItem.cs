using UnityEngine;

[CreateAssetMenu(menuName = "Apocalypse/Loot System/Money Loot Item")]
public class MoneyLootItem : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public string Amount { get; private set; }
    [field: SerializeField] public Sprite CardSprite { get; private set; }
    [field: SerializeField] public Sprite CurrencyIcon { get; private set; }
    [field: SerializeField] public Rarity Rarity { get; private set; }
}