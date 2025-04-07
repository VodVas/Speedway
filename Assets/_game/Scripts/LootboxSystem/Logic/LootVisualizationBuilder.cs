using System;
using UnityEngine;

public class LootVisualizationBuilder : MonoBehaviour
{
    private const int CARDS_COUNT = 3;

    [SerializeField] private MoneyLootCardController[] _moneyCards = new MoneyLootCardController[CARDS_COUNT];
    [SerializeField] private PaintLootCardController[] _paintCards = new PaintLootCardController[CARDS_COUNT];
    [SerializeField] private CarLootCardController[] _carCards = new CarLootCardController[CARDS_COUNT];
    [SerializeField] private Transform[] _spherePoints = new Transform[CARDS_COUNT];
    [SerializeField] private Transform[] _carPoints = new Transform[CARDS_COUNT];
    [SerializeField] private CardLootSpheresSpawner _sphereSpawner;
    [SerializeField] private CardLootCarSpawner _carSpawner;
    [SerializeField] private OnceSoundPlayer _soundPlayer;

    private LootPaintSphere[] _spheres = new LootPaintSphere[CARDS_COUNT];
    private LootCar[] _cars = new LootCar[CARDS_COUNT];
    private int _sphereCount;
    private int _carCount;

    public event Action<int, LootRewardType, object> CardVisualized;

    private void Awake()
    {
        if (!ValidateComponents()) enabled = false;
    }

    private bool ValidateComponents()
    {
        if (!_sphereSpawner || !_carSpawner || !_soundPlayer) return false;

        for (var i = 0; i < CARDS_COUNT; i++)
        {
            if (!_moneyCards[i] || !_paintCards[i] || !_carCards[i] ||
                !_spherePoints[i] || !_carPoints[i]) return false;
        }
        return true;
    }

    public void VisualizeCar(int index, CarLootItem item)
    {
        if (!IsValidRequest(index, item)) return;

        var car = _carSpawner.SpawnCarForCard(_carPoints[index].position, index);
        if (!car) return;

        car.Initialize(item.CarPrefab);
        StoreAndShow(index, car, _cars, ref _carCount, _carCards[index], item);
    }

    public void VisualizeMoney(int index, MoneyLootItem item)
    {
        if (!IsValidRequest(index, item)) return;

        _moneyCards[index].gameObject.SetActive(true);
        _moneyCards[index].ShowCard(item);
        UpdateCardState(index, LootRewardType.Money, item);
    }

    public void VisualizePaint(int index, PaintLootItemSO item, Rarity rarity)
    {
        if (!IsValidRequest(index, item)) return;

        var sphere = _sphereSpawner.SpawnSphereForCard(rarity, _spherePoints[index].position, index);
        if (!sphere) return;

        StoreAndShow(index, sphere, _spheres, ref _sphereCount, _paintCards[index], item);
    }

    public void ResetAllVisuals()
    {
        for (var i = 0; i < CARDS_COUNT; i++)
        {
            _moneyCards[i].gameObject.SetActive(false);
            _paintCards[i].gameObject.SetActive(false);
            _carCards[i].gameObject.SetActive(false);
        }

        ReleaseObjects(_cars, ref _carCount);
        ReleaseObjects(_spheres, ref _sphereCount);
    }

    private void StoreAndShow<T>(int index, T obj, T[] array, ref int count,
        MonoBehaviour card, object item) where T : Component
    {
        array[count++] = obj;
        card.gameObject.SetActive(true);

        switch (card)
        {
            case CarLootCardController carCard:
                carCard.ShowCard(obj.gameObject);
                break;
            case PaintLootCardController paintCard:
                paintCard.ShowCard((item as PaintLootItemSO), obj.gameObject);
                break;
        }

        _soundPlayer.Play();
        CardVisualized?.Invoke(index, GetRewardType<T>(), item);
    }

    private LootRewardType GetRewardType<T>()
    {
        return typeof(T) == typeof(LootCar) ? LootRewardType.Car :
            typeof(T) == typeof(LootPaintSphere) ? LootRewardType.Paint :
            LootRewardType.Money;
    }

    private bool IsValidRequest<T>(int index, T item)
    {
        if (index < 0 || index >= CARDS_COUNT || item == null)
        {
            Debug.LogError($"Invalid request: {typeof(T).Name}");
            return false;
        }
        return true;
    }

    private void UpdateCardState(int index, LootRewardType type, object item)
    {
        _soundPlayer.Play();
        CardVisualized?.Invoke(index, type, item);
    }

    private void ReleaseObjects<T>(T[] items, ref int count) where T : ITerminatable
    {
        for (var i = 0; i < count; i++) items[i]?.Terminate();
        count = 0;
    }
}