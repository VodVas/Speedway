using UnityEngine;

[CreateAssetMenu(menuName = "Loot System/Paint Loot Item")]
public class PaintLootItemSO : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite CardSprite { get; private set; }
    [field: SerializeField] public Rarity Rarity { get; private set; }
}

public enum Rarity
{
    Common,
    Rare,
    Unique,
    Legendary,
    Epic
}