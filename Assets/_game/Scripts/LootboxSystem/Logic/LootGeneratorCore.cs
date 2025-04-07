using UnityEngine;

public class LootGeneratorCore
{
    private const int DEFAULT_CARD_COUNT = 3;
    private const float EPIC_CAR_CHANCE = 0.333f;
    private const float EPIC_MONEY_CHANCE = 0.666f;
    private const float NON_EPIC_MONEY_CHANCE = 0.5f;

    private readonly int _guaranteedEpicAfterAttempts;
    private readonly float _commonChance;
    private readonly float _rareChance;
    private readonly float _uniqueChance;
    private readonly float _legendaryChance;
    private readonly float _epicChance;
    private readonly int _cardsCount;

    private int _consecutiveNonEpicCount;

    public struct LootResult
    {
        public Rarity Rarity;
        public LootRewardType Type;

        public LootResult(Rarity rarity, LootRewardType type)
        {
            Rarity = rarity;
            Type = type;
        }
    }

    public LootGeneratorCore(int guaranteedEpicAfterAttempts = 5, int cardsCount = DEFAULT_CARD_COUNT,
        float commonChance = 0.5f, float rareChance = 0.2f, float uniqueChance = 0.1f,
        float legendaryChance = 0.1f, float epicChance = 0.1f)
    {
        _guaranteedEpicAfterAttempts = guaranteedEpicAfterAttempts;
        _cardsCount = cardsCount;

        float totalChance = commonChance + rareChance + uniqueChance + legendaryChance + epicChance;
        float normalizeFactor = Mathf.Approximately(totalChance, 1f) ? 1f : 1f / totalChance;

        _commonChance = commonChance * normalizeFactor;
        _rareChance = rareChance * normalizeFactor;
        _uniqueChance = uniqueChance * normalizeFactor;
        _legendaryChance = legendaryChance * normalizeFactor;
        _epicChance = epicChance * normalizeFactor;
    }

    public LootResult[] GenerateLootSet()
    {
        Rarity[] rarities = GenerateRarities();
        LootResult[] results = new LootResult[_cardsCount];

        for (int i = 0; i < _cardsCount; i++)
        {
            bool isEpic = rarities[i] == Rarity.Epic;
            LootRewardType type = DetermineRewardType(isEpic);
            results[i] = new LootResult(rarities[i], type);
        }

        return results;
    }

    private Rarity[] GenerateRarities()
    {
        Rarity[] rarities = new Rarity[_cardsCount];
        bool hasEpic = false;

        for (int i = 0; i < _cardsCount; i++)
        {
            rarities[i] = DetermineRarity();
            if (rarities[i] == Rarity.Epic) hasEpic = true;
        }

        if (!hasEpic)
        {
            _consecutiveNonEpicCount++;
            if (_consecutiveNonEpicCount >= _guaranteedEpicAfterAttempts)
            {
                int forcedIndex = Random.Range(0, _cardsCount);
                rarities[forcedIndex] = Rarity.Epic;
                _consecutiveNonEpicCount = 0;
            }
        }
        else
        {
            _consecutiveNonEpicCount = 0;
        }

        return rarities;
    }

    public Rarity DetermineRarity()
    {
        float roll = Random.value;

        if (roll < _commonChance) return Rarity.Common;
        if (roll < _commonChance + _rareChance) return Rarity.Rare;
        if (roll < _commonChance + _rareChance + _uniqueChance) return Rarity.Unique;
        if (roll < 1f - _epicChance) return Rarity.Legendary;

        return Rarity.Epic;
    }

    private LootRewardType DetermineRewardType(bool isEpic)
    {
        float roll = Random.value;

        if (isEpic)
        {
            if (roll < EPIC_CAR_CHANCE) return LootRewardType.Car;
            if (roll < EPIC_MONEY_CHANCE) return LootRewardType.Money;
            return LootRewardType.Paint;
        }
        else
        {
            return roll < NON_EPIC_MONEY_CHANCE ? LootRewardType.Money : LootRewardType.Paint;
        }
    }
}