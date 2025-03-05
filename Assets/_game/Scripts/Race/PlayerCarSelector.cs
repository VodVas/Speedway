using UnityEngine;
using ArcadeVP;
using YG;
using System;
using Reflex.Attributes;
using System.Collections.Generic;

public class PlayerCarSelector : MonoBehaviour
{
    [Serializable]
    private class SceneCarReference
    {
        [SerializeField] private CarData _carData;
        [SerializeField] private GameObject _carObject;

        public int Id => _carData ? _carData.Id : -1;
        public CarData Data => _carData;
        public GameObject CarObject => _carObject;
    }

    [Header("Scene Cars")]
    [SerializeField] private List<SceneCarReference> _sceneCars = new List<SceneCarReference>();
    [SerializeField] private bool _validateOnStart = true;
    [SerializeField] private UiCarBinder _uiCarBinder = null;

    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
    [Inject] private DriftScoreUIDisplayer _driftScoreUIDisplayer;
    [Inject] private DeadCarRespawner _deadCarRespawner;

    private GameObject _activeCar;
    private Racer _playerRacer;

    public event Action CarActivated;

    private void Start()
    {
        if (_validateOnStart)
        {
            if (!ValidateCarSetup())
            {
                enabled = false;
                return;
            }
        }

        InitializeCarSystem();
    }

    public Racer GetPlayerRacer() => _playerRacer;

    private bool ValidateCarSetup()
    {
        if (_sceneCars.Count == 0)
        {
            Debug.LogError("No cars assigned in Scene Cars list!");
            return false;
        }

        var ids = new HashSet<int>();

        foreach (var carRef in _sceneCars)
        {
            if (carRef.CarObject == null)
            {
                Debug.LogError("Missing car object reference!");
                return false;
            }

            if (carRef.Data == null)
            {
                Debug.LogError($"Car object {carRef.CarObject.name} missing CarData component!");
                return false;
            }

            if (!ids.Add(carRef.Id))
            {
                Debug.LogError($"Duplicate car ID {carRef.Id} detected!");
                return false;
            }

            if (!CheckRequiredComponents(carRef.CarObject))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckRequiredComponents(GameObject carObject)
    {
        var required = new Type[]
        {
            typeof(Racer),
            typeof(Health),
            typeof(ArcadeVehicleController),
            typeof(Rigidbody)
        };

        for (int i = 0; i < required.Length; i++)
        {
            if (carObject.GetComponent(required[i]) == null)
            {
                Debug.LogError($"Car {carObject.name} missing {required[i].Name} component!");
                return false;
            }
        }

        return true;
    }

    private void InitializeCarSystem()
    {
        DeactivateAllCars();
        ActivateSelectedCar();
        ProcessCarComponents();

        CarActivated?.Invoke();
    }

    private void DeactivateAllCars()
    {
        foreach (var carRef in _sceneCars)
        {
            carRef.CarObject.SetActive(false);
        }
    }

    private void ActivateSelectedCar()
    {
        int targetId = YandexGame.savesData.GetLastUsedCarId();
        SceneCarReference carRef = FindCarById(targetId);

        if (carRef == null)
        {
            carRef = _sceneCars[0];
        }

        _activeCar = carRef.CarObject;
        _activeCar.SetActive(true);

        _playerRacer = _activeCar.GetComponent<Racer>();
    }

    private SceneCarReference FindCarById(int carId)
    {
        foreach (var carRef in _sceneCars)
        {
            if (carRef.Id == carId)
                return carRef;
        }
        return null;
    }

    private void ProcessCarComponents()
    {
        InitializeUpgradeSystems();
        SetupUIComponents();
        RegisterRespawnSystem();
    }

    private void InitializeUpgradeSystems()
    {
        var upgrades = _activeCar.GetComponent<CarUpgrades>();
        var modifications = _activeCar.GetComponent<CarModifications>();

        if (upgrades != null)
        {
            upgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
            upgrades.ApplyPurchasedStats(
                YandexGame.savesData.HasCarUpgrade,
                _activeCar.GetComponent<ArcadeVehicleController>(),
                _activeCar.GetComponent<Health>()
            );
        }

        if (modifications != null)
        {
            modifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
            modifications.ApplyPurchasedMods(
                YandexGame.savesData.GetCarModificationCount,
                _activeCar.GetComponent<ArcadeVehicleController>(),
                _activeCar.GetComponent<Health>()
            );
        }
    }

    private void SetupUIComponents()
    {
        var health = _activeCar.GetComponent<Health>();
        var rb = _activeCar.GetComponent<Rigidbody>();
        var vehicle = _activeCar.GetComponent<ArcadeVehicleController>();

        _healthBarDisplay.Initialize(health);
        _driftScoreUIDisplayer.SetPlayerCar(vehicle);
        _uiCarBinder.BindPlayerCar(rb, health, _activeCar.transform);
    }

    private void RegisterRespawnSystem()
    {
        if (_activeCar.TryGetComponent(out Vehicle vehicle))
        {
            _deadCarRespawner.AddVehicle(vehicle);
        }
    }
}