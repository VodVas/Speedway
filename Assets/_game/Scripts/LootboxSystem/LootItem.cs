using UnityEngine;

[CreateAssetMenu(menuName = "Loot System/Loot Item")]
public class LootItem : ScriptableObject
{
    [SerializeField] private Rarity _rarity;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _cardSprite;
    [SerializeField] private GameObject _linkedSphere;

    public Rarity Rarity => _rarity;
    public string DisplayName => _displayName;
    public Sprite CardSprite => _cardSprite;
    public GameObject LinkedSphere => _linkedSphere;
}

public enum Rarity
{
    Common,
    Rare,
    Unique,
    Legendary,
    Epic
}