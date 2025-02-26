using UnityEngine;
using YG;
using System.Collections;
using System;

[RequireComponent(typeof(GarageCarInstancePool))]
public class GarageNavigator : MonoBehaviour
{
    private GarageCarInstancePool _carPool;
    private GarageCarSelectionCycler _selectionCycler;

    public event Action OnGarageReady;

    private IEnumerator Start()
    {
        _carPool = GetComponent<GarageCarInstancePool>();

        if (_carPool == null)
        {
            Debug.LogError("[GarageNavigator] Не найден компонент GarageCarInstancePool!");
            enabled = false;
            yield break;
        }

        yield return StartCoroutine(_carPool.SpawnPurchasedCars());

        if (_carPool.SpawnedCars.Count == 0)
        {
            Debug.LogWarning("[GarageNavigator] Нет купленных машин для отображения в гараже.");
            enabled = false;
            yield break;
        }

        _selectionCycler = new GarageCarSelectionCycler(_carPool);

        int lastUsedCarId = YandexGame.savesData.GetLastUsedCarId();
        int foundIndex = FindCarIndexById(lastUsedCarId);

        if (foundIndex != -1)
        {
            SetCyclerIndex(foundIndex);
        }
        else
        {
            if (_carPool.SpawnedCars.Count > 0)
            {
                SetCyclerIndex(0);
            }
        }

        InitializeCarUpgradesAndMods();

        OnGarageReady?.Invoke();
    }

    public void NextCar()
    {
        if (_selectionCycler == null || _carPool.SpawnedCars.Count <= 1)
            return;

        _selectionCycler.SwitchCar(1);
        OnCarChanged();
    }

    public void PrevCar()
    {
        if (_selectionCycler == null || _carPool.SpawnedCars.Count <= 1)
            return;

        _selectionCycler.SwitchCar(-1);
        OnCarChanged();
    }

    public CarModifications GetCurrentCarModifications()
    {
        if (_selectionCycler == null)
            return null;

        CarData data = _selectionCycler.GetCurrentCarData();
        if (data == null)
            return null;

        return data.GetComponent<CarModifications>();
    }

    public CarUpgrades GetCurrentCarUpgrades()
    {
        if (_selectionCycler == null)
            return null;

        CarData data = _selectionCycler.GetCurrentCarData();
        if (data == null)
            return null;

        return data.GetComponent<CarUpgrades>();
    }

    private void SetCyclerIndex(int index)
    {
        _selectionCycler.SetCarActive(false);
        _selectionCycler.SetCurrentIndex(index);
        _selectionCycler.SetCarActive(true);
    }

    private void OnCarChanged()
    {
        CarData data = _selectionCycler.GetCurrentCarData();

        if (data != null)
        {
            YandexGame.savesData.SetLastUsedCarId(data.Id);
            YandexGame.SaveProgress();
            InitializeCarUpgradesAndMods();
        }
    }

    private void InitializeCarUpgradesAndMods()
    {
        CarUpgrades upgrades = GetCurrentCarUpgrades();

        if (upgrades != null)
        {
            upgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
        }

        CarModifications modifications = GetCurrentCarModifications();

        if (modifications != null)
        {
            modifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
        }
    }

    private int FindCarIndexById(int carId)
    {
        var spawnedCars = _carPool.SpawnedCars;

        for (int i = 0; i < spawnedCars.Count; i++)
        {
            CarData data = spawnedCars[i].GetComponent<CarData>();
            if (data != null && data.Id == carId)
            {
                return i;
            }
        }

        return -1;
    }
}