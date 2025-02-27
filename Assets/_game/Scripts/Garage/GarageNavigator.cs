using UnityEngine;
using YG;
using System.Collections.Generic;
using System;

public class GarageNavigator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _carsInScene;

    private GarageCarSelectionCycler _selectionCycler;

    public event Action OnGarageReady;

    private void Start()
    {

        if (_carsInScene == null || _carsInScene.Count == 0)
        {
            Debug.LogWarning("[GarageNavigator] Список машин в сцене пуст или не задан!");
            enabled = false;
            return;
        }

        _selectionCycler = new GarageCarSelectionCycler(_carsInScene);

        int lastUsedCarId = YandexGame.savesData.GetLastUsedCarId();
        int foundIndex = FindCarIndexById(lastUsedCarId);

        if (foundIndex != -1)
        {
            SetCyclerIndex(foundIndex);
        }
        else
        {
            SetCyclerIndex(0);
        }

        InitializeCarUpgradesAndMods();

        OnGarageReady?.Invoke();
    }

    public void NextCar()
    {
        if (_selectionCycler == null || _carsInScene.Count <= 1)
            return;

        _selectionCycler.SwitchCar(1);
        OnCarChanged();
    }

    public void PrevCar()
    {
        if (_selectionCycler == null || _carsInScene.Count <= 1)
            return;

        _selectionCycler.SwitchCar(-1);
        OnCarChanged();
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

    private void SetCyclerIndex(int index)
    {
        if (_selectionCycler == null)
            return;

        _selectionCycler.SetCarActive(false);
        _selectionCycler.SetCurrentIndex(index);
        _selectionCycler.SetCarActive(true);
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

    private int FindCarIndexById(int carId)
    {
        for (int i = 0; i < _carsInScene.Count; i++)
        {
            GameObject carObj = _carsInScene[i];
            if (carObj == null)
                continue;

            CarData data = carObj.GetComponent<CarData>();
            if (data != null && data.Id == carId)
            {
                return i;
            }
        }

        return -1;
    }
}