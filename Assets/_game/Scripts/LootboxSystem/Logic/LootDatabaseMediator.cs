using UnityEngine;

public class LootDatabaseMediator : MonoBehaviour
{
    [SerializeField] private PaintLootDatabase _paintLootDatabase;
    [SerializeField] private MoneyLootDatabase _moneyLootDatabase;
    [SerializeField] private CarLootDatabase _carLootDatabase;

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
    }

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
}