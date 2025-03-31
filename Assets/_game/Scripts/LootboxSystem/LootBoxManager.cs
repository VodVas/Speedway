using UnityEngine;
using UnityEngine.UI;

public class LootBoxManager : MonoBehaviour
{
    private const int CardsCount = 3;

    [SerializeField] private int _guaranteedEpicAfterAttempts = 5;
    [Space(1)]
    [SerializeField] private MoneyLootCardController[] _moneyLootCards;
    [SerializeField] private PaintLootCardController[] _colorLootCards;
    [SerializeField] private CarLootController[] _carLootCards;
    [SerializeField] private LootDatabase _lootDatabase;
    [SerializeField] private LootSpheresSpawner _lootSpheresSpawner;
    [SerializeField] private LootCarSpawner _lootCarSpawner;
    [SerializeField] private Button[] _boxButtons;
    [SerializeField] private Transform[] _sphereSpawnPoints;
    [SerializeField] private Transform[] _carSpawnPoints;

    private int _consecutiveNonEpicCount = 0;

    private void Awake()
    {
        InitializeSystem();
    }

    private void OnDisable()
    {
        UnsubscribeFromButtons();
    }

    private void InitializeSystem()
    {
        _lootDatabase.Initialize();
        ValidateDependencies();
        SubscribeToButtons();
    }

    private void ValidateDependencies()
    {
        if (_lootSpheresSpawner == null || _lootCarSpawner == null)
        {
            Debug.LogError("[LootBoxManager] Spawners are not assigned!");
            enabled = false;
            return;
        }

        if (_colorLootCards.Length < CardsCount || _carLootCards.Length < CardsCount ||
            _sphereSpawnPoints.Length < CardsCount || _carSpawnPoints.Length < CardsCount)
        {
            Debug.LogError("[LootBoxManager] Arrays have insufficient length!");
            enabled = false;
            return;
        }

        for (int i = 0; i < CardsCount; i++)
        {
            if (_sphereSpawnPoints[i] == null || _carSpawnPoints[i] == null)
            {
                Debug.LogError($"[LootBoxManager] Spawn point at index {i} is null!");
                enabled = false;
                return;
            }
        }
    }

    private void SubscribeToButtons()
    {
        for (int i = 0; i < _boxButtons.Length; i++)
        {
            int index = i;
            _boxButtons[i].onClick.AddListener(() => OnBoxSelected());
        }
    }

    private void OnBoxSelected()
    {
        DisableAllButtons();

        bool forceEpic = _consecutiveNonEpicCount >= _guaranteedEpicAfterAttempts;
        Rarity[] rarities = new Rarity[CardsCount];

        for (int i = 0; i < CardsCount; i++)
        {
            rarities[i] = _lootDatabase.GetRandomRarity();
        }

        if (forceEpic)
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

        bool hasEpicInBox = false;

        for (int i = 0; i < CardsCount; i++)
        {
            Rarity rarity = rarities[i];
            bool isEpic = rarity == Rarity.Epic;
            bool spawnCar = isEpic && Random.value < 0.5f;

            if (spawnCar)
            {
                SpawnCar(i);
            }
            else
            {
                SpawnColor(rarity, i);
            }

            if (isEpic)
            {
                hasEpicInBox = true;
            }
        }

        if (hasEpicInBox)
        {
            _consecutiveNonEpicCount = 0;
        }
        else
        {
            _consecutiveNonEpicCount++;
        }
    }

    private void SpawnCar(int index)
    {
        Transform spawnPoint = _carSpawnPoints[index];
        LootCar car = _lootCarSpawner.SpawnLootCar(spawnPoint.position);

        _carLootCards[index].gameObject.SetActive(true);
        _carLootCards[index].ShowCard(car.gameObject);
        _colorLootCards[index].gameObject.SetActive(false);
    }

    private void SpawnColor(Rarity rarity, int index)
    {
        PaintLootItem item = _lootDatabase.GetRandomItem(rarity);
        Transform spawnPoint = _sphereSpawnPoints[index];
        LootPaintSphere sphere = _lootSpheresSpawner.SpawnLootSphere(rarity, spawnPoint.position);

        _colorLootCards[index].gameObject.SetActive(true);
        _colorLootCards[index].ShowCard(item, sphere.gameObject);
        _carLootCards[index].gameObject.SetActive(false);
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
    [ContextMenu("Run Spawn Test (100 iterations)")]
    private void RunMassTest()
    {
        int carsSpawned = 0;
        int epicColorsSpawned = 0;
        int otherRarities = 0;

        for (int i = 0; i < 100; i++)
        {
            Rarity rarity = _lootDatabase.GetRandomRarity();
            bool isEpic = rarity == Rarity.Epic;
            bool spawnCar = isEpic && Random.value < 0.5f;

            if (spawnCar) carsSpawned++;
            else if (isEpic) epicColorsSpawned++;
            else otherRarities++;
        }

        Debug.Log(
            $"<color=yellow>TEST RESULTS:</color>\n" +
            $"Cars: {carsSpawned}\n" +
            $"Epic Colors: {epicColorsSpawned}\n" +
            $"Other rarities: {otherRarities}\n" +
            $"Epic/Car ratio: {(float)carsSpawned / (carsSpawned + epicColorsSpawned) * 100}%");
    }
#endif
}






//public class LootBoxManager : MonoBehaviour
//{
//    private const int CardsCount = 3;

//    [SerializeField] private PaintLootCardController[] _colorLootCards;
//    [SerializeField] private CarLootController[] _carLootCards;
//    [SerializeField] private LootDatabase _lootDatabase;
//    [SerializeField] private LootSpheresSpawner _lootSpheresSpawner;
//    [SerializeField] private LootCarSpawner _lootCarSpawner;
//    [SerializeField] private Button[] _boxButtons;
//    [SerializeField] private Transform[] _sphereSpawnPoints;
//    [SerializeField] private Transform[] _carSpawnPoints;

//    private void Awake()
//    {
//        InitializeSystem();
//    }

//    private void OnDisable()
//    {
//        UnsubscribeFromButtons();
//    }

//    private void InitializeSystem()
//    {
//        _lootDatabase.Initialize();
//        ValidateDependencies();
//        SubscribeToButtons();
//    }

//#if UNITY_EDITOR
//    [ContextMenu("Run Spawn Test (100 iterations)")]
//    private void RunMassTest()
//    {
//        int carsSpawned = 0;
//        int epicColorsSpawned = 0;
//        int otherRarities = 0;

//        for (int i = 0; i < 100; i++)
//        {
//            Rarity rarity = _lootDatabase.GetRandomRarity();
//            bool isEpic = rarity == Rarity.Epic;
//            bool spawnCar = isEpic && Random.value < 0.5f;

//            if (spawnCar) carsSpawned++;
//            else if (isEpic) epicColorsSpawned++;
//            else otherRarities++;
//        }

//        Debug.Log(
//            $"<color=yellow>TEST RESULTS:</color>\n" +
//            $"Cars: {carsSpawned}\n" +
//            $"Epic Colors: {epicColorsSpawned}\n" +
//            $"Other rarities: {otherRarities}\n" +
//            $"Epic/Car ratio: {(float)carsSpawned / (carsSpawned + epicColorsSpawned) * 100}%");
//    }
//#endif

//    private void ValidateDependencies()
//    {
//        if (_lootSpheresSpawner == null || _lootCarSpawner == null)
//        {
//            Debug.LogError("[LootBoxManager] Spawners are not assigned!");
//            enabled = false;
//            return;
//        }

//        if (_colorLootCards.Length < CardsCount || _carLootCards.Length < CardsCount ||
//            _sphereSpawnPoints.Length < CardsCount || _carSpawnPoints.Length < CardsCount)
//        {
//            Debug.LogError("[LootBoxManager] Arrays have insufficient length!");
//            enabled = false;
//            return;
//        }

//        for (int i = 0; i < CardsCount; i++)
//        {
//            if (_sphereSpawnPoints[i] == null || _carSpawnPoints[i] == null)
//            {
//                Debug.LogError($"[LootBoxManager] Spawn point at index {i} is null!");
//                enabled = false;
//                return;
//            }
//        }
//    }

//    private void SubscribeToButtons()
//    {
//        for (int i = 0; i < _boxButtons.Length; i++)
//        {
//            int index = i;
//            _boxButtons[i].onClick.AddListener(() => OnBoxSelected(index));
//        }
//    }

//    private void OnBoxSelected(int selectedIndex)
//    {
//        DisableAllButtons();

//        for (int i = 0; i < CardsCount; i++)
//        {
//            ProcessCard(i);
//        }
//    }

//    private void ProcessCard(int index)
//    {
//        Rarity rarity = _lootDatabase.GetRandomRarity();
//        bool isEpic = rarity == Rarity.Epic;
//        bool spawnCar = isEpic && Random.value < 0.5f;

//        if (spawnCar)
//        {
//            SpawnCar(index);
//        }
//        else
//        {
//            SpawnColor(rarity, index);
//        }
//    }

//    private void SpawnCar(int index)
//    {
//        Transform spawnPoint = _carSpawnPoints[index];
//        LootCar car = _lootCarSpawner.SpawnLootCar(spawnPoint.position);

//        _carLootCards[index].gameObject.SetActive(true);
//        _carLootCards[index].ShowCard(car.gameObject);
//        _colorLootCards[index].gameObject.SetActive(false);
//    }

//    private void SpawnColor(Rarity rarity, int index)
//    {
//        if (rarity == Rarity.Epic)
//        {
//            rarity = Rarity.Epic;
//        }

//        LootItem item = _lootDatabase.GetRandomItem(rarity);
//        Transform spawnPoint = _sphereSpawnPoints[index];
//        LootPaintSphere sphere = _lootSpheresSpawner.SpawnLootSphere(rarity, spawnPoint.position);

//        _colorLootCards[index].gameObject.SetActive(true);
//        _colorLootCards[index].ShowCard(item, sphere.gameObject);
//        _carLootCards[index].gameObject.SetActive(false);
//    }

//    private void DisableAllButtons()
//    {
//        for (int i = 0; i < _boxButtons.Length; i++)
//        {
//            _boxButtons[i].interactable = false;
//        }
//    }

//    private void UnsubscribeFromButtons()
//    {
//        for (int i = 0; i < _boxButtons.Length; i++)
//        {
//            _boxButtons[i].onClick.RemoveAllListeners();
//        }
//    }
//}