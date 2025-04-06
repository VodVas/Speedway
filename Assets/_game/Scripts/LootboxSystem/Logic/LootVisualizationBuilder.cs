using System;
using UnityEngine;

public class LootVisualizationBuilder : MonoBehaviour
{
    private const int CARDS_COUNT = 3;

    [Header("Card Controllers")]
    [SerializeField] private MoneyLootCardController[] _moneyLootCards = new MoneyLootCardController[CARDS_COUNT];
    [SerializeField] private PaintLootCardController[] _colorLootCards = new PaintLootCardController[CARDS_COUNT];
    [SerializeField] private CarLootCardController[] _carLootCards = new CarLootCardController[CARDS_COUNT];

    [Header("Spawn Points")]
    [SerializeField] private Transform[] _sphereSpawnPoints = new Transform[CARDS_COUNT];
    [SerializeField] private Transform[] _carSpawnPoints = new Transform[CARDS_COUNT];

    [Header("Spawners")]
    [SerializeField] private LootSpheresSpawner _lootSpheresSpawner;
    [SerializeField] private LootCarSpawner _lootCarSpawner;

    [Header("Audio")]
    [SerializeField] private OnceSoundPlayer _cardAppearSound;

    private bool _initialized;

    public event Action<int, LootRewardType, object> CardVisualized;

    private void Awake()
    {
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        if (_moneyLootCards.Length < CARDS_COUNT || _colorLootCards.Length < CARDS_COUNT ||
            _carLootCards.Length < CARDS_COUNT || _sphereSpawnPoints.Length < CARDS_COUNT ||
            _carSpawnPoints.Length < CARDS_COUNT || _lootSpheresSpawner == null ||
            _lootCarSpawner == null || _cardAppearSound == null)
        {
            Debug.Log("[LootVisualizationBuilder] Components mismatch or missing!");
            enabled = false;
            return;
        }

        for (int i = 0; i < CARDS_COUNT; i++)
        {
            if (_moneyLootCards[i] == null || _colorLootCards[i] == null ||
                _carLootCards[i] == null || _sphereSpawnPoints[i] == null ||
                _carSpawnPoints[i] == null)
            {
                Debug.Log($"[LootVisualizationBuilder] Required component at index {i} is null!");
                enabled = false;
                return;
            }
        }

        _initialized = true;
    }

    public void VisualizeCar(int cardIndex, CarLootItem carItem)
    {
        if (!_initialized || cardIndex < 0 || cardIndex >= CARDS_COUNT || carItem == null)
            return;

        LootCar car = _lootCarSpawner.SpawnLootCar(_carSpawnPoints[cardIndex].position);

        if (car != null)
        {
            car.Initialize(carItem.CarPrefab);

            _carLootCards[cardIndex].gameObject.SetActive(true);
            _carLootCards[cardIndex].ShowCard(car.gameObject);
            _colorLootCards[cardIndex].gameObject.SetActive(false);
            _moneyLootCards[cardIndex].gameObject.SetActive(false);

            if (_cardAppearSound != null)
                _cardAppearSound.Play();

            CardVisualized?.Invoke(cardIndex, LootRewardType.Car, carItem);
        }
        else
        {
            Debug.Log("[LootVisualizationBuilder] Failed to spawn car!");
            enabled = false;
            return;
        }
    }

    public void VisualizeMoney(int cardIndex, MoneyLootItem moneyItem)
    {
        if (!_initialized || cardIndex < 0 || cardIndex >= CARDS_COUNT || moneyItem == null)
            return;

        _moneyLootCards[cardIndex].gameObject.SetActive(true);
        _moneyLootCards[cardIndex].ShowCard(moneyItem);
        _colorLootCards[cardIndex].gameObject.SetActive(false);
        _carLootCards[cardIndex].gameObject.SetActive(false);

        if (_cardAppearSound != null) _cardAppearSound.Play();

        CardVisualized?.Invoke(cardIndex, LootRewardType.Money, moneyItem);
    }

    public void VisualizePaint(int cardIndex, PaintLootItemSO paintItem, Rarity rarity)
    {
        if (!_initialized || cardIndex < 0 || cardIndex >= CARDS_COUNT || paintItem == null)
            return;

        LootPaintSphere sphere = _lootSpheresSpawner.SpawnLootSphere(
            rarity, _sphereSpawnPoints[cardIndex].position);

        if (sphere != null)
        {
            _colorLootCards[cardIndex].gameObject.SetActive(true);
            _colorLootCards[cardIndex].ShowCard(paintItem, sphere.gameObject);
            _carLootCards[cardIndex].gameObject.SetActive(false);
            _moneyLootCards[cardIndex].gameObject.SetActive(false);

            if (_cardAppearSound != null) _cardAppearSound.Play();

            CardVisualized?.Invoke(cardIndex, LootRewardType.Paint, sphere);
        }
        else
        {
            Debug.Log("[LootVisualizationBuilder] Failed to spawn paint sphere!");
            enabled = false;
            return;
        }
    }

    public void ResetAllVisuals()
    {
        for (int i = 0; i < CARDS_COUNT; i++)
        {
            _moneyLootCards[i].gameObject.SetActive(false);
            _colorLootCards[i].gameObject.SetActive(false);
            _carLootCards[i].gameObject.SetActive(false);
        }
    }
}