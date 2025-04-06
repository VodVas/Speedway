using UnityEngine;
using System.Collections;
using YG;
using UnityEngine.UI;

public class LootBoxManager : MonoBehaviour
{
    private int CardsCount = 3;
    private float EpicCardChance = 0.333f;
    private float EpicMoneyChance = 0.666f;
    private float NonEpicCardChance = 0.5f;

    [Header("Settings")]
    [SerializeField] private int _guaranteedEpicAfterAttempts = 5;

    [Header("Databases")]
    [SerializeField] private PaintLootDatabase _PaintLootDatabase;
    [SerializeField] private MoneyLootDatabase _moneyLootDatabase;
    [SerializeField] private CarLootDatabase _carLootDatabase;

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

    [Header("Box Components")]
    [SerializeField] private BoxOpener[] _boxOpeners;
    [SerializeField] private ObjectOnceShaker[] _boxShakers;

    [Header("Audio")]
    [SerializeField] private OnceSoundPlayer _cardAppearSound;

    private int _consecutiveNonEpicCount;
    private int _selectedButtonIndex = -1;

    private void OnValidate()
    {
        float total = _commonChance + _rareChance + _uniqueChance + _legendaryChance + _epicChance;
        if (Mathf.Abs(total - 1f) > 0.001f)
        {
            Debug.LogWarning("Not 100%!");
        }
    }

    private void Awake()
    {
        InitializeSystem();
    }

    private void InitializeSystem()
    {
        _PaintLootDatabase.Initialize();
        _moneyLootDatabase.Initialize();
        _carLootDatabase.Initialize();
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
            Debug.LogError("[LootBoxManager] Card arrays mismatch!");
            error = true;
        }

        if (_carLootDatabase == null)
        {
            Debug.LogError("[LootBoxManager] Car Loot Database is not assigned!");
            error = true;
        }

        if (_boxOpeners == null || _boxOpeners.Length == 0)
        {
            Debug.LogError("[LootBoxManager] Box Openers are not assigned!");
            error = true;
        }
        else if (_boxOpeners.Length != _boxButtons.Length)
        {
            Debug.LogError("[LootBoxManager] Box Openers count mismatch with Buttons!");
            error = true;
        }

        if (_boxShakers == null || _boxShakers.Length == 0)
        {
            Debug.LogError("[LootBoxManager] Box Shakers are not assigned!");
            error = true;
        }
        else if (_boxShakers.Length != _boxButtons.Length)
        {
            Debug.LogError("[LootBoxManager] Box Shakers count mismatch with Buttons!");
            error = true;
        }

        if (_cardAppearSound == null)
        {
            Debug.LogError("[LootBoxManager] Card Appear Sound not assigned!");
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
        if (_selectedButtonIndex != -1 || buttonIndex < 0 || buttonIndex >= _boxButtons.Length)
            return;

        _selectedButtonIndex = buttonIndex;
        DisableAllButtons();

        if (buttonIndex < _boxShakers.Length)
        {
            _boxShakers[buttonIndex].Shake();
        }
        StartCoroutine(OpenLidsAfterShake(buttonIndex));
    }

    private IEnumerator OpenLidsAfterShake(int buttonIndex)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < _boxOpeners.Length; i++)
        {
            _boxOpeners[i].OpenBox();
        }

        yield return new WaitForSeconds(1f);

        Rarity[] rarities = GenerateRarities();
        ProcessCards(rarities, buttonIndex);
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

    private Rarity[] GenerateRarities()
    {
        Rarity[] rarities = new Rarity[CardsCount];
        bool hasEpic = false;

        for (int i = 0; i < CardsCount; i++)
        {
            rarities[i] = DetermineRarity();
            if (rarities[i] == Rarity.Epic) hasEpic = true;
        }

        if (!hasEpic)
        {
            _consecutiveNonEpicCount++;
            if (_consecutiveNonEpicCount >= _guaranteedEpicAfterAttempts)
            {
                int forcedIndex = Random.Range(0, CardsCount);
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

    private void ProcessCards(Rarity[] rarities, int selectedIndex)
    {
        // Спавним выбранную карту
        ProcessSingleCard(selectedIndex, rarities[selectedIndex]);
        // Спавним остальные
        StartCoroutine(ProcessRemainingCards(rarities, selectedIndex));
    }

    private IEnumerator ProcessRemainingCards(Rarity[] rarities, int selectedIndex)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < CardsCount; i++)
        {
            if (i != selectedIndex)
            {
                ProcessSingleCard(i, rarities[i]);
            }
        }
    }

    private void ProcessSingleCard(int index, Rarity rarity)
    {
        if (index < 0 || index >= CardsCount)
        {
            Debug.LogError($"[LootBoxManager] Invalid card index: {index}");
            return;
        }

        bool isEpic = (rarity == Rarity.Epic);
        float roll = Random.value;

        if (isEpic)
        {
            if (roll < EpicCardChance) SpawnCar(index, true);
            else if (roll < EpicMoneyChance) SpawnMoney(index, true);
            else SpawnPaint(index, true);
        }
        else
        {
            if (roll < NonEpicCardChance) SpawnMoney(index, false);
            else SpawnPaint(index, false);
        }

        if (_cardAppearSound != null) _cardAppearSound.Play();
    }

    private void SpawnCar(int index, bool isEpic)
    {
        CarLootItem carItem = _carLootDatabase.GetRandomItem(Rarity.Epic);
        if (carItem == null)
        {
            Debug.LogError("[LootBoxManager] Failed to get random car!");
            return;
        }

        transform.position = _carSpawnPoints[index].position; // Not mandatory; or use local var
        LootCar car = _lootCarSpawner.SpawnLootCar(_carSpawnPoints[index].position);
        car.Initialize(carItem.CarPrefab);

        _carLootCards[index].gameObject.SetActive(true);
        _carLootCards[index].ShowCard(car.gameObject);
        _colorLootCards[index].gameObject.SetActive(false);
        _moneyLootCards[index].gameObject.SetActive(false);

        if (index == _selectedButtonIndex)
        {
            _carLootDatabase.UnlockCar(carItem.CarId);
            YandexGame.savesData.UnlockEpicCar(carItem.CarId);
            YandexGame.SaveProgress();
        }
    }

    private void SpawnMoney(int index, bool isEpic)
    {
        Rarity rarity = isEpic ? Rarity.Epic : DetermineRarity();
        MoneyLootItem item = _moneyLootDatabase.GetRandomItem(rarity);
        if (item == null)
        {
            Debug.LogError($"[LootBoxManager] Missing money item for {rarity}");
            return;
        }

        _moneyLootCards[index].gameObject.SetActive(true);
        _moneyLootCards[index].ShowCard(item);
        _colorLootCards[index].gameObject.SetActive(false);
        _carLootCards[index].gameObject.SetActive(false);

        if (index == _selectedButtonIndex)
        {
            if (int.TryParse(item.Amount, out int amount))
            {
                YandexGame.savesData.AddMoney(amount);
            }
            YandexGame.SaveProgress();
        }
    }

    private void SpawnPaint(int index, bool isEpic)
    {
        Rarity rarity = isEpic ? Rarity.Epic : DetermineRarity();
        PaintLootItemSO item = _PaintLootDatabase.GetRandomItem(rarity);
        if (item == null)
        {
            Debug.LogError($"[LootBoxManager] Missing paint item for {rarity}");
            return;
        }

        LootPaintSphere sphere = _lootSpheresSpawner.SpawnLootSphere(rarity, _sphereSpawnPoints[index].position);
        _colorLootCards[index].gameObject.SetActive(true);
        _colorLootCards[index].ShowCard(item, sphere.gameObject);
        _carLootCards[index].gameObject.SetActive(false);
        _moneyLootCards[index].gameObject.SetActive(false);

        if (index == _selectedButtonIndex)
        {
            int paintId = sphere.GetComponent<LootPaintSphere>().GetPaintId();
            YandexGame.savesData.UnlockPaint(paintId);
            YandexGame.SaveProgress();

            FindObjectOfType<PaintIntegrationSystem>()?.ForceRefresh();
        }
    }

    private void DisableAllButtons()
    {
        for (int i = 0; i < _boxButtons.Length; i++)
        {
            _boxButtons[i].interactable = false;
        }
    }

    private void EnableAllButtons()
    {
        for (int i = 0; i < _boxButtons.Length; i++)
        {
            _boxButtons[i].interactable = true;
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromButtons();
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

                    if (roll < EpicCardChance) carsSpawned++;
                    else if (roll < EpicMoneyChance) epicMoney++;
                    else epicColors++;
                }
                else
                {
                    if (Random.value < NonEpicCardChance) nonEpicMoney++;
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




















//public class LootBoxManager : MonoBehaviour
//{
//    private const int CardsCount = 3;
//    private const float EPIC_CAR_CHANCE = 0.333f;
//    private const float EPIC_MONEY_CHANCE = 0.666f;
//    private const float NON_EPIC_MONEY_CHANCE = 0.5f;

//    [Header("Settings")]
//    [SerializeField] private int _guaranteedEpicAfterAttempts = 5;

//    [Header("Databases")]
//    [SerializeField] private PaintLootDatabase _PaintLootDatabase;
//    [SerializeField] private MoneyLootDatabase _moneyLootDatabase;
//    [SerializeField] private CarLootDatabase _carLootDatabase;

//    [Header("UI Components")]
//    [SerializeField] private MoneyLootCardController[] _moneyLootCards;
//    [SerializeField] private PaintLootCardController[] _colorLootCards;
//    [SerializeField] private CarLootCardController[] _carLootCards;
//    [SerializeField] private Button[] _boxButtons;

//    [Header("Spawn Points")]
//    [SerializeField] private Transform[] _sphereSpawnPoints;
//    [SerializeField] private Transform[] _carSpawnPoints;

//    [Header("Spawners")]
//    [SerializeField] private LootSpheresSpawner _lootSpheresSpawner;
//    [SerializeField] private LootCarSpawner _lootCarSpawner;

//    [Header("Rarity Chances")]
//    [SerializeField] private float _commonChance = 0.5f;
//    [SerializeField] private float _rareChance = 0.2f;
//    [SerializeField] private float _uniqueChance = 0.1f;
//    [SerializeField] private float _legendaryChance = 0.1f;
//    [SerializeField] private float _epicChance = 0.1f;

//    [Header("Box Components")]
//    [SerializeField] private BoxOpener[] _boxOpeners;
//    [SerializeField] private ObjectOnceShaker[] _boxShakers;

//    [Header("Audio")]
//    [SerializeField] private OnceSoundPlayer _cardAppearSound;

//    private int _consecutiveNonEpicCount = 0;
//    private int _selectedButtonIndex = -1;
//    private Rarity[] _pendingRarities;

//    private void OnValidate()
//    {
//        float total = _commonChance + _rareChance + _uniqueChance +
//                      _legendaryChance + _epicChance;

//        if (Mathf.Abs(total - 1f) > 0.001f)
//        {
//            Debug.LogWarning(" Not 100%!");
//        }
//    }

//    public Rarity DetermineRarity()
//    {
//        float roll = Random.value;
//        if (roll < _commonChance) return Rarity.Common;
//        if (roll < _commonChance + _rareChance) return Rarity.Rare;
//        if (roll < _commonChance + _rareChance + _uniqueChance) return Rarity.Unique;
//        if (roll < 1f - _epicChance) return Rarity.Legendary;
//        return Rarity.Epic;
//    }

//    private void Awake()
//    {
//        InitializeSystem();
//    }

//    private void InitializeSystem()
//    {
//        _PaintLootDatabase.Initialize();
//        _moneyLootDatabase.Initialize();
//        _carLootDatabase.Initialize();
//        ValidateDependencies();
//        SubscribeToButtons();
//    }

//    private void ValidateDependencies()
//    {
//        bool error = false;

//        if (_lootSpheresSpawner == null || _lootCarSpawner == null)
//        {
//            Debug.LogError("[LootBoxManager] Spawners are not assigned!");
//            error = true;
//        }

//        if (_moneyLootCards.Length < CardsCount ||
//            _colorLootCards.Length < CardsCount ||
//            _carLootCards.Length < CardsCount)
//        {
//            Debug.LogError("[LootBoxManager] Card arrays length mismatch!");
//            error = true;
//        }

//        if (_carLootDatabase == null)
//        {
//            Debug.LogError("[LootBoxManager] Car Loot Database is not assigned!");
//            error = true;
//        }

//        if (_boxOpeners == null || _boxOpeners.Length == 0)
//        {
//            Debug.LogError("[LootBoxManager] Box Openers are not assigned!");
//            error = true;
//        }
//        else if (_boxOpeners.Length != _boxButtons.Length)
//        {
//            Debug.LogError("[LootBoxManager] Box Openers count does not match Box Buttons count!");
//            error = true;
//        }

//        if (_boxShakers == null || _boxShakers.Length == 0)
//        {
//            Debug.LogError("[LootBoxManager] Box Shakers are not assigned!");
//            error = true;
//        }
//        else if (_boxShakers.Length != _boxButtons.Length)
//        {
//            Debug.LogError("[LootBoxManager] Box Shakers count does not match Box Buttons count!");
//            error = true;
//        }

//        if (_cardAppearSound == null)
//        {
//            Debug.LogError("[LootBoxManager] Card Appear Sound is not assigned!");
//            error = true;
//        }

//        for (int i = 0; i < CardsCount; i++)
//        {
//            if (_sphereSpawnPoints[i] == null || _carSpawnPoints[i] == null)
//            {
//                Debug.LogError($"[LootBoxManager] Spawn point {i} is null!");
//                error = true;
//            }
//        }

//        if (error) enabled = false;
//    }

//    private void SubscribeToButtons()
//    {
//        for (int i = 0; i < _boxButtons.Length; i++)
//        {
//            int index = i;
//            _boxButtons[i].onClick.AddListener(() => OnBoxSelected(index));
//        }
//    }

//    private void OnBoxSelected(int buttonIndex)
//    {
//        if (_selectedButtonIndex != -1 || buttonIndex < 0 || buttonIndex >= _boxButtons.Length)
//        {
//            return;
//        }

//        _selectedButtonIndex = buttonIndex;
//        DisableAllButtons();

//        // Ensure the correct box is shaken by checking if the button and shaker indices match
//        if (buttonIndex < _boxShakers.Length)
//        {
//            _boxShakers[buttonIndex].Shake();
//        }
//        else
//        {
//            Debug.LogError($"[LootBoxManager] Box Shaker index {buttonIndex} is out of range!");
//        }

//        // Wait for shake animation to complete before opening lids
//        StartCoroutine(OpenLidsAfterShake());
//    }

//    private IEnumerator OpenLidsAfterShake()
//    {
//        // Wait for shake animation to complete (0.5 seconds)
//        yield return new WaitForSeconds(0.5f);

//        // Start box opening animation for all boxes
//        foreach (var boxOpener in _boxOpeners)
//        {
//            boxOpener.OpenBox();
//        }

//        // Wait for lid animation to complete before showing cards
//        yield return new WaitForSeconds(1f);

//        // Generate and show cards
//        _pendingRarities = GenerateRarities();
//        ProcessCards(_pendingRarities);
//    }

//    private Rarity[] GenerateRarities()
//    {
//        Rarity[] rarities = new Rarity[CardsCount];
//        bool hasEpic = false;

//        for (int i = 0; i < CardsCount; i++)
//        {
//            rarities[i] = DetermineRarity();

//            if (rarities[i] == Rarity.Epic)
//            {
//                hasEpic = true;
//            }
//        }

//        if (!hasEpic)
//        {
//            _consecutiveNonEpicCount++;
//            if (_consecutiveNonEpicCount >= _guaranteedEpicAfterAttempts)
//            {
//                int forcedIndex = Random.Range(0, CardsCount);
//                rarities[forcedIndex] = Rarity.Epic;
//                _consecutiveNonEpicCount = 0; // Reset counter after forcing epic
//            }
//        }
//        else
//        {
//            _consecutiveNonEpicCount = 0; // Reset counter if we got epic naturally
//        }

//        return rarities;
//    }

//    private void ProcessCards(Rarity[] rarities)
//    {
//        // First, process the selected card
//        ProcessSingleCard(_selectedButtonIndex, rarities[_selectedButtonIndex]);

//        // Then process the rest of the cards with a delay
//        StartCoroutine(ProcessRemainingCards(rarities));
//    }

//    private void ProcessSingleCard(int index, Rarity rarity)
//    {
//        if (index < 0 || index >= CardsCount)
//        {
//            Debug.LogError($"[LootBoxManager] Invalid card index: {index}");
//            return;
//        }

//        // Определяем, является ли карточка действительно «выбранной» (той, по которой кликнули)
//        bool isThisTheChosenBox = (index == _selectedButtonIndex);

//        // В зависимости от редкости — генерируем тип награды (машина/деньги/краска)
//        if (rarity == Rarity.Epic)
//        {
//            float roll = Random.value;
//            if (roll < EPIC_CAR_CHANCE)
//            {
//                SpawnCar(index, isThisTheChosenBox);    // <-- передаём, выбранная ли это карточка
//            }
//            else if (roll < (EPIC_CAR_CHANCE + EPIC_MONEY_CHANCE))
//            {
//                SpawnMoney(index, isThisTheChosenBox);  // <-- передаём, выбранная ли это карточка
//            }
//            else
//            {
//                SpawnPaint(index, isThisTheChosenBox);  // <-- передаём, выбранная ли это карточка
//            }
//        }
//        else
//        {
//            // Если не эпик, то, например, половина — деньги, половина — краска (NON_EPIC_MONEY_CHANCE = 0.5)
//            // Но с той же логикой: передаём признак выбранной карточки
//            float roll = Random.value;
//            if (roll < NON_EPIC_MONEY_CHANCE)
//            {
//                SpawnMoney(index, isThisTheChosenBox);
//            }
//            else
//            {
//                SpawnPaint(index, isThisTheChosenBox);
//            }
//        }

//        // Проигрываем звук карты
//        if (_cardAppearSound != null)
//        {
//            _cardAppearSound.Play();
//        }
//    }

//    //private void ProcessSingleCard(int index, Rarity rarity)
//    //{
//    //    if (index < 0 || index >= CardsCount)
//    //    {
//    //        Debug.LogError($"[LootBoxManager] Invalid card index: {index}");
//    //        return;
//    //    }

//    //    // Determine reward type and spawn appropriate card
//    //    if (rarity == Rarity.Epic)
//    //    {
//    //        float roll = Random.value;
//    //        if (roll < EPIC_CAR_CHANCE)
//    //        {
//    //            SpawnCar(index, true);
//    //        }
//    //        else if (roll < EPIC_CAR_CHANCE + EPIC_MONEY_CHANCE)
//    //        {
//    //            SpawnMoney(index, true);
//    //        }
//    //        else
//    //        {
//    //            SpawnPaint(index, true);
//    //        }
//    //    }
//    //    else
//    //    {
//    //        float roll = Random.value;
//    //        if (roll < NON_EPIC_MONEY_CHANCE)
//    //        {
//    //            SpawnMoney(index, false);
//    //        }
//    //        else
//    //        {
//    //            SpawnPaint(index, false);
//    //        }
//    //    }

//    //    // Play sound when card appears
//    //    _cardAppearSound.Play();
//    //}

//    private IEnumerator ProcessRemainingCards(Rarity[] rarities)
//    {
//        // Wait for the first card to appear
//        yield return new WaitForSeconds(0.5f);

//        // Process all remaining cards simultaneously
//        for (int i = 0; i < CardsCount; i++)
//        {
//            if (i != _selectedButtonIndex) // Skip the already processed card
//            {
//                ProcessSingleCard(i, rarities[i]);
//            }
//        }
//    }

//    private void SpawnCar(int index, bool isSelected)
//    {
//        CarLootItem carItem = _carLootDatabase.GetRandomItem(Rarity.Epic);
//        if (carItem == null)
//        {
//            Debug.LogError("[LootBoxManager] Не удалось получить случайную машину!");
//            return;
//        }

//        Transform spawnPoint = _carSpawnPoints[index];
//        LootCar car = _lootCarSpawner.SpawnLootCar(spawnPoint.position);
//        car.Initialize(carItem.CarPrefab);

//        _carLootCards[index].gameObject.SetActive(true);
//        _carLootCards[index].ShowCard(car.gameObject);
//        _colorLootCards[index].gameObject.SetActive(false);
//        _moneyLootCards[index].gameObject.SetActive(false);

//        if (isSelected)
//        {
//            _carLootDatabase.UnlockCar(carItem.CarId);
//            YandexGame.savesData.UnlockEpicCar(carItem.CarId);
//            YandexGame.SaveProgress();
//        }
//    }

//    private void SpawnMoney(int index, bool isEpic)
//    {
//        if (index < 0 || index >= _moneyLootCards.Length)
//        {
//            Debug.LogError($"[LootBoxManager] Invalid money card index: {index}");
//            return;
//        }

//        Rarity rarity = isEpic ? Rarity.Epic : DetermineRarity();
//        MoneyLootItem item = _moneyLootDatabase.GetRandomItem(rarity);

//        if (item == null)
//        {
//            Debug.LogError($"Missing money item for rarity: {rarity}");
//            return;
//        }

//        _moneyLootCards[index].gameObject.SetActive(true);
//        _moneyLootCards[index].ShowCard(item);
//        _colorLootCards[index].gameObject.SetActive(false);
//        _carLootCards[index].gameObject.SetActive(false);

//        if (index == _selectedButtonIndex)
//        {
//            if (int.TryParse(item.Amount, out int amount))
//            {
//                YandexGame.savesData.AddMoney(amount);
//            }
//            else
//            {
//                Debug.LogError($"Failed to parse money amount from '{item.Amount}'");
//            }
//            YandexGame.SaveProgress();
//        }
//    }

//    private void SpawnPaint(int index, bool isEpic)
//    {
//        if (index < 0 || index >= _colorLootCards.Length)
//        {
//            Debug.LogError($"[LootBoxManager] Invalid paint card index: {index}");
//            return;
//        }

//        Rarity rarity = isEpic ? Rarity.Epic : DetermineRarity();
//        PaintLootItemSO item = _PaintLootDatabase.GetRandomItem(rarity);
//        Transform spawnPoint = _sphereSpawnPoints[index];
//        LootPaintSphere sphere = _lootSpheresSpawner.SpawnLootSphere(rarity, spawnPoint.position);

//        _colorLootCards[index].gameObject.SetActive(true);
//        _colorLootCards[index].ShowCard(item, sphere.gameObject);
//        _carLootCards[index].gameObject.SetActive(false);
//        _moneyLootCards[index].gameObject.SetActive(false);

//        if (index == _selectedButtonIndex)
//        {
//            int paintId = sphere.GetComponent<LootPaintSphere>().GetPaintId();
//            YandexGame.savesData.UnlockPaint(paintId);
//            YandexGame.SaveProgress();

//            FindObjectOfType<PaintIntegrationSystem>()?.ForceRefresh();
//        }
//    }

//    private void DisableAllButtons()
//    {
//        foreach (var button in _boxButtons)
//        {
//            button.interactable = false;
//        }
//    }

//    private void EnableAllButtons()
//    {
//        foreach (var button in _boxButtons)
//        {
//            button.interactable = true;
//        }
//    }

//    private void UnsubscribeFromButtons()
//    {
//        for (int i = 0; i < _boxButtons.Length; i++)
//        {
//            _boxButtons[i].onClick.RemoveAllListeners();
//        }
//    }

//    private void OnEnable()
//    {
//        YandexGame.RewardVideoEvent += OnRewardVideoComplete;
//    }

//    private void OnDisable()
//    {
//        YandexGame.RewardVideoEvent -= OnRewardVideoComplete;
//        UnsubscribeFromButtons();
//    }

//    private void OnRewardVideoComplete(int id)
//    {
//        if (id == 1 && _pendingRarities != null)
//        {
//            ProcessCards(_pendingRarities);
//            _pendingRarities = null;
//        }
//        else
//        {
//            EnableAllButtons();
//            _pendingRarities = null;
//        }
//    }

//#if UNITY_EDITOR
//    [ContextMenu("Run Balance Test (1000 iterations)")]
//    private void RunMassTest()
//    {
//        const int iterations = 1000;
//        int totalEpics = 0;
//        int carsSpawned = 0;
//        int epicMoney = 0;
//        int epicColors = 0;
//        int nonEpicMoney = 0;
//        int nonEpicColors = 0;
//        int forcedEpics = 0;
//        int currentNonEpicStreak = 0;

//        var lootDb = _PaintLootDatabase;
//        var moneyDb = _moneyLootDatabase;

//        for (int i = 0; i < iterations; i++)
//        {
//            bool forceEpic = currentNonEpicStreak >= _guaranteedEpicAfterAttempts;
//            Rarity[] rarities = new Rarity[CardsCount];
//            bool hasEpic = false;

//            for (int j = 0; j < CardsCount; j++)
//            {
//                rarities[j] = DetermineRarity();

//                if (rarities[j] == Rarity.Epic) hasEpic = true;
//            }

//            if (forceEpic && !hasEpic)
//            {
//                int forcedIndex = Random.Range(0, CardsCount);
//                rarities[forcedIndex] = Rarity.Epic;
//                hasEpic = true;
//                forcedEpics++;
//            }

//            for (int j = 0; j < CardsCount; j++)
//            {
//                Rarity rarity = rarities[j];
//                bool isEpic = rarity == Rarity.Epic;

//                if (isEpic)
//                {
//                    totalEpics++;
//                    float roll = Random.value;

//                    if (roll < EPIC_CAR_CHANCE) carsSpawned++;
//                    else if (roll < EPIC_MONEY_CHANCE) epicMoney++;
//                    else epicColors++;
//                }
//                else
//                {
//                    if (Random.value < NON_EPIC_MONEY_CHANCE) nonEpicMoney++;
//                    else nonEpicColors++;
//                }
//            }

//            currentNonEpicStreak = hasEpic ? 0 : currentNonEpicStreak + 1;
//        }

//        string report = $"<color=#00FF00>TEST RESULTS ({iterations} iterations):</color>\n" +
//            $"Total Epics: {totalEpics} ({totalEpics / (float)(iterations * CardsCount) * 100:F1}%)\n" +
//            $"Epic Cars: {carsSpawned} ({carsSpawned / (float)totalEpics * 100:F1}% of epics)\n" +
//            $"Epic Money: {epicMoney} ({epicMoney / (float)totalEpics * 100:F1}%)\n" +
//            $"Epic Colors: {epicColors} ({epicColors / (float)totalEpics * 100:F1}%)\n" +
//            $"Non-Epic Money: {nonEpicMoney} ({nonEpicMoney / (float)(iterations * CardsCount) * 100:F1}%)\n" +
//            $"Non-Epic Colors: {nonEpicColors} ({nonEpicColors / (float)(iterations * CardsCount) * 100:F1}%)\n" +
//            $"Forced Epics: {forcedEpics} (after {_guaranteedEpicAfterAttempts} misses)";

//        Debug.Log(report);

//        _consecutiveNonEpicCount = 0;
//    }
//#endif
//}