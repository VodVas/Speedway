#if UNITY_EDITOR
using UnityEngine;

public class LootBoxTester : MonoBehaviour
{
    [SerializeField] private int _testIterations = 1000;
    [SerializeField] private int _guaranteedEpicAfterAttempts = 5;
    [SerializeField] private int _cardsPerBox = 3;
    [SerializeField] private float _commonChance = 0.5f;
    [SerializeField] private float _rareChance = 0.2f;
    [SerializeField] private float _uniqueChance = 0.1f;
    [SerializeField] private float _legendaryChance = 0.1f;
    [SerializeField] private float _epicChance = 0.1f;

    [ContextMenu("Run Balance Test")]
    private void RunBalanceTest()
    {
        LootGeneratorCore generator = new LootGeneratorCore(
            _guaranteedEpicAfterAttempts, _cardsPerBox,
            _commonChance, _rareChance, _uniqueChance,
            _legendaryChance, _epicChance);

        int totalEpics = 0;
        int carsSpawned = 0;
        int epicMoney = 0;
        int epicColors = 0;
        int nonEpicMoney = 0;
        int nonEpicColors = 0;
        int forcedEpics = 0;
        int currentNonEpicStreak = 0;

        for (int i = 0; i < _testIterations; i++)
        {
            bool forceEpic = currentNonEpicStreak >= _guaranteedEpicAfterAttempts;
            LootGeneratorCore.LootResult[] results = generator.GenerateLootSet();
            bool hasEpic = false;

            for (int j = 0; j < results.Length; j++)
            {
                LootGeneratorCore.LootResult result = results[j];
                bool isEpic = result.Rarity == Rarity.Epic;

                if (isEpic)
                {
                    hasEpic = true;
                    totalEpics++;

                    switch (result.Type)
                    {
                        case LootRewardType.Car:
                            carsSpawned++;
                            break;
                        case LootRewardType.Money:
                            epicMoney++;
                            break;
                        case LootRewardType.Paint:
                            epicColors++;
                            break;
                    }
                }
                else
                {
                    switch (result.Type)
                    {
                        case LootRewardType.Money:
                            nonEpicMoney++;
                            break;
                        case LootRewardType.Paint:
                            nonEpicColors++;
                            break;
                    }
                }
            }

            if (forceEpic && hasEpic)
            {
                forcedEpics++;
            }

            currentNonEpicStreak = hasEpic ? 0 : currentNonEpicStreak + 1;
        }

        // Вывод результатов тестирования
        int totalCards = _testIterations * _cardsPerBox;
        string report = $"<color=#00FF00>TEST RESULTS ({_testIterations} iterations):</color>\n" +
            $"Total Epics: {totalEpics} ({totalEpics / (float)totalCards * 100:F1}%)\n" +
            $"Epic Cars: {carsSpawned} ({carsSpawned / (float)totalEpics * 100:F1}% of epics)\n" +
            $"Epic Money: {epicMoney} ({epicMoney / (float)totalEpics * 100:F1}%)\n" +
            $"Epic Colors: {epicColors} ({epicColors / (float)totalEpics * 100:F1}%)\n" +
            $"Non-Epic Money: {nonEpicMoney} ({nonEpicMoney / (float)totalCards * 100:F1}%)\n" +
            $"Non-Epic Colors: {nonEpicColors} ({nonEpicColors / (float)totalCards * 100:F1}%)\n" +
            $"Forced Epics: {forcedEpics} (after {_guaranteedEpicAfterAttempts} misses)";

        Debug.Log(report);
    }
}
#endif