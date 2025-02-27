using UnityEngine;
using ArcadeVP;
using YG;
using System;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using System.Collections.Generic;

public class PlayerCarSelector : MonoBehaviour
{
    [Serializable]
    public class SceneCarReference
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
    [Inject] private Container _container;

    private GameObject _activeCar;
    private Racer _playerRacer;

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
        var required = new List<Type>
        {
            typeof(Racer),
            typeof(Health),
            typeof(ArcadeVehicleController),
            typeof(Rigidbody)
        };

        foreach (var type in required)
        {
            if (carObject.GetComponent(type) == null)
            {
                Debug.LogError($"Car {carObject.name} missing {type.Name} component!");
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
        var carRef = FindCarById(targetId) ?? _sceneCars[0];

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
       //InjectDependencies(_activeCar);
        InitializeUpgradeSystems();
        SetupUIComponents();
        RegisterRespawnSystem();
    }

    //private void InjectDependencies(GameObject target)
    //{
    //    foreach (var component in target.GetComponentsInChildren<MonoBehaviour>(true))
    //    {
    //        try
    //        {
    //            AttributeInjector.Inject(component, _container);
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.LogError($"Dependency injection failed: {ex.Message}");
    //        }
    //    }
    //}

    private void InitializeUpgradeSystems()
    {
        var upgrades = _activeCar.GetComponent<CarUpgrades>();
        var modifications = _activeCar.GetComponent<CarModifications>();

        upgrades?.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
        modifications?.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
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

    public Racer GetPlayerRacer() => _playerRacer;
}








//public class PlayerCarSelector : MonoBehaviour
//{
//    [SerializeField] private UiCarBinder _uiCarBinder = null;
//    [SerializeField] private List<RaceCarItem> _carsList = new List<RaceCarItem>();

//    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
//    [Inject] private DriftScoreUIDisplayer _driftScoreUIDisplayer;
//    [Inject] private DeadCarRespawner _deadCarRespawner;
//    [Inject] private Container _container;

//    private Racer _playerRacer;
//    private GameObject _currentActiveCar;

//    private void Start()
//    {
//        ValidateCarsList();
//        ActivateLastUsedCar();
//    }

//    public Racer GetPlayerRacer() => _playerRacer;

//    private void ValidateCarsList()
//    {
//        if (_carsList.Count == 0)
//        {
//            Debug.LogError("[RaceCarSelector] Cars list is empty!");
//            enabled = false;
//            return;
//        }

//        foreach (var carItem in _carsList)
//        {
//            if (carItem.carObject == null)
//            {
//                Debug.LogError($"[RaceCarSelector] Car {carItem.carId} has null reference!");
//                enabled = false;
//                return;
//            }
//        }
//    }

//    private void ActivateLastUsedCar()
//    {
//        int lastCarId = YandexGame.savesData.GetLastUsedCarId();
//        RaceCarItem targetCar = FindCarById(lastCarId);

//        if (targetCar == null)
//        {
//            Debug.LogWarning($"[RaceCarSelector] Car {lastCarId} not found, using default");
//            targetCar = _carsList.Count > 0 ? _carsList[0] : null;
//        }

//        if (targetCar != null)
//        {
//            SetActiveCar(targetCar);
//        }
//        else
//        {
//            Debug.LogError("[RaceCarSelector] No valid cars available!");
//            enabled = false;
//        }
//    }

//    private RaceCarItem FindCarById(int carId)
//    {
//        foreach (var carItem in _carsList)
//        {
//            if (carItem.carId == carId)
//                return carItem;
//        }
//        return null;
//    }

//    private void SetActiveCar(RaceCarItem targetCar)
//    {
//        if (targetCar.carObject == null)
//        {
//            Debug.LogError("[RaceCarSelector] Attempting to activate null car object!");
//            enabled = false;
//            return;
//        }

//        DeactivateAllCars();
//        _currentActiveCar = targetCar.carObject;
//        _currentActiveCar.SetActive(true);

//        ProcessCarComponents(_currentActiveCar);
//        _playerRacer = _currentActiveCar.GetComponent<Racer>();

//        RegisterVehicleForRespawn();
//    }

//    private void DeactivateAllCars()
//    {
//        foreach (var carItem in _carsList)
//        {
//            if (carItem.carObject != null)
//                carItem.carObject.SetActive(false);
//        }
//    }

//    private void ProcessCarComponents(GameObject targetCar)
//    {
//        //ProcessInjection(targetCar);
//        InitializeVehicleSystems(targetCar);
//        InitializeCarCustomizations(targetCar);
//        BindUiComponents(targetCar);
//    }

//    private void ProcessInjection(GameObject target)
//    {
//        var injectables = target.GetComponentsInChildren<MonoBehaviour>(true);
//        foreach (var component in injectables)
//        {
//            try
//            {
//                AttributeInjector.Inject(component, _container);
//            }
//            catch (Exception e)
//            {
//                Debug.LogError($"Injection failed for {component.GetType().Name}: {e.Message}");
//            }
//        }
//    }

//    private void InitializeVehicleSystems(GameObject carInstance)
//    {
//        if (!carInstance.TryGetComponent(out Health playerHealth))
//        {
//            Debug.LogError("[RaceCarSelector] Health component missing!");
//            enabled = false;
//            return;
//        }

//        if (!carInstance.TryGetComponent(out ArcadeVehicleController _))
//        {
//            Debug.LogError("[RaceCarSelector] ArcadeVehicleController missing!");
//            enabled = false;
//            return;
//        }

//        _healthBarDisplay?.Initialize(playerHealth);
//    }

//    private void InitializeCarCustomizations(GameObject carInstance)
//    {
//        CarUpgrades carUpgrades = carInstance.GetComponent<CarUpgrades>();
//        if (carUpgrades != null)
//        {
//            carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
//            carUpgrades.ApplyPurchasedStats(
//                YandexGame.savesData.HasCarUpgrade,
//                carInstance.GetComponent<ArcadeVehicleController>(),
//                carInstance.GetComponent<Health>()
//            );
//        }

//        CarModifications carMods = carInstance.GetComponent<CarModifications>();
//        if (carMods != null)
//        {
//            carMods.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
//            carMods.ApplyPurchasedMods(
//                YandexGame.savesData.GetCarModificationCount,
//                carInstance.GetComponent<ArcadeVehicleController>(),
//                carInstance.GetComponent<Health>()
//            );
//        }
//    }

//    private void BindUiComponents(GameObject carInstance)
//    {
//        if (_uiCarBinder != null)
//        {
//            Rigidbody rb = carInstance.GetComponent<Rigidbody>();
//            Health health = carInstance.GetComponent<Health>();
//            Transform carTransform = carInstance.transform;

//            if (rb != null && health != null)
//            {
//                _uiCarBinder.BindPlayerCar(rb, health, carTransform);
//            }
//        }

//        if (carInstance.TryGetComponent(out ArcadeVehicleController driftController))
//        {
//            _driftScoreUIDisplayer?.SetPlayerCar(driftController);
//        }
//    }

//    private void RegisterVehicleForRespawn()
//    {
//        if (_deadCarRespawner != null &&
//            _currentActiveCar.TryGetComponent(out Vehicle vehicle))
//        {
//            _deadCarRespawner.AddVehicle(vehicle);
//        }
//    }
//}







//public class PlayerCarSelector : MonoBehaviour
//{
//    [SerializeField] private UiCarBinder _uiCarBinder = null;
//    [SerializeField] private Transform _carPosition;
//    [SerializeField] private List<RaceCarItem> _carsList = new List<RaceCarItem>();

//    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
//    [Inject] private DriftScoreUIDisplayer _driftScoreUIDisplayer;
//    [Inject] private DeadCarRespawner _deadCarRespawner;
//    [Inject] private Container _container;

//    private Racer _playerRacer;
//    private GameObject _currentCarInstance;

//    private void Start()
//    {
//        ValidateCarsList();
//        InitializeCarsPosition();
//        ActivateLastUsedCar();
//    }

//    public Racer GetPlayerRacer()
//    {
//        return _playerRacer;
//    }

//    private void ValidateCarsList()
//    {
//        if (_carsList.Count == 0)
//        {
//            Debug.LogError("[RaceCarSelector] Cars list is empty!");
//            enabled = false;
//            return;
//        }

//        foreach (var carItem in _carsList)
//        {
//            if (carItem.carObject == null)
//            {
//                Debug.LogError($"[RaceCarSelector] Car {carItem.carId} has null reference!");
//                enabled = false;
//                return;
//            }
//        }
//    }

//    private void InitializeCarsPosition()
//    {
//        foreach (var carItem in _carsList)
//        {
//            if (carItem.carObject != null)
//            {
//                carItem.carObject.transform.SetPositionAndRotation(
//                    _carPosition.position,
//                    _carPosition.rotation
//                );
//                carItem.carObject.SetActive(false);
//            }
//        }
//    }

//    private void ActivateLastUsedCar()
//    {
//        int lastCarId = YandexGame.savesData.GetLastUsedCarId();
//        RaceCarItem targetCar = FindCarById(lastCarId);

//        if (targetCar == null)
//        {
//            Debug.LogWarning($"[RaceCarSelector] Car {lastCarId} not found, using default");
//            targetCar = _carsList.Count > 0 ? _carsList[0] : null;
//        }

//        if (targetCar != null)
//        {
//            ActivateCar(targetCar.carObject);
//        }
//        else
//        {
//            Debug.LogError("[RaceCarSelector] No valid cars available!");
//            enabled = false;
//        }
//    }

//    private RaceCarItem FindCarById(int carId)
//    {
//        foreach (var carItem in _carsList)
//        {
//            if (carItem.carId == carId)
//            {
//                return carItem;
//            }
//        }
//        return null;
//    }

//    private void ActivateCar(GameObject carObject)
//    {
//        if (carObject == null)
//        {
//            Debug.LogError("[RaceCarSelector] Attempting to activate null car object!");
//            enabled = false;
//            return;
//        }

//        DeactivateAllCars();
//        carObject.SetActive(true);
//        _currentCarInstance = carObject;

//        ProcessInjection(_currentCarInstance);
//        InitializeVehicleSystems(_currentCarInstance);
//        InitializeCarCustomizations(_currentCarInstance);
//        BindUiComponents(_currentCarInstance);

//        _playerRacer = _currentCarInstance.GetComponent<Racer>();

//        if (_deadCarRespawner != null &&
//            _currentCarInstance.TryGetComponent(out Vehicle vehicle))
//        {
//            _deadCarRespawner.AddVehicle(vehicle);
//        }
//    }

//    private void DeactivateAllCars()
//    {
//        foreach (var carItem in _carsList)
//        {
//            if (carItem.carObject != null)
//            {
//                carItem.carObject.SetActive(false);
//            }
//        }
//    }

//    private void ProcessInjection(GameObject target)
//    {
//        var injectables = target.GetComponentsInChildren<MonoBehaviour>(true);
//        foreach (var component in injectables)
//        {
//            try
//            {
//                AttributeInjector.Inject(component, _container);
//            }
//            catch (Exception e)
//            {
//                Debug.LogError($"Injection failed for {component.GetType().Name}: {e.Message}");
//            }
//        }
//    }

//    private void InitializeVehicleSystems(GameObject carInstance)
//    {
//        if (!carInstance.TryGetComponent(out Health playerHealth))
//        {
//            Debug.LogError("[RaceCarSelector] Health component missing!");
//            enabled = false;
//            return;
//        }

//        if (!carInstance.TryGetComponent(out ArcadeVehicleController _))
//        {
//            Debug.LogError("[RaceCarSelector] ArcadeVehicleController missing!");
//            enabled = false;
//            return;
//        }

//        if (_healthBarDisplay != null)
//        {
//            _healthBarDisplay.Initialize(playerHealth);
//        }
//        else
//        {
//            Debug.LogError("[RaceCarSelector] HealthBar Display reference not set!");
//            enabled = false;
//        }
//    }

//    private void InitializeCarCustomizations(GameObject carInstance)
//    {
//        CarUpgrades carUpgrades = carInstance.GetComponent<CarUpgrades>();
//        if (carUpgrades != null)
//        {
//            carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
//            carUpgrades.ApplyPurchasedStats(
//                YandexGame.savesData.HasCarUpgrade,
//                carInstance.GetComponent<ArcadeVehicleController>(),
//                carInstance.GetComponent<Health>()
//            );
//        }

//        CarModifications carMods = carInstance.GetComponent<CarModifications>();
//        if (carMods != null)
//        {
//            carMods.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
//            carMods.ApplyPurchasedMods(
//                YandexGame.savesData.GetCarModificationCount,
//                carInstance.GetComponent<ArcadeVehicleController>(),
//                carInstance.GetComponent<Health>()
//            );
//        }
//    }

//    private void BindUiComponents(GameObject carInstance)
//    {
//        if (_uiCarBinder != null)
//        {
//            Rigidbody rb = carInstance.GetComponent<Rigidbody>();
//            Health health = carInstance.GetComponent<Health>();
//            Transform carTransform = carInstance.transform;

//            if (rb != null && health != null && carTransform != null)
//            {
//                _uiCarBinder.BindPlayerCar(rb, health, carTransform);
//            }
//        }

//        if (carInstance.TryGetComponent(out ArcadeVehicleController driftController) &&
//            _driftScoreUIDisplayer != null)
//        {
//            _driftScoreUIDisplayer.SetPlayerCar(driftController);
//        }
//    }
//}








//public class PlayerCarSelector : MonoBehaviour
//{
//    [SerializeField] private UiCarBinder _uiCarBinder = null;
//    [SerializeField] private Transform _carPosition;

//    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
//    [Inject] private DriftScoreUIDisplayer _driftScoreUIDisplayer;
//    [Inject] private DeadCarRespawner _deadCarRespawner;
//    [Inject] private Container _container;

//    private Racer _playerRacer;
//    private GameObject _currentCarInstance;

//    private void Start()
//    {
//        ActivateLastUsedCar();
//    }

//    public Racer GetPlayerRacer()
//    {
//        return _playerRacer;
//    }

//    private void ActivateLastUsedCar()
//    {
//        int lastCarId = YandexGame.savesData.GetLastUsedCarId();
//        GameObject carPrefab = LoadCarPrefab(lastCarId);
//        ActivateCar(carPrefab);
//    }

//    private GameObject LoadCarPrefab(int carId)
//    {
//        if (carId < 0)
//        {
//            Debug.LogWarning("[RaceCarSelector] Invalid car ID, using default Car_1");
//            carId = 1;
//        }

//        string resourcePath = $"Cars/Player/Car_{carId}";
//        GameObject prefab = Resources.Load<GameObject>(resourcePath);

//        if (prefab == null)
//        {
//            Debug.LogWarning($"[RaceCarSelector] Car_{carId} not found at {resourcePath}, loading default");
//            prefab = Resources.Load<GameObject>("Cars/Player/Car_1");

//            if (prefab == null)
//            {
//                Debug.LogError("[RaceCarSelector] Default car Car_1 not found in Resources!");
//                return null;
//            }
//        }

//        return prefab;
//    }

//    private void ActivateCar(GameObject carPrefab)
//    {
//        if (carPrefab == null)
//        {
//            Debug.LogError("[RaceCarSelector] Cannot activate null car prefab!");
//            enabled = false;
//            return;
//        }

//        CleanupExistingCar();

//        _currentCarInstance = Instantiate(carPrefab, _carPosition.position, Quaternion.identity);
//        _currentCarInstance.SetActive(true);

//        var injectables = _currentCarInstance.GetComponentsInChildren<MonoBehaviour>();

//        foreach (var component in injectables)
//        {
//            try
//            {
//                AttributeInjector.Inject(component, _container);
//            }
//            catch (Exception e)
//            {
//                Debug.LogError($"Injection failed for {component.GetType().Name}: {e.Message}");
//            }
//        }

//        InitializeVehicleSystems(_currentCarInstance);
//        InitializeCarCustomizations(_currentCarInstance);
//        BindUiComponents(_currentCarInstance);

//        _playerRacer = _currentCarInstance.GetComponent<Racer>();

//        if (_deadCarRespawner != null && _currentCarInstance.TryGetComponent(out Vehicle vehicle))
//        {
//            _deadCarRespawner.AddVehicle(vehicle);
//        }
//    }

//    private void CleanupExistingCar()
//    {
//        if (_currentCarInstance != null)
//        {
//            Destroy(_currentCarInstance);
//            _currentCarInstance = null;
//        }
//    }

//    private void InitializeVehicleSystems(GameObject carInstance)
//    {
//        if (!carInstance.TryGetComponent(out Health playerHealth))
//        {
//            Debug.LogError("[RaceCarSelector] Health component missing!");
//            enabled = false;
//            return;
//        }

//        if (!carInstance.TryGetComponent(out ArcadeVehicleController _))
//        {
//            Debug.LogError("[RaceCarSelector] ArcadeVehicleController missing!");
//            enabled = false;
//            return;
//        }

//        if (_healthBarDisplay != null)
//        {
//            _healthBarDisplay.Initialize(playerHealth);
//        }
//        else
//        {
//            Debug.LogError("[RaceCarSelector] HealthBar Display reference not set!");
//            enabled = false;
//            return;
//        }
//    }

//    private void InitializeCarCustomizations(GameObject carInstance)
//    {
//        CarUpgrades carUpgrades = carInstance.GetComponent<CarUpgrades>();
//        if (carUpgrades != null)
//        {
//            carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
//            carUpgrades.ApplyPurchasedStats(
//                YandexGame.savesData.HasCarUpgrade,
//                carInstance.GetComponent<ArcadeVehicleController>(),
//                carInstance.GetComponent<Health>()
//            );
//        }

//        CarModifications carMods = carInstance.GetComponent<CarModifications>();
//        if (carMods != null)
//        {
//            carMods.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
//            carMods.ApplyPurchasedMods(
//                YandexGame.savesData.GetCarModificationCount,
//                carInstance.GetComponent<ArcadeVehicleController>(),
//                carInstance.GetComponent<Health>()
//            );
//        }
//    }

//    private void BindUiComponents(GameObject carInstance)
//    {
//        if (_uiCarBinder != null)
//        {
//            Rigidbody rb = carInstance.GetComponent<Rigidbody>();
//            Health health = carInstance.GetComponent<Health>();
//            Transform carTransform = carInstance.transform;

//            if (rb != null && health != null && carTransform != null)
//            {
//                _uiCarBinder.BindPlayerCar(rb, health, carTransform);
//            }
//        }

//        if (carInstance.TryGetComponent(out ArcadeVehicleController driftController)
//            && _driftScoreUIDisplayer != null)
//        {
//            _driftScoreUIDisplayer.SetPlayerCar(driftController);
//        }
//    }
//}