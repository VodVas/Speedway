using UnityEngine;
using Zenject;
using System.Collections.Generic;
using ArcadeVP;

public class RaceCarSelector : MonoBehaviour
{
    [SerializeField] private List<RaceCarItem> _allCarsInRace;

    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
    [Inject] private UiCarBinder _uiCarBinder = null;
    [Inject] private SaveService _saveManager;
    private Racer _playerRacer;

    private void Start()
    {
        if (_allCarsInRace == null || _allCarsInRace.Count == 0)
        {
            Debug.LogWarning("[RaceCarSelector] Список машин пуст!");
            return;
        }

        DeactivateAllCars();
        ActivateLastUsedCar();
    }

    public Racer GetPlayerRacer()
    {
        return _playerRacer;
    }

    public IReadOnlyList<RaceCarItem> GetAllCars()
    {
        return _allCarsInRace.AsReadOnly();
    }

    private void DeactivateAllCars()
    {
        for (int i = 0; i < _allCarsInRace.Count; i++)
        {
            if (_allCarsInRace[i] != null && _allCarsInRace[i].carObject != null)
            {
                _allCarsInRace[i].carObject.SetActive(false);
            }
        }
    }

    private void ActivateLastUsedCar()
    {
        int lastCarId = _saveManager.LastUsedCarId;

        if (lastCarId < 0)
        {
            Debug.LogWarning("[RaceCarSelector] LastUsedCarId не задан, включаем первую машину по умолчанию.");
            ActivateCar(0);
            return;
        }

        bool foundCar = false;

        for (int i = 0; i < _allCarsInRace.Count; i++)
        {
            RaceCarItem item = _allCarsInRace[i];

            if (item == null || item.carObject == null)
                continue;

            if (item.carId == lastCarId)
            {
                ActivateCar(i);
                foundCar = true;
                break;
            }
        }

        if (foundCar == false)
        {
            Debug.LogWarning($"[RaceCarSelector] Машина с id={lastCarId} не найдена в списке!");
        }
    }

    private void ActivateCar(int index)
    {
        RaceCarItem item = _allCarsInRace[index];
        item.carObject.SetActive(true);

        if (item.carObject.TryGetComponent(out Health playerHealth) == false)
        {
            Debug.LogError("[RaceCarSelector] Health component not found on the car object!");
            return;
        }

        if (_healthBarDisplay != null)
        {
            _healthBarDisplay.Initialize(playerHealth);
        }
        else
        {
            Debug.LogError("[RaceCarSelector] HealthBarDisplay is not assigned!");
        }

        if (item.carUpgrades != null)
        {
            item.carUpgrades.InitializePurchasedUpgrades(_saveManager.HasCarUpgrade);
            item.carUpgrades.ApplyPurchasedStats(
                _saveManager.HasCarUpgrade,
                item.carObject.GetComponent<ArcadeVehicleController>(),
                item.carObject.GetComponent<Health>()
            );
        }

        if (item.carModifications != null)
        {
            item.carModifications.InitializePurchasedMods(_saveManager.GetCarModificationCount);
            item.carModifications.ApplyPurchasedMods(
                _saveManager.GetCarModificationCount,
                item.carObject.GetComponent<ArcadeVehicleController>(),
                item.carObject.GetComponent<Health>()
            );
        }

        _playerRacer = item.carObject.GetComponent<Racer>();

        if (_uiCarBinder != null)
        {
            var rigidbody = item.carObject.GetComponent<Rigidbody>();
            var health = item.carObject.GetComponent<Health>();
            var carTransform = item.carObject.transform;

            _uiCarBinder.BindPlayerCar(rigidbody, health, carTransform);
        }
    }
}