using UnityEngine;
using YG;
using System.Collections.Generic;
using System;


public class GarageNavigator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _carsInScene;

    private GarageCarSelectionCycler _selectionCycler;
    private List<GameObject> _purchasedCars;
    private CarData _cachedCarData;
    private SavesYG _savesData;

    public event Action OnGarageReady;
    public event Action OnCarChanged;

    public int CurrentCarID => _selectionCycler?.GetCurrentCarData()?.Id ?? -1;

    private void Start()
    {
        InitializeGarageSystem();
    }

    private void InitializeGarageSystem()
    {
        if (!ValidateCarsList()) return;

        CacheReferences();
        FilterPurchasedCars();

        if (!CheckPurchasedCarsExist()) return;

        InitializeSelectionCycler();
        HandleLastUsedCar();
        InitializeCarSystems();
        NotifyGarageReady();
    }

    private bool ValidateCarsList()
    {
        if (_carsInScene != null && _carsInScene.Count > 0) return true;

        Debug.LogWarning("[GarageNavigator] Cars list is empty or not set!");
        enabled = false;
        return false;
    }

    private void CacheReferences()
    {
        _savesData = YandexGame.savesData;
        _purchasedCars = new List<GameObject>(_carsInScene.Count);
    }

    private void FilterPurchasedCars()
    {
        GameObject currentCar;
        CarData carData;

        for (int i = 0; i < _carsInScene.Count; i++)
        {
            currentCar = _carsInScene[i];
            if (currentCar == null) continue;

            carData = currentCar.GetComponent<CarData>();
            if (carData != null && _savesData.HasCar(carData.Id))
            {
                _purchasedCars.Add(currentCar);
            }
        }
    }

    private bool CheckPurchasedCarsExist()
    {
        if (_purchasedCars.Count > 0) return true;

        Debug.LogError("[GarageNavigator] No purchased cars available!");
        enabled = false;
        return false;
    }

    private void InitializeSelectionCycler()
    {
        _selectionCycler = new GarageCarSelectionCycler(_purchasedCars);
    }

    private void HandleLastUsedCar()
    {
        int lastUsedId = _savesData.GetLastUsedCarId();
        int foundIndex = FindCarIndex(lastUsedId);

        if (foundIndex >= 0)
        {
            SetCyclerIndex(foundIndex);
        }
        else
        {
            SetCyclerIndex(0);
            UpdateLastUsedCarId();
        }
    }

    private int FindCarIndex(int carId)
    {
        GameObject currentCar;
        CarData carData;

        for (int i = 0; i < _purchasedCars.Count; i++)
        {
            currentCar = _purchasedCars[i];
            if (currentCar == null) continue;

            carData = currentCar.GetComponent<CarData>();
            if (carData != null && carData.Id == carId)
            {
                return i;
            }
        }
        return -1;
    }

    private void UpdateLastUsedCarId()
    {
        if (_purchasedCars.Count == 0 || _selectionCycler == null) return;

        _cachedCarData = _selectionCycler.GetCurrentCarData();
        if (_cachedCarData == null) return;

        _savesData.SetLastUsedCarId(_cachedCarData.Id);
        YandexGame.SaveProgress();
    }

    private void InitializeCarSystems()
    {
        InitializeUpgrades();
        InitializeModifications();
    }

    private void InitializeUpgrades()
    {
        CarUpgrades upgrades = GetCurrentCarUpgrades();
        if (upgrades != null)
        {
            upgrades.InitializePurchasedUpgrades(_savesData.HasCarUpgrade);
        }
    }

    private void InitializeModifications()
    {
        CarModifications modifications = GetCurrentCarModifications();
        if (modifications != null)
        {
            modifications.InitializePurchasedMods(_savesData.GetCarModificationCount);
        }
    }

    private void NotifyGarageReady()
    {
        OnGarageReady?.Invoke();
    }

    public void NextCar()
    {
        if (_selectionCycler == null || _purchasedCars.Count <= 1) return;

        SwitchCar(1);
        UpdateCarSystems();
    }

    public void PrevCar()
    {
        if (_selectionCycler == null || _purchasedCars.Count <= 1) return;

        SwitchCar(-1);
        UpdateCarSystems();
    }

    private void SwitchCar(int direction)
    {
        _selectionCycler.SwitchCar(direction);
        UpdateLastUsedCarId();
        OnCarChanged?.Invoke();
    }

    private void UpdateCarSystems()
    {
        InitializeCarSystems();
    }

    private void SetCyclerIndex(int index)
    {
        if (_selectionCycler == null) return;

        _selectionCycler.SetCarActive(false);
        _selectionCycler.SetCurrentIndex(index);
        _selectionCycler.SetCarActive(true);
    }

    public CarModifications GetCurrentCarModifications()
    {
        return _selectionCycler?.GetCurrentCarData()?.GetComponent<CarModifications>();
    }

    public CarUpgrades GetCurrentCarUpgrades()
    {
        return _selectionCycler?.GetCurrentCarData()?.GetComponent<CarUpgrades>();
    }
}