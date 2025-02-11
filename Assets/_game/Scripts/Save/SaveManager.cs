using UnityEngine;
using YG;
using Zenject;
using System.Collections.Generic;
using System;

public class SaveManager : IInitializable
{
    public int Money
    {
        get => YandexGame.savesData.money;
        private set
        {
            YandexGame.savesData.money = value;
            OnMoneyChanged?.Invoke();
        }
    }

    public List<int> PurchasedCarIDs => YandexGame.savesData.purchasedCarIDs;
    public List<PurchasedUpgrade> PurchasedUpgrades => YandexGame.savesData.purchasedUpgrades;

    public event Action OnMoneyChanged;

    public void Initialize()
    {
        Debug.Log("[SaveManager] Initialize() → Загружаем сохранения из YandexGame");

        YandexGame.LoadProgress();

        if (YandexGame.savesData.isFirstSession)
        {
            // Можно дать стартовые деньги и прочие параметры
            YandexGame.savesData.isFirstSession = false;
            // Money = 1500; // пример
            // SaveProgress();
        }
    }

    #region Деньги

    public bool TrySpendMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("[SaveManager] Нельзя списывать отрицательную сумму!");
            return false;
        }

        if (Money >= amount)
        {
            Money -= amount;
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("[SaveManager] Нельзя добавлять отрицательную сумму!");
            return;
        }

        Money += amount;
    }

    #endregion

    #region Машины

    public void AddCar(int carId)
    {
        if (!PurchasedCarIDs.Contains(carId))
        {
            PurchasedCarIDs.Add(carId);
        }
    }

    public bool HasCar(int carId)
    {
        return PurchasedCarIDs.Contains(carId);
    }

    #endregion

    #region Апгрейды

    public bool HasCarUpgrade(int carId, int upgradeId)
    {
        for (int i = 0; i < PurchasedUpgrades.Count; i++)
        {
            if (PurchasedUpgrades[i].carId == carId && PurchasedUpgrades[i].upgradeId == upgradeId)
                return true;
        }
        return false;
    }

    public void AddCarUpgrade(int carId, int upgradeId)
    {
        if (!HasCarUpgrade(carId, upgradeId))
        {
            PurchasedUpgrade record = new PurchasedUpgrade { carId = carId, upgradeId = upgradeId };
            PurchasedUpgrades.Add(record);
        }
    }

    #endregion

    public void Save()
    {
        YandexGame.SaveProgress();
        Debug.Log("[SaveManager] Сохранение выполнено!");
    }
}