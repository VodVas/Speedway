using UnityEngine;

[CreateAssetMenu(menuName = "Loot System/Money Loot Item")]
public class MoneyLootItem : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public string Count { get; private set; }
    [field: SerializeField] public Sprite CardSprite { get; private set; }
}