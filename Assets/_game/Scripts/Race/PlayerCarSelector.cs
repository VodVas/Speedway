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
        [field: SerializeField] public CarData CarData;
        [field: SerializeField] public GameObject CarObject;

        public int Id => CarData ? CarData.Id : -1;
    }

    [SerializeField] private List<SceneCarReference> _sceneCars = new List<SceneCarReference>();
    [SerializeField] private bool _validateOnStart = true;
    [SerializeField] private UiCarBinder _desktopUICarBinder = null;
    [SerializeField] private UiCarBinder _mobileUICarBinder = null;

    [Inject] private SmoothSliderHealthBarDisplay _desktopHealthBarDisplay;
    [Inject] private SmoothSliderHealthBarDisplay _mobileHealthBarDisplay;
    [Inject] private DriftScoreUIDisplayer _desktopDriftScoreUIDisplayer;
    [Inject] private DriftScoreUIDisplayer _mobileDriftScoreUIDisplayer;
    [Inject] private DeadCarRespawner _deadCarRespawner;

    private GameObject _activeCar;
    private Racer _playerRacer;
    private bool _isMobile;

    public event Action CarActivated;

    private void Start()
    {
        _isMobile = YandexGame.EnvironmentData.isMobile;

        if (_validateOnStart && !ValidateCarSetup())
        {
            enabled = false;
            return;
        }

        InitializeCarSystem();
    }

    public Racer GetPlayerRacer() => _playerRacer;

    private bool ValidateCarSetup()
    {
        if (_sceneCars.Count == 0)
        {
            Debug.LogError("No cars assigned in Scene Cars list!", this);
            return false;
        }

        var ids = new HashSet<int>();
        bool isValid = true;

        foreach (var carRef in _sceneCars)
        {
            if (carRef.CarObject == null)
            {
                Debug.LogError("Missing car object reference!", this);
                isValid = false;
            }

            if (carRef.CarData == null)
            {
                Debug.LogError($"Car object {carRef.CarObject?.name} missing CarData component!", this);
                isValid = false;
            }

            if (carRef.CarObject != null && !CheckRequiredComponents(carRef.CarObject))
            {
                isValid = false;
            }

            if (carRef.CarData != null && !ids.Add(carRef.Id))
            {
                Debug.LogError($"Duplicate car ID {carRef.Id} detected!", this);
                isValid = false;
            }
        }

        if (_desktopHealthBarDisplay == null || _mobileHealthBarDisplay == null)
        {
            Debug.LogError("HealthBarDisplay components not assigned!", this);
            isValid = false;
        }

        if (_desktopDriftScoreUIDisplayer == null || _mobileDriftScoreUIDisplayer == null)
        {
            Debug.LogError("DriftScoreUIDisplayer components not assigned!", this);
            isValid = false;
        }

        return isValid;
    }

    private bool CheckRequiredComponents(GameObject carObject)
    {
        bool isValid = true;
        var components = new[]
        {
            typeof(Racer),
            typeof(Health),
            typeof(ArcadeVehicleController),
            typeof(Rigidbody)
        };

        foreach (var type in components)
        {
            if (carObject.GetComponent(type) == null)
            {
                Debug.LogError($"Car {carObject.name} missing {type.Name} component!", this);
                isValid = false;
            }
        }

        return isValid;
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
            if (carRef.CarObject != null)
            {
                carRef.CarObject.SetActive(false);
            }
        }
    }

    private void ActivateSelectedCar()
    {
        int targetId = YandexGame.savesData.GetLastUsedCarId();
        SceneCarReference carRef = FindCarById(targetId) ?? _sceneCars[0];

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
        if (_activeCar == null)
        {
            Debug.LogWarning("[PlayerCarSelector] No active car selected, skipping component initialization");
            enabled = false;
            return;


        }

        InitializeUpgradeSystems();
        SetupPlatformSpecificUI();
        RegisterRespawnSystem();
    }

    private void InitializeUpgradeSystems()
    {
        if (_activeCar == null) return;

        var upgrades = _activeCar.GetComponent<CarUpgrades>();
        var modifications = _activeCar.GetComponent<CarModifications>();
        var vehicleController = _activeCar.GetComponent<ArcadeVehicleController>();
        var health = _activeCar.GetComponent<Health>();

        if (upgrades != null)
        {
            upgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
            upgrades.ApplyPurchasedStats(
                YandexGame.savesData.HasCarUpgrade,
                vehicleController,
                health
            );
        }

        if (modifications != null)
        {
            modifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
            modifications.ApplyPurchasedMods(
                YandexGame.savesData.GetCarModificationCount,
                vehicleController,
                health
            );
        }
    }

    private void SetupPlatformSpecificUI()
    {
        var health = _activeCar.GetComponent<Health>();
        var rb = _activeCar.GetComponent<Rigidbody>();
        var vehicle = _activeCar.GetComponent<ArcadeVehicleController>();

        if (_isMobile)
        {
            _mobileHealthBarDisplay.Initialize(health);
            _mobileDriftScoreUIDisplayer.SetPlayerCar(vehicle);
            _mobileUICarBinder.BindPlayerCar(rb, health, _activeCar.transform);
        }
        else
        {
            _desktopHealthBarDisplay.Initialize(health);
            _desktopDriftScoreUIDisplayer.SetPlayerCar(vehicle);
            _desktopUICarBinder.BindPlayerCar(rb, health, _activeCar.transform);
        }
    }

    private void RegisterRespawnSystem()
    {
        if (_activeCar.TryGetComponent(out Vehicle vehicle))
        {
            _deadCarRespawner.AddVehicle(vehicle);
        }
    }
}






//public class PlayerCarSelector : MonoBehaviour
//{
//    [Serializable]
//    private class SceneCarReference
//    {
//        [field: SerializeField] public CarData CarData;
//        [field: SerializeField] public GameObject CarObject;

//        public int Id => CarData ? CarData.Id : -1;
//        // public CarData CarData => _carData;
//        // public GameObject CarObject => _carObject;
//    }

//    [SerializeField] private List<SceneCarReference> _sceneCars = new List<SceneCarReference>();
//    [SerializeField] private bool _validateOnStart = true;
//    [SerializeField] private UiCarBinder _desktopUICarBinder = null;
//    [SerializeField] private UiCarBinder _mobileUICarBinder = null;

//    [Inject] private SmoothSliderHealthBarDisplay _desktopHealthBarDisplay;
//    [Inject] private SmoothSliderHealthBarDisplay _mobileHealthBarDisplay;
//    [Inject] private DriftScoreUIDisplayer _desktopDriftScoreUIDisplayer;
//    [Inject] private DriftScoreUIDisplayer _mobileDriftScoreUIDisplayer;
//    [Inject] private DeadCarRespawner _deadCarRespawner;

//    private GameObject _activeCar;
//    private Racer _playerRacer;

//    public event Action CarActivated;

//    private void Start()
//    {
//        if (_validateOnStart)
//        {
//            if (!ValidateCarSetup())
//            {
//                enabled = false;
//                return;
//            }
//        }

//        InitializeCarSystem();
//    }

//    public Racer GetPlayerRacer() => _playerRacer;

//    private bool ValidateCarSetup()
//    {
//        if (_sceneCars.Count == 0)
//        {
//            Debug.LogError("No cars assigned in Scene Cars list!");
//            return false;
//        }

//        var ids = new HashSet<int>();

//        foreach (var carRef in _sceneCars)
//        {
//            if (carRef.CarObject == null)
//            {
//                Debug.LogError("Missing car object reference!");
//                return false;
//            }

//            if (carRef.CarData == null)
//            {
//                Debug.LogError($"Car object {carRef.CarObject.name} missing CarData component!");
//                return false;
//            }

//            if (!ids.Add(carRef.Id))
//            {
//                Debug.LogError($"Duplicate car ID {carRef.Id} detected!");
//                return false;
//            }

//            if (!CheckRequiredComponents(carRef.CarObject))
//            {
//                return false;
//            }
//        }

//        return true;
//    }

//    private bool CheckRequiredComponents(GameObject carObject)
//    {
//        var required = new Type[]
//        {
//            typeof(Racer),
//            typeof(Health),
//            typeof(ArcadeVehicleController),
//            typeof(Rigidbody)
//        };

//        for (int i = 0; i < required.Length; i++)
//        {
//            if (carObject.GetComponent(required[i]) == null)
//            {
//                Debug.LogError($"Car {carObject.name} missing {required[i].Name} component!");
//                return false;
//            }
//        }

//        return true;
//    }

//    private void InitializeCarSystem()
//    {
//        DeactivateAllCars();
//        ActivateSelectedCar();
//        ProcessCarComponents();

//        CarActivated?.Invoke();
//    }

//    private void DeactivateAllCars()
//    {
//        foreach (var carRef in _sceneCars)
//        {
//            carRef.CarObject.SetActive(false);
//        }
//    }

//    private void ActivateSelectedCar()
//    {
//        int targetId = YandexGame.savesData.GetLastUsedCarId();
//        SceneCarReference carRef = FindCarById(targetId);

//        if (carRef == null)
//        {
//            carRef = _sceneCars[0];
//        }

//        _activeCar = carRef.CarObject;
//        _activeCar.SetActive(true);

//        _playerRacer = _activeCar.GetComponent<Racer>();
//    }

//    private SceneCarReference FindCarById(int carId)
//    {
//        foreach (var carRef in _sceneCars)
//        {
//            if (carRef.Id == carId)
//                return carRef;
//        }
//        return null;
//    }

//    private void ProcessCarComponents()
//    {
//        InitializeUpgradeSystems();
//        SetupUIComponents();
//        RegisterRespawnSystem();
//    }

//    private void InitializeUpgradeSystems()
//    {
//        var upgrades = _activeCar.GetComponent<CarUpgrades>();
//        var modifications = _activeCar.GetComponent<CarModifications>();

//        if (upgrades != null)
//        {
//            upgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
//            upgrades.ApplyPurchasedStats(
//                YandexGame.savesData.HasCarUpgrade,
//                _activeCar.GetComponent<ArcadeVehicleController>(),
//                _activeCar.GetComponent<Health>()
//            );
//        }

//        if (modifications != null)
//        {
//            modifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
//            modifications.ApplyPurchasedMods(
//                YandexGame.savesData.GetCarModificationCount,
//                _activeCar.GetComponent<ArcadeVehicleController>(),
//                _activeCar.GetComponent<Health>()
//            );
//        }
//    }

//    private void SetupUIComponents()
//    {
//        var health = _activeCar.GetComponent<Health>();
//        var rb = _activeCar.GetComponent<Rigidbody>();
//        var vehicle = _activeCar.GetComponent<ArcadeVehicleController>();

//        _desktopHealthBarDisplay.Initialize(health);
//        _desktopDriftScoreUIDisplayer.SetPlayerCar(vehicle);

//        if (YandexGame.EnvironmentData.isDesktop)
//        {
//            _desktopUICarBinder.BindPlayerCar(rb, health, _activeCar.transform);
//        }
//        else if (YandexGame.EnvironmentData.isDesktop)
//        {
//            _mobileUICarBinder.BindPlayerCar(rb, health, _activeCar.transform);
//        }
//    }

//    private void RegisterRespawnSystem()
//    {
//        if (_activeCar.TryGetComponent(out Vehicle vehicle))
//        {
//            _deadCarRespawner.AddVehicle(vehicle);
//        }
//    }
//}