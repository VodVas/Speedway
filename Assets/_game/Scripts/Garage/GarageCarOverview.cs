using System.Collections.Generic;
using UnityEngine;
using Zenject;

//public class GarageCarOverview : MonoBehaviour
//{
//    [SerializeField] private List<GarageCarItem> _garageCars = null;

//    [Inject] private SaveManager _saveManager = null;

//    private CarShowcase _carShowcase = null;

//    public CarUpgrades CurrentCarUpgrades => _carShowcase?.GetCurrentCarUpgrades();

//    private void Awake()
//    {
//        if (_garageCars == null)
//        {
//            Debug.LogError("GarageCarOverview: список _garageCars не назначен.", this);
//            enabled = false;
//            return;
//        }

//        if (_garageCars.Count == 0)
//        {
//            Debug.LogError("GarageCarOverview: список _garageCars пуст.", this);
//            enabled = false;
//            return;
//        }

//        if (_saveManager == null)
//        {
//            Debug.LogError("GarageCarOverview: _saveManager не назначен.", this);
//            enabled = false;
//            return;
//        }

//        HideAllCars();
//    }

//    private void Start()
//    {
//        _carShowcase = new CarShowcase(_garageCars, _saveManager);

//        int firstIndex = _carShowcase.FindFirstPurchasedCarIndex();

//        if (firstIndex < 0)
//        {
//            Debug.LogWarning("GarageCarOverview: нет купленных машин.", this);
//            return;
//        }

//        _carShowcase.SetCurrentIndex(firstIndex);

//        ShowCar(firstIndex);
//    }

//    public void ShowNextPurchasedCar()
//    {
//        if (_carShowcase == null)
//            return;

//        int newIndex = _carShowcase.MoveToNextPurchased();

//        if (newIndex >= 0)
//        {
//            ShowCar(newIndex);
//        }
//        else
//        {
//            Debug.LogWarning("GarageCarOverview: нет купленных машин для переключения.", this);
//        }
//    }

//    public void ShowPreviousPurchasedCar()
//    {
//        if (_carShowcase == null)
//            return;

//        int newIndex = _carShowcase.MoveToPreviousPurchased();
//        if (newIndex >= 0)
//        {
//            ShowCar(newIndex);
//        }
//        else
//        {
//            Debug.LogWarning("GarageCarOverview: нет купленных машин для переключения.", this);
//        }
//    }

//    public CarUpgrades RetrieveCurrentCarUpgrades()
//    {
//        return CurrentCarUpgrades;
//    }

//    private void ShowCar(int index)
//    {
//        HideAllCars();

//        GarageCarItem carItem = _carShowcase.GetCurrentCarItem();

//        if (carItem == null)
//        {
//            Debug.LogError($"ShowCar: Не удалось получить машину по индексу {index}.", this);
//            return;
//        }

//        if (carItem.carObject != null)
//        {
//            carItem.carObject.SetActive(true);
//        }

//        if (carItem.carUpgrades != null)
//        {
//            carItem.carUpgrades.InitializePurchasedUpgrades(_saveManager.HasCarUpgrade);
//        }

//        _carShowcase.SaveLastUsedCarId(carItem.carId);
//    }

//    private void HideAllCars()
//    {
//        for (int i = 0; i < _garageCars.Count; i++)
//        {
//            if (_garageCars[i].carObject != null)
//            {
//                _garageCars[i].carObject.SetActive(false);
//            }
//        }
//    }
//}