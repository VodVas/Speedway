using System.Collections.Generic;

public class RarityLocalization : IRarityLocalization
{
    private const string RarityCommon = "RarityCommon";
    private const string RarityRare = "RarityRare";
    private const string RarityUnique = "RarityUnique";
    private const string RarityLegendary = "RarityLegendary";
    private const string RarityEpic = "RarityEpic";

    private readonly SystemLocalization _localization;
    private readonly Dictionary<Rarity, string> _rarityKeys;

    public RarityLocalization(SystemLocalization localization)
    {
        _localization = localization;
        _rarityKeys = new Dictionary<Rarity, string>
        {
            { Rarity.Common, RarityCommon },
            { Rarity.Rare, RarityRare },
            { Rarity.Unique, RarityUnique },
            { Rarity.Legendary, RarityLegendary },
            { Rarity.Epic, RarityEpic }
        };
    }

    public string GetLocalizedRarityText(Rarity rarity)
    {
        if (_rarityKeys.TryGetValue(rarity, out string key))
        {
            return _localization.GetPhrase(key);
        }

        return rarity.ToString();
    }
}