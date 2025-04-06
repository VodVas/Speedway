using UnityEngine;

// ≈дина€ точка доступа ко всем базам данных лута
public class LootDatabaseMediator : MonoBehaviour
{
    [SerializeField] private PaintLootDatabase _paintLootDatabase;
    [SerializeField] private MoneyLootDatabase _moneyLootDatabase;
    [SerializeField] private CarLootDatabase _carLootDatabase;

    private bool _initialized;

    private void Awake()
    {
        InitializeDatabases();
    }

    private void InitializeDatabases()
    {
        if (_paintLootDatabase == null || _moneyLootDatabase == null || _carLootDatabase == null)
        {
            Debug.LogError("[LootDatabaseMediator] One or more databases are missing!");
            enabled = false;
            return;
        }

        _paintLootDatabase.Initialize();
        _moneyLootDatabase.Initialize();
        _carLootDatabase.Initialize();

        _initialized = true;
    }

    public T GetRandomItem<T>(Rarity rarity, LootRewardType type) where T : class
    {
        if (!_initialized) return null;

        switch (type)
        {
            case LootRewardType.Car:
                return _carLootDatabase.GetRandomItem(Rarity.Epic) as T;

            case LootRewardType.Money:
                return _moneyLootDatabase.GetRandomItem(rarity) as T;

            case LootRewardType.Paint:
                return _paintLootDatabase.GetRandomItem(rarity) as T;

            default:
                Debug.LogError($"[LootDatabaseMediator] Unknown reward type: {type}");
                return null;
        }
    }

    // —пециализированные методы дл€ каждого типа награды
    public CarLootItem GetRandomCarItem(Rarity rarity)
    {
        return rarity == Rarity.Epic ? _carLootDatabase.GetRandomItem(rarity) : null;
    }

    public MoneyLootItem GetRandomMoneyItem(Rarity rarity)
    {
        return _moneyLootDatabase.GetRandomItem(rarity);
    }

    public PaintLootItemSO GetRandomPaintItem(Rarity rarity)
    {
        return _paintLootDatabase.GetRandomItem(rarity);
    }

    public CarLootDatabase GetCarDatabase() => _carLootDatabase;
    public MoneyLootDatabase GetMoneyDatabase() => _moneyLootDatabase;
    public PaintLootDatabase GetPaintDatabase() => _paintLootDatabase;
}
