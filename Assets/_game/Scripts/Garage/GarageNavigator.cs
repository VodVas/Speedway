using UnityEngine;
using YG;
using System.Collections;


[RequireComponent(typeof(GarageCarInstancePool))]
public class GarageNavigator : MonoBehaviour
{
    private GarageCarInstancePool _carPool;
    private GarageCarSelectionCycler _selectionCycler;

    private IEnumerator Start()
    {
        _carPool = GetComponent<GarageCarInstancePool>();

        if (_carPool == null)
        {
            Debug.LogError("[GarageNavigator] Не найден компонент GarageCarInstancePool!");
            yield break;
        }

        yield return StartCoroutine(_carPool.SpawnPurchasedCars());

        if (_carPool.SpawnedCars.Count == 0)
        {
            Debug.LogWarning("[GarageNavigator] Нет купленных машин для отображения в гараже.");
            yield break;
        }

        _selectionCycler = new GarageCarSelectionCycler(_carPool);

        int lastUsedCarId = YandexGame.savesData.GetLastUsedCarId();
        int foundIndex = FindCarIndexById(lastUsedCarId);

        if (foundIndex != -1)
        {
            _selectionCycler.SetCarActive(false);
            SetCyclerIndex(foundIndex);
        }

        if (_selectionCycler.CurrentIndex == -1 && _carPool.SpawnedCars.Count > 0)
        {
            SetCyclerIndex(0);
        }

        _selectionCycler.SetCarActive(true);

        InitializeCarUpgradesAndMods();
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

        typeof(GarageCarSelectionCycler)
            .GetProperty(nameof(GarageCarSelectionCycler.CurrentIndex)) //----------------------------------------
            ?.SetValue(_selectionCycler, index);

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











//public class GarageNavigator : MonoBehaviour
//{
//    [SerializeField] private List<GarageCarItem> _garageCars;

//    private int _currentIndex = -1;

//    private void Start()
//    {
//        foreach (var car in _garageCars)
//        {
//            if (car.carObject != null)
//                car.carObject.SetActive(false);
//        }

//        _currentIndex = FindFirstPurchasedCarIndex();

//        if (_currentIndex >= 0)
//        {
//            ShowCar(_currentIndex);
//        }
//        else
//        {
//            Debug.LogWarning("Нет купленных машин.");
//        }
//    }

//    private int FindFirstPurchasedCarIndex()
//    {
//        for (int i = 0; i < _garageCars.Count; i++)
//        {
//            if (YandexGame.savesData.HasCar(_garageCars[i].carId))
//            {
//                return i;
//            }
//        }

//        return -1;
//    }

//    private void ShowCar(int index)
//    {
//        for (int i = 0; i < _garageCars.Count; i++)
//        {
//            if (_garageCars[i].carObject != null)
//                _garageCars[i].carObject.SetActive(false);
//        }

//        var carItem = _garageCars[index];

//        if (carItem.carObject != null)
//        {
//            carItem.carObject.SetActive(true);

//            if (carItem.carUpgrades != null)
//            {
//                carItem.carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
//            }

//            if (carItem.carModifications != null)
//            {
//                carItem.carModifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
//            }

//            SetLastUsedCarId(carItem.carId);
//        }
//    }

//    public void NextCar()
//    {
//        if (_currentIndex < 0) return;

//        do
//        {
//            _currentIndex = (_currentIndex + 1) % _garageCars.Count;
//        }
//        while (!YandexGame.savesData.HasCar(_garageCars[_currentIndex].carId));

//        ShowCar(_currentIndex);
//    }

//    public void PrevCar()
//    {
//        if (_currentIndex < 0) return;

//        do
//        {
//            _currentIndex = (_currentIndex - 1 + _garageCars.Count) % _garageCars.Count;
//        } 
//        while (!YandexGame.savesData.HasCar(_garageCars[_currentIndex].carId));

//        ShowCar(_currentIndex);
//    }

//    public CarModifications GetCurrentCarModifications()
//    {
//        if (_currentIndex < 0 || _currentIndex >= _garageCars.Count)
//        {
//            return null;
//        }
//        return _garageCars[_currentIndex].carModifications;
//    }

//    public CarUpgrades GetCurrentCarUpgrades()
//    {
//        if (_currentIndex < 0 || _currentIndex >= _garageCars.Count)
//            return null;

//        return _garageCars[_currentIndex].carUpgrades;
//    }

//    private void SetLastUsedCarId(int carId)
//    {
//        YandexGame.savesData.SetLastUsedCarId(carId);
//        YandexGame.SaveProgress();
//    }
//}