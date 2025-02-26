using UnityEngine;
using ArcadeVP;
using YG;
using System;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;

public class RaceCarSelector : MonoBehaviour
{
    [SerializeField] private UiCarBinder _uiCarBinder = null;
    [SerializeField] private Transform _carPosition;

    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
    [Inject] private DriftScoreUIDisplayer _driftScoreUIDisplayer;
    [Inject] private Container _container;

    private Racer _playerRacer;
    private GameObject _currentCarInstance;

    private void Start()
    {
        ActivateLastUsedCar();
    }

    public Racer GetPlayerRacer()
    {
        return _playerRacer;
    }

    private void ActivateLastUsedCar()
    {
        int lastCarId = YandexGame.savesData.GetLastUsedCarId();
        GameObject carPrefab = LoadCarPrefab(lastCarId);
        ActivateCar(carPrefab);
    }

    private GameObject LoadCarPrefab(int carId)
    {
        if (carId < 0)
        {
            Debug.LogWarning("[RaceCarSelector] Invalid car ID, using default Car_1");
            carId = 1;
        }

        string resourcePath = $"Cars/Player/Car_{carId}";
        GameObject prefab = Resources.Load<GameObject>(resourcePath);

        if (prefab == null)
        {
            Debug.LogWarning($"[RaceCarSelector] Car_{carId} not found at {resourcePath}, loading default");
            prefab = Resources.Load<GameObject>("Cars/Player/Car_1");

            if (prefab == null)
            {
                Debug.LogError("[RaceCarSelector] Default car Car_1 not found in Resources!");
                return null;
            }
        }

        return prefab;
    }

    private void ActivateCar(GameObject carPrefab)
    {
        if (carPrefab == null)
        {
            Debug.LogError("[RaceCarSelector] Cannot activate null car prefab!");
            enabled = false;
            return;
        }

        CleanupExistingCar();

        _currentCarInstance = Instantiate(carPrefab, _carPosition.position, Quaternion.identity);
        _currentCarInstance.SetActive(true);

        var injectables = _currentCarInstance.GetComponentsInChildren<MonoBehaviour>();

        foreach (var component in injectables)
        {
            try
            {
                AttributeInjector.Inject(component, _container);
            }
            catch (Exception e)
            {
                Debug.LogError($"Injection failed for {component.GetType().Name}: {e.Message}");
            }
        }



        InitializeVehicleSystems(_currentCarInstance);
        InitializeCarCustomizations(_currentCarInstance);
        BindUiComponents(_currentCarInstance);

        _playerRacer = _currentCarInstance.GetComponent<Racer>();
    }

    private void CleanupExistingCar()
    {
        if (_currentCarInstance != null)
        {
            Destroy(_currentCarInstance);
            _currentCarInstance = null;
        }
    }

    private void InitializeVehicleSystems(GameObject carInstance)
    {
        if (!carInstance.TryGetComponent(out Health playerHealth))
        {
            Debug.LogError("[RaceCarSelector] Health component missing!");
            enabled = false;
            return;
        }

        if (!carInstance.TryGetComponent(out ArcadeVehicleController _))
        {
            Debug.LogError("[RaceCarSelector] ArcadeVehicleController missing!");
            enabled = false;
            return;
        }

        if (_healthBarDisplay != null)
        {
            _healthBarDisplay.Initialize(playerHealth);
        }
        else
        {
            Debug.LogError("[RaceCarSelector] HealthBar Display reference not set!");
            enabled = false;
            return;
        }
    }

    private void InitializeCarCustomizations(GameObject carInstance)
    {
        CarUpgrades carUpgrades = carInstance.GetComponent<CarUpgrades>();
        if (carUpgrades != null)
        {
            carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
            carUpgrades.ApplyPurchasedStats(
                YandexGame.savesData.HasCarUpgrade,
                carInstance.GetComponent<ArcadeVehicleController>(),
                carInstance.GetComponent<Health>()
            );
        }

        CarModifications carMods = carInstance.GetComponent<CarModifications>();
        if (carMods != null)
        {
            carMods.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
            carMods.ApplyPurchasedMods(
                YandexGame.savesData.GetCarModificationCount,
                carInstance.GetComponent<ArcadeVehicleController>(),
                carInstance.GetComponent<Health>()
            );
        }
    }

    private void BindUiComponents(GameObject carInstance)
    {
        if (_uiCarBinder != null)
        {
            Rigidbody rb = carInstance.GetComponent<Rigidbody>();
            Health health = carInstance.GetComponent<Health>();
            Transform carTransform = carInstance.transform;

            if (rb != null && health != null && carTransform != null)
            {
                _uiCarBinder.BindPlayerCar(rb, health, carTransform);
            }
        }

        if (carInstance.TryGetComponent(out ArcadeVehicleController driftController)
            && _driftScoreUIDisplayer != null)
        {
            _driftScoreUIDisplayer.SetPlayerCar(driftController);
        }
    }
}