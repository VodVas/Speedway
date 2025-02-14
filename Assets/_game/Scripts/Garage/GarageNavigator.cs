using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class GarageNavigator : MonoBehaviour
{
    [SerializeField] private List<GarageCarItem> _garageCars;

    [Inject] private SaveService _saveManager;
    private int _currentIndex = -1;

    private void Start()
    {
        foreach (var car in _garageCars)
        {
            if (car.carObject != null)
                car.carObject.SetActive(false);
        }

        _currentIndex = FindFirstPurchasedCarIndex();

        if (_currentIndex >= 0)
        {
            ShowCar(_currentIndex);
        }
        else
        {
            Debug.LogWarning("Нет купленных машин.");
        }
    }

    private int FindFirstPurchasedCarIndex()
    {
        for (int i = 0; i < _garageCars.Count; i++)
        {
            if (_saveManager.HasCar(_garageCars[i].carId))
            {
                return i;
            }
        }

        return -1;
    }

    private void ShowCar(int index)
    {
        for (int i = 0; i < _garageCars.Count; i++)
        {
            if (_garageCars[i].carObject != null)
                _garageCars[i].carObject.SetActive(false);
        }

        var carItem = _garageCars[index];

        if (carItem.carObject != null)
        {
            carItem.carObject.SetActive(true);

            if (carItem.carUpgrades != null)
            {
                carItem.carUpgrades.InitializePurchasedUpgrades(_saveManager.HasCarUpgrade);
            }

            if (carItem.carModifications != null)
            {
                carItem.carModifications.InitializePurchasedMods(_saveManager.GetCarModificationCount);
            }

            SetLastUsedCarId(carItem.carId);
        }

        //if (carItem.carObject != null)
        //{
        //    carItem.carObject.SetActive(true);

        //    if (carItem.carUpgrades != null)
        //    {
        //        carItem.carUpgrades.InitializePurchasedUpgrades(_saveManager.HasCarUpgrade);
        //    }

        //    SetLastUsedCarId(carItem.carId);
        //}
    }

    public void NextCar()
    {
        if (_currentIndex < 0) return;

        do
        {
            _currentIndex = (_currentIndex + 1) % _garageCars.Count;
        }
        while (!_saveManager.HasCar(_garageCars[_currentIndex].carId));

        ShowCar(_currentIndex);
    }

    public void PrevCar()
    {
        if (_currentIndex < 0) return;

        do
        {
            _currentIndex = (_currentIndex - 1 + _garageCars.Count) % _garageCars.Count;
        } 
        while (!_saveManager.HasCar(_garageCars[_currentIndex].carId));

        ShowCar(_currentIndex);
    }

    public CarModifications GetCurrentCarModifications()
    {
        if (_currentIndex < 0 || _currentIndex >= _garageCars.Count)
        {
            return null;
        }
        return _garageCars[_currentIndex].carModifications;
    }

    public CarUpgrades GetCurrentCarUpgrades()
    {
        if (_currentIndex < 0 || _currentIndex >= _garageCars.Count)
            return null;

        return _garageCars[_currentIndex].carUpgrades;
    }

    private void SetLastUsedCarId(int carId)
    {
        _saveManager.LastUsedCarId = carId;
        _saveManager.Save(); // TODO: возможно перенести на кнопку перехода на гонку
    }
}