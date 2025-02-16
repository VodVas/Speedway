using UnityEngine;
using YG;
using Zenject;
using System.Collections.Generic;
using System;

public class SaveService : IInitializable
{
    private const int MAX_MODIFICATION_COUNT = 5;

    public event Action OnMoneyChanged;

    public int Money
    {
        get => YandexGame.savesData.money;
        private set
        {
            YandexGame.savesData.money = value;
            OnMoneyChanged?.Invoke();
        }
    }

    public int LastUsedCarId
    {
        get => YandexGame.savesData.lastUsedCarId;
        set => YandexGame.savesData.lastUsedCarId = value;
    }

    public List<int> PurchasedCarIDs => YandexGame.savesData.purchasedCarIDs;
    public List<PurchasedUpgrade> PurchasedUpgrades => YandexGame.savesData.purchasedUpgrades;
    public List<PurchasedModification> PurchasedModifications => YandexGame.savesData.purchasedModifications;

    public void Initialize()
    {
        Debug.Log("[SaveService] Initialize() → Загрузка сохранений из YandexGame");
        YandexGame.LoadProgress();

        if (YandexGame.savesData.isFirstSession)
        {
            YandexGame.savesData.isFirstSession = false;
            // Пример: Money = 1500;
            // Save();
        }
    }

    public bool TrySpendMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("[SaveService] Недопустимо списывать отрицательную сумму!");
            return false;
        }
        if (Money < amount)
        {
            return false;
        }

        Money -= amount;
        return true;
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("[SaveService] Нельзя добавлять отрицательную сумму!");
            return;
        }

        Money += amount;
    }

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
            var record = new PurchasedUpgrade { carId = carId, upgradeId = upgradeId };
            PurchasedUpgrades.Add(record);
        }
    }

    public int GetCarModificationCount(int carId, int modificationId)
    {
        for (int i = 0; i < PurchasedModifications.Count; i++)
        {
            if (PurchasedModifications[i].carId == carId &&
                PurchasedModifications[i].modificationId == modificationId)
            {
                return PurchasedModifications[i].count;
            }
        }

        return 0;
    }

    public void AddCarModification(int carId, int modificationId)
    {
        int currentCount = GetCarModificationCount(carId, modificationId);

        if (currentCount >= MAX_MODIFICATION_COUNT)
        {
            Debug.LogWarning($"[SaveService] Модификация {modificationId} для машины {carId} уже куплена 5 раз!");
            return;
        }

        bool found = false;

        for (int i = 0; i < PurchasedModifications.Count; i++)
        {
            if (PurchasedModifications[i].carId == carId &&
                PurchasedModifications[i].modificationId == modificationId)
            {
                var modif = PurchasedModifications[i];
                modif.count++;
                PurchasedModifications[i] = modif;
                found = true;
                break;
            }
        }
        if (!found)
        {
            var newRecord = new PurchasedModification
            {
                carId = carId,
                modificationId = modificationId,
                count = 1
            };
            PurchasedModifications.Add(newRecord);
        }
    }

    public void Save()
    {
        YandexGame.SaveProgress();
        Debug.Log("[SaveService] Сохранение выполнено!");
    }
}