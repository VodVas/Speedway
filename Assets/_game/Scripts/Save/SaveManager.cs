using UnityEngine;
using YG;
using Zenject;
using System.Collections.Generic;

public class SaveManager : IInitializable
{
    public int Money
    {
        get => YandexGame.savesData.money;
        private set => YandexGame.savesData.money = value;
    }

    public List<int> PurchasedCarIDs => YandexGame.savesData.purchasedCarIDs;

    public SaveManager() { }

    public void Initialize()
    {
        Debug.Log("[SaveManager] Initialize() → Загружаем сохранения из YandexGame");

        YandexGame.LoadProgress();

        if (YandexGame.savesData.isFirstSession)
        {
            YandexGame.savesData.isFirstSession = false;
            //YandexGame.savesData.money = 1500;
            // YandexGame.SaveProgress();
        }
    }

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

    public void Save()
    {
        YandexGame.SaveProgress();
        Debug.Log("[SaveManager] Сохранение завершено!");
    }
}