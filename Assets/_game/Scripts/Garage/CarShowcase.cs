using System.Collections.Generic;
using UnityEngine;

//internal sealed class CarShowcase
//{
//    private readonly List<GarageCarItem> _cars;
//    private readonly SaveManager _saveManager;
//    private int _currentIndex = -1;

//    internal CarShowcase(List<GarageCarItem> cars, SaveManager saveManager)
//    {
//        if (cars == null)
//        {
//            Debug.LogError("CarShowcase: список машин (cars) = null.");
//            throw new System.ArgumentNullException(nameof(cars));
//        }
//        if (cars.Count == 0)
//        {
//            Debug.LogError("CarShowcase: список машин (cars) пуст.");
//            throw new System.ArgumentException("Cars list is empty.");
//        }
//        if (saveManager == null)
//        {
//            Debug.LogError("CarShowcase: saveManager = null.");
//            throw new System.ArgumentNullException(nameof(saveManager));
//        }

//        _cars = cars;
//        _saveManager = saveManager;
//    }

//    internal int CurrentIndex => _currentIndex;

//    internal int FindFirstPurchasedCarIndex()
//    {
//        for (int i = 0; i < _cars.Count; i++)
//        {
//            if (_saveManager.HasCar(_cars[i].carId))
//            {
//                return i;
//            }
//        }
//        return -1;
//    }

//    internal void SetCurrentIndex(int index)
//    {
//        if (index < 0 || index >= _cars.Count)
//        {
//            Debug.LogError($"CarShowcase: некорректный индекс {index} для SetCurrentIndex.");
//            throw new System.ArgumentOutOfRangeException(nameof(index));
//        }
//        _currentIndex = index;
//    }

//    internal int MoveToNextPurchased()
//    {
//        if (_currentIndex < 0)
//            return -1;

//        int newIndex = _currentIndex;

//        do
//        {
//            newIndex = (newIndex + 1) % _cars.Count;
//        }
//        while (!_saveManager.HasCar(_cars[newIndex].carId) && newIndex != _currentIndex);

//        if (!_saveManager.HasCar(_cars[newIndex].carId))
//        {
//            return -1;
//        }

//        _currentIndex = newIndex;

//        return _currentIndex;
//    }

//    internal int MoveToPreviousPurchased()
//    {
//        if (_currentIndex < 0)
//            return -1;

//        int newIndex = _currentIndex;

//        do
//        {
//            newIndex = (newIndex - 1 + _cars.Count) % _cars.Count;
//        }
//        while (!_saveManager.HasCar(_cars[newIndex].carId) && newIndex != _currentIndex);

//        if (!_saveManager.HasCar(_cars[newIndex].carId))
//        {
//            return -1;
//        }

//        _currentIndex = newIndex;

//        return _currentIndex;
//    }

//    internal void SaveLastUsedCarId(int carId)
//    {
//        _saveManager.LastUsedCarId = carId;
//        _saveManager.Save();
//    }

//    internal CarUpgrades GetCurrentCarUpgrades()
//    {
//        if (_currentIndex < 0 || _currentIndex >= _cars.Count)
//        {
//            return null;
//        }
//        return _cars[_currentIndex].carUpgrades;
//    }

//    internal GarageCarItem GetCurrentCarItem()
//    {
//        if (_currentIndex < 0 || _currentIndex >= _cars.Count)
//            return null;

//        return _cars[_currentIndex];
//    }
//}