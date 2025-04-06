public abstract class LootReward
{
    public Rarity Rarity { get; private set; }
    public LootRewardType Type { get; private set; }

    public LootReward(Rarity rarity, LootRewardType type)
    {
        Rarity = rarity;
        Type = type;
    }
}