using UnityEngine;

[CreateAssetMenu(menuName = "Apocalypse/Loot System/Paint Loot Item")]
public class PaintLootItemSO : ScriptableObject
{
    [field: SerializeField] public int PaintId { get; private set; }
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite CardSprite { get; private set; }
    [field: SerializeField] public Rarity Rarity { get; private set; }
    [field: SerializeField] public Material PaintMaterial { get; private set; }
}