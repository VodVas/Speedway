using UnityEngine;
using YG;
using Zenject;
using System.Collections.Generic;
using System;

public sealed class SaveService : IInitializable
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

    // 1. Конструктор (у нас нет входных параметров — Zenject сам создаст)
    public SaveService() { }

    // 2. Событие IInitializable
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

    // 3. Свойства — уже объявлены выше

    // 4. Методы для работы с деньгами
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

    // 5. Методы для работы с машинами
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

    // 6. Методы для работы с апгрейдами (у вас уже реализовано, не меняем)
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

    // 7. Методы для работы с МОДИФИКАЦИЯМИ
    public int GetCarModificationCount(int carId, int modificationId)
    {
        // Возвращаем, сколько раз уже куплено
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

    public bool IsModificationMaxed(int carId, int modificationId)
    {
        return GetCarModificationCount(carId, modificationId) >= MAX_MODIFICATION_COUNT;
    }

    public void AddCarModification(int carId, int modificationId)
    {
        // Если уже купили 5 раз — не даём купить больше
        int currentCount = GetCarModificationCount(carId, modificationId);
        if (currentCount >= MAX_MODIFICATION_COUNT)
        {
            Debug.LogWarning($"[SaveService] Модификация {modificationId} для машины {carId} уже куплена 5 раз!");
            return;
        }

        // Находим запись либо создаём новую
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

    // 8. Сохранение
    public void Save()
    {
        YandexGame.SaveProgress();
        Debug.Log("[SaveService] Сохранение выполнено!");
    }
}







//public class SaveService : IInitializable
//{
//    public int Money
//    {
//        get => YandexGame.savesData.money;
//        private set
//        {
//            YandexGame.savesData.money = value;
//            OnMoneyChanged?.Invoke();
//        }
//    }

//    public List<int> PurchasedCarIDs => YandexGame.savesData.purchasedCarIDs;
//    public List<PurchasedUpgrade> PurchasedUpgrades => YandexGame.savesData.purchasedUpgrades;

//    public event Action OnMoneyChanged;

//    public int LastUsedCarId
//    {
//        get => YandexGame.savesData.lastUsedCarId;
//        set => YandexGame.savesData.lastUsedCarId = value;
//    }

//    public void Initialize()
//    {
//        Debug.Log("[SaveManager] Initialize() → Загружаем сохранения из YandexGame");

//        YandexGame.LoadProgress();

//        if (YandexGame.savesData.isFirstSession)
//        {
//            // Можно дать стартовые деньги и прочие параметры
//            YandexGame.savesData.isFirstSession = false;
//            // Money = 1500; // пример
//            // SaveProgress();
//        }
//    }

//    #region Money

//    public bool TrySpendMoney(int amount)
//    {
//        if (amount < 0)
//        {
//            Debug.LogError("[SaveManager] Нельзя списывать отрицательную сумму!");
//            return false;
//        }

//        if (Money >= amount)
//        {
//            Money -= amount;
//            return true;
//        }
//        return false;
//    }

//    public void AddMoney(int amount)
//    {
//        if (amount < 0)
//        {
//            Debug.LogError("[SaveManager] Нельзя добавлять отрицательную сумму!");
//            return;
//        }

//        Money += amount;
//    }

//    #endregion

//    #region Cars

//    public void AddCar(int carId)
//    {
//        if (!PurchasedCarIDs.Contains(carId))
//        {
//            PurchasedCarIDs.Add(carId);
//        }
//    }

//    public bool HasCar(int carId)
//    {
//        return PurchasedCarIDs.Contains(carId);
//    }

//    #endregion

//    #region Upgrades

//    public bool HasCarUpgrade(int carId, int upgradeId)
//    {
//        for (int i = 0; i < PurchasedUpgrades.Count; i++)
//        {
//            if (PurchasedUpgrades[i].carId == carId && PurchasedUpgrades[i].upgradeId == upgradeId)
//                return true;
//        }

//        return false;
//    }

//    public void AddCarUpgrade(int carId, int upgradeId)
//    {
//        if (!HasCarUpgrade(carId, upgradeId))
//        {
//            PurchasedUpgrade record = new PurchasedUpgrade { carId = carId, upgradeId = upgradeId };
//            PurchasedUpgrades.Add(record);
//        }
//    }

//    #endregion

//    public void Save()
//    {
//        YandexGame.SaveProgress();
//        Debug.Log("[SaveManager] Сохранение выполнено!");
//    }
//}