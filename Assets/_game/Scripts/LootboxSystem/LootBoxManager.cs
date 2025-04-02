using UnityEngine;
using UnityEngine.UI;
using YG;

public class LootBoxManager : MonoBehaviour
{
    private const int CardsCount = 3;
    private const float EPIC_CAR_CHANCE = 0.333f;
    private const float EPIC_MONEY_CHANCE = 0.666f;
    private const float NON_EPIC_MONEY_CHANCE = 0.5f;

    [Header("Settings")]
    [SerializeField] private int _guaranteedEpicAfterAttempts = 5;

    [Header("Databases")]
    [SerializeField] private PaintLootDatabase _PaintLootDatabase;
    [SerializeField] private MoneyLootDatabase _moneyLootDatabase;

    [Header("UI Components")]
    [SerializeField] private MoneyLootCardController[] _moneyLootCards;
    [SerializeField] private PaintLootCardController[] _colorLootCards;
    [SerializeField] private CarLootCardController[] _carLootCards;
    [SerializeField] private Button[] _boxButtons;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] _sphereSpawnPoints;
    [SerializeField] private Transform[] _carSpawnPoints;

    [Header("Spawners")]
    [SerializeField] private LootSpheresSpawner _lootSpheresSpawner;
    [SerializeField] private LootCarSpawner _lootCarSpawner;

    [Header("Rarity Chances")]
    [SerializeField] private float _commonChance = 0.5f;
    [SerializeField] private float _rareChance = 0.2f;
    [SerializeField] private float _uniqueChance = 0.1f;
    [SerializeField] private float _legendaryChance = 0.1f;
    [SerializeField] private float _epicChance = 0.1f;

    private int _consecutiveNonEpicCount = 0;
    private int _selectedButtonIndex = -1;

    private void Awake()
    {
        InitializeSystem();
    }

    private void OnDisable()
    {
        UnsubscribeFromButtons();
    }

    private void OnValidate()
    {
        float total = _commonChance + _rareChance + _uniqueChance +
                      _legendaryChance + _epicChance;

        if (Mathf.Abs(total - 1f) > 0.001f)
        {
            Debug.LogWarning("Сумма шансов редкостей должна быть равна 100%");
        }
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

    private void InitializeSystem()
    {
        _PaintLootDatabase.Initialize();
        _moneyLootDatabase.Initialize();
        ValidateDependencies();
        SubscribeToButtons();
    }

    private void ValidateDependencies()
    {
        bool error = false;

        if (_lootSpheresSpawner == null || _lootCarSpawner == null)
        {
            Debug.LogError("[LootBoxManager] Spawners are not assigned!");
            error = true;
        }

        if (_moneyLootCards.Length < CardsCount ||
            _colorLootCards.Length < CardsCount ||
            _carLootCards.Length < CardsCount)
        {
            Debug.LogError("[LootBoxManager] Card arrays length mismatch!");
            error = true;
        }

        for (int i = 0; i < CardsCount; i++)
        {
            if (_sphereSpawnPoints[i] == null || _carSpawnPoints[i] == null)
            {
                Debug.LogError($"[LootBoxManager] Spawn point {i} is null!");
                error = true;
            }
        }

        if (error) enabled = false;
    }

    private void SubscribeToButtons()
    {
        for (int i = 0; i < _boxButtons.Length; i++)
        {
            int index = i;
            _boxButtons[i].onClick.AddListener(() => OnBoxSelected(index));
        }
    }

    private void OnBoxSelected(int buttonIndex)
    {
        _selectedButtonIndex = buttonIndex;
        DisableAllButtons();
        Rarity[] rarities = GenerateRarities();
        ProcessCards(rarities);
    }

    private Rarity[] GenerateRarities()
    {
        Rarity[] rarities = new Rarity[CardsCount];
        bool forceEpic = ShouldForceEpic();

        for (int i = 0; i < CardsCount; i++)
        {
            rarities[i] = DetermineRarity();
        }

        if (forceEpic) ForceEpicCard(ref rarities);
        return rarities;
    }

    private bool ShouldForceEpic()
    {
        return _consecutiveNonEpicCount >= _guaranteedEpicAfterAttempts;
    }

    private void ForceEpicCard(ref Rarity[] rarities)
    {
        bool hasEpic = false;
        for (int i = 0; i < CardsCount; i++)
        {
            if (rarities[i] == Rarity.Epic)
            {
                hasEpic = true;
                break;
            }
        }

        if (!hasEpic)
        {
            int forcedIndex = Random.Range(0, CardsCount);
            rarities[forcedIndex] = Rarity.Epic;
        }
    }

    private void ProcessCards(Rarity[] rarities)
    {
        bool hasEpicInBox = false;

        for (int i = 0; i < CardsCount; i++)
        {
            Rarity rarity = rarities[i];
            bool isEpic = rarity == Rarity.Epic;
            bool isSelected = (i == _selectedButtonIndex);

            if (isEpic)
            {
                ProcessEpicCard(i, isSelected);
                hasEpicInBox = true;
            }
            else
            {
                ProcessNonEpicCard(rarity, i, isSelected);
            }
        }

        _consecutiveNonEpicCount = hasEpicInBox ? 0 : _consecutiveNonEpicCount + 1;
    }

    private void ProcessEpicCard(int index, bool isSelected)
    {
        float roll = Random.value;

        if (roll < EPIC_CAR_CHANCE)
        {
            SpawnCar(index, isSelected);
        }
        else if (roll < EPIC_MONEY_CHANCE)
        {
            SpawnMoney(Rarity.Epic, index, isSelected);
        }
        else
        {
            SpawnColor(Rarity.Epic, index, isSelected);
        }
    }

    private void ProcessNonEpicCard(Rarity rarity, int index, bool isSelected)
    {
        bool spawnMoney = Random.value < NON_EPIC_MONEY_CHANCE;

        if (spawnMoney)
        {
            SpawnMoney(rarity, index, isSelected);
        }
        else
        {
            SpawnColor(rarity, index, isSelected);
        }
    }

    private void SpawnCar(int index, bool isSelected)
    {
        Transform spawnPoint = _carSpawnPoints[index];
        LootCar car = _lootCarSpawner.SpawnLootCar(spawnPoint.position);

        _carLootCards[index].gameObject.SetActive(true);
        _carLootCards[index].ShowCard(car.gameObject);
        _colorLootCards[index].gameObject.SetActive(false);
        _moneyLootCards[index].gameObject.SetActive(false);

        // Если нужно обрабатывать выпадение машины при выборе
        if (isSelected)
        {
            // Логика разблокировки машины
        }
    }

    private void SpawnColor(Rarity rarity, int index, bool isSelected)
    {
        PaintLootItemOld item = _PaintLootDatabase.GetRandomItem(rarity);
        Transform spawnPoint = _sphereSpawnPoints[index];
        LootPaintSphere sphere = _lootSpheresSpawner.SpawnLootSphere(rarity, spawnPoint.position);

        _colorLootCards[index].gameObject.SetActive(true);
        _colorLootCards[index].ShowCard(item, sphere.gameObject);
        _carLootCards[index].gameObject.SetActive(false);
        _moneyLootCards[index].gameObject.SetActive(false);

        // Если нужно обрабатывать выпадение цвета при выборе
        if (isSelected)
        {
            int paintId = sphere.GetComponent<LootPaintSphere>().GetPaintId();
            YandexGame.savesData.UnlockPaint(paintId);
            YandexGame.SaveProgress();

            // Явное обновление материалов
            FindObjectOfType<PaintIntegrationSystem>()?.ForceRefresh();
        }
    }

    private void SpawnMoney(Rarity rarity, int index, bool isSelected)
    {
        MoneyLootItem item = _moneyLootDatabase.GetRandomItem(rarity);

        if (item == null)
        {
            Debug.LogError($"Missing money item for rarity: {rarity}");
            return;
        }

        _moneyLootCards[index].gameObject.SetActive(true);
        _moneyLootCards[index].ShowCard(item);
        _colorLootCards[index].gameObject.SetActive(false);
        _carLootCards[index].gameObject.SetActive(false);

        if (isSelected)
        {
            if (int.TryParse(item.Count, out int amount))
            {
                YandexGame.savesData.AddMoney(amount);
            }
            else
            {
                Debug.LogError($"Failed to parse money amount from '{item.Count}'");
            }
            YandexGame.SaveProgress();
        }
    }

    private void DisableAllButtons()
    {
        for (int i = 0; i < _boxButtons.Length; i++)
        {
            _boxButtons[i].interactable = false;
        }
    }

    private void UnsubscribeFromButtons()
    {
        for (int i = 0; i < _boxButtons.Length; i++)
        {
            _boxButtons[i].onClick.RemoveAllListeners();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Run Balance Test (1000 iterations)")]
    private void RunMassTest()
    {
        const int iterations = 1000;
        int totalEpics = 0;
        int carsSpawned = 0;
        int epicMoney = 0;
        int epicColors = 0;
        int nonEpicMoney = 0;
        int nonEpicColors = 0;
        int forcedEpics = 0;
        int currentNonEpicStreak = 0;

        var lootDb = _PaintLootDatabase;
        var moneyDb = _moneyLootDatabase;

        for (int i = 0; i < iterations; i++)
        {
            bool forceEpic = currentNonEpicStreak >= _guaranteedEpicAfterAttempts;
            Rarity[] rarities = new Rarity[CardsCount];
            bool hasEpic = false;

            for (int j = 0; j < CardsCount; j++)
            {
                rarities[j] = DetermineRarity();

                if (rarities[j] == Rarity.Epic) hasEpic = true;
            }

            if (forceEpic && !hasEpic)
            {
                int forcedIndex = Random.Range(0, CardsCount);
                rarities[forcedIndex] = Rarity.Epic;
                hasEpic = true;
                forcedEpics++;
            }

            for (int j = 0; j < CardsCount; j++)
            {
                Rarity rarity = rarities[j];
                bool isEpic = rarity == Rarity.Epic;

                if (isEpic)
                {
                    totalEpics++;
                    float roll = Random.value;

                    if (roll < EPIC_CAR_CHANCE) carsSpawned++;
                    else if (roll < EPIC_MONEY_CHANCE) epicMoney++;
                    else epicColors++;
                }
                else
                {
                    if (Random.value < NON_EPIC_MONEY_CHANCE) nonEpicMoney++;
                    else nonEpicColors++;
                }
            }

            currentNonEpicStreak = hasEpic ? 0 : currentNonEpicStreak + 1;
        }

        string report = $"<color=#00FF00>TEST RESULTS ({iterations} iterations):</color>\n" +
            $"Total Epics: {totalEpics} ({totalEpics / (float)(iterations * CardsCount) * 100:F1}%)\n" +
            $"Epic Cars: {carsSpawned} ({carsSpawned / (float)totalEpics * 100:F1}% of epics)\n" +
            $"Epic Money: {epicMoney} ({epicMoney / (float)totalEpics * 100:F1}%)\n" +
            $"Epic Colors: {epicColors} ({epicColors / (float)totalEpics * 100:F1}%)\n" +
            $"Non-Epic Money: {nonEpicMoney} ({nonEpicMoney / (float)(iterations * CardsCount) * 100:F1}%)\n" +
            $"Non-Epic Colors: {nonEpicColors} ({nonEpicColors / (float)(iterations * CardsCount) * 100:F1}%)\n" +
            $"Forced Epics: {forcedEpics} (after {_guaranteedEpicAfterAttempts} misses)";

        Debug.Log(report);

        _consecutiveNonEpicCount = 0;
    }
#endif
}