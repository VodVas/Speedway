using UnityEngine;
using Reflex.Attributes;
using System.Collections.Generic;
using ArcadeVP;
using YG;

public class RaceCarSelector : MonoBehaviour
{
    [SerializeField] private List<RaceCarItem> _allCarsInRace;
    [SerializeField] private UiCarBinder _uiCarBinder = null;
    [SerializeField] private Transform _carPosition;

    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
    [Inject] private DriftScoreUIDisplayer _driftScoreUIDisplayer;

    private Racer _playerRacer;

    private void Start()
    {
        if (_allCarsInRace == null || _allCarsInRace.Count == 0)
        {
            Debug.LogWarning("[RaceCarSelector] Список машин пуст!");
            return;
        }

        ActivateLastUsedCar();
    }

    private void ActivateLastUsedCar()
    {
        int lastCarId = YandexGame.savesData.GetLastUsedCarId();

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

        if (!foundCar)
        {
            Debug.LogWarning($"[RaceCarSelector] Машина с id={lastCarId} не найдена в списке!");
            // Можем fallback-ом запустить первую машину
            ActivateCar(0);
        }
    }

    private void ActivateCar(int index)
    {
        if (index < 0 || index >= _allCarsInRace.Count)
        {
            Debug.LogError($"[RaceCarSelector] Неверный индекс машины: {index}");
            return;
        }

        RaceCarItem item = _allCarsInRace[index];
        if (item == null || item.carObject == null)
        {
            Debug.LogError($"[RaceCarSelector] RaceCarItem или его prefab carObject не назначен для index={index}");
            return;
        }

        // Создаём экземпляр машины
        GameObject carInstance = Instantiate(item.carObject, _carPosition.position, Quaternion.identity);
        carInstance.SetActive(true);

        // Проверяем все нужные компоненты на созданном объекте
        if (!carInstance.TryGetComponent(out Health playerHealth))
        {
            Debug.LogError("[RaceCarSelector] Health component не найден на машине!");
            return;
        }

        if (!carInstance.TryGetComponent(out ArcadeVehicleController controller))
        {
            Debug.LogError("[RaceCarSelector] ArcadeVehicleController не найден на машине!");
            return;
        }

        // Инициализируем HealthBar
        if (_healthBarDisplay != null)
        {
            _healthBarDisplay.Initialize(playerHealth);
        }
        else
        {
            Debug.LogError("[RaceCarSelector] _healthBarDisplay не назначен в инспекторе!");
        }

        // Теперь Берём CarUpgrades непосредственно с инстанцированной машины
        CarUpgrades carUpgrades = carInstance.GetComponent<CarUpgrades>();
        if (carUpgrades != null)
        {
            // Первым делом говорим машине, какие апгрейды куплены:
            carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);

            // Применяем характеристики:
            carUpgrades.ApplyPurchasedStats(
                YandexGame.savesData.HasCarUpgrade,
                controller,
                playerHealth
            );
        }
        else
        {
            Debug.LogWarning("[RaceCarSelector] CarUpgrades не найден на инстанцированной машине!");
        }

        // Аналогично с модификациями, если используете:
        CarModifications carModifications = carInstance.GetComponent<CarModifications>();
        if (carModifications != null)
        {
            carModifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
            carModifications.ApplyPurchasedMods(
                YandexGame.savesData.GetCarModificationCount,
                controller,
                playerHealth
            );
        }
        else
        {
            // Если модификации не обязательны, можно не ругаться
            // Debug.Log.Warning("[RaceCarSelector] CarModifications не найден на машине!");
        }

        // Сохраняем Racer, если он нужен
        _playerRacer = carInstance.GetComponent<Racer>();

        // Биндим UI, если нужно
        if (_uiCarBinder != null)
        {
            var rigidbody = carInstance.GetComponent<Rigidbody>();
            var health = carInstance.GetComponent<Health>();
            var carTransform = carInstance.transform;

            _uiCarBinder.BindPlayerCar(rigidbody, health, carTransform);
        }

        // Дрифт‐UI
        if (carInstance.TryGetComponent(out ArcadeVehicleController driftCar) && _driftScoreUIDisplayer != null)
        {
            _driftScoreUIDisplayer.SetPlayerCar(driftCar);
        }
        else
        {
            Debug.LogWarning("[RaceCarSelector] Либо ArcadeVehicleController не найден повторно, либо _driftScoreUIDisplayer не назначен!");
        }
    }

    public Racer GetPlayerRacer()
    {
        return _playerRacer;
    }
}














//public class RaceCarSelector : MonoBehaviour
//{
//    [SerializeField] private List<RaceCarItem> _allCarsInRace;
//    [SerializeField] private UiCarBinder _uiCarBinder = null;
//    [SerializeField] private Transform _carPosition;

//    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
//    [Inject] private DriftScoreUIDisplayer _driftScoreUIDisplayer;

//    private Racer _playerRacer;

//    private void Start()
//    {
//        if (_allCarsInRace == null || _allCarsInRace.Count == 0)
//        {
//            Debug.LogWarning("[RaceCarSelector] Список машин пуст!");
//            return;
//        }

//        //DeactivateAllCars();
//        ActivateLastUsedCar();
//    }

//    public Racer GetPlayerRacer()
//    {
//        return _playerRacer;
//    }

//    //private void DeactivateAllCars()
//    //{
//    //    for (int i = 0; i < _allCarsInRace.Count; i++)
//    //    {
//    //        if (_allCarsInRace[i] != null && _allCarsInRace[i].carObject != null)
//    //        {
//    //            _allCarsInRace[i].carObject.SetActive(false); //TODO: instantiate префаб

//    //           // Instantiate(_allCarsInRace[i].carObject, _carPosition.position, Quaternion.identity);
//    //        }
//    //    }
//    //}

//    private void ActivateLastUsedCar()
//    {
//        int lastCarId = YandexGame.savesData.GetLastUsedCarId();

//        if (lastCarId < 0)
//        {
//            Debug.LogWarning("[RaceCarSelector] LastUsedCarId не задан, включаем первую машину по умолчанию.");
//            ActivateCar(0);
//            return;
//        }

//        bool foundCar = false;

//        for (int i = 0; i < _allCarsInRace.Count; i++)
//        {
//            RaceCarItem item = _allCarsInRace[i];

//            if (item == null || item.carObject == null)
//                continue;

//            if (item.carId == lastCarId)
//            {
//                ActivateCar(i);
//                foundCar = true;
//                break;
//            }
//        }

//        if (foundCar == false)
//        {
//            Debug.LogWarning($"[RaceCarSelector] Машина с id={lastCarId} не найдена в списке!");
//        }
//    }

//    private void ActivateCar(int index)
//    {
//        RaceCarItem item = _allCarsInRace[index];

//        // Создайте экземпляр машины и сохраните его в переменной
//        GameObject carInstance = Instantiate(item.carObject, _carPosition.position, Quaternion.identity);

//        // Убедитесь, что экземпляр активен
//        carInstance.SetActive(true);

//        // Получите компонент Health из созданного экземпляра
//        if (!carInstance.TryGetComponent(out Health playerHealth))
//        {
//            Debug.LogError("[RaceCarSelector] Health component not found on the car object!");
//            return;
//        }

//        // Получите ArcadeVehicleController из созданного экземпляра
//        if (!carInstance.TryGetComponent(out ArcadeVehicleController controller))
//        {
//            Debug.LogError("[RaceCarSelector] ArcadeVehicleController не найден на созданном объекте!");
//            return;
//        }

//        // Теперь вы можете безопасно использовать controller и playerHealth
//        if (_healthBarDisplay != null)
//        {
//            _healthBarDisplay.Initialize(playerHealth);
//        }
//        else
//        {
//            Debug.LogError("[RaceCarSelector] HealthBarDisplay is not assigned!");
//        }

//        if (item.carUpgrades != null)
//        {
//            item.carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
//            item.carUpgrades.ApplyPurchasedStats(
//                YandexGame.savesData.HasCarUpgrade,
//                controller, // используем контроллер из созданного экземпляра
//                playerHealth
//            );
//        }

//        if (item.carModifications != null)
//        {
//            item.carModifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
//            item.carModifications.ApplyPurchasedMods(
//                YandexGame.savesData.GetCarModificationCount,
//                controller,
//                playerHealth
//            );
//        }

//        // Замените _playerRacer на тот, который был создан
//        _playerRacer = carInstance.GetComponent<Racer>();

//        if (_uiCarBinder != null)
//        {
//            var rigidbody = carInstance.GetComponent<Rigidbody>();
//            var health = carInstance.GetComponent<Health>();
//            var carTransform = carInstance.transform;

//            _uiCarBinder.BindPlayerCar(rigidbody, health, carTransform);
//        }

//        if (carInstance.TryGetComponent(out ArcadeVehicleController driftCar) && _driftScoreUIDisplayer != null)
//        {
//            _driftScoreUIDisplayer.SetPlayerCar(driftCar);
//        }
//        else
//        {
//            Debug.LogWarning("[RaceCarSelector] ArcadeVehicleController не найден");
//        }
//    }


//    //private void ActivateCar(int index)
//    //{
//    //    RaceCarItem item = _allCarsInRace[index];
//    //    Instantiate(item.carObject, _carPosition.position, Quaternion.identity);
//    //    item.carObject.SetActive(true);

//    //    if (item.carObject.TryGetComponent(out Health playerHealth) == false)
//    //    {
//    //        Debug.LogError("[RaceCarSelector] Health component not found on the car object!");
//    //        return;
//    //    }

//    //    if (_healthBarDisplay != null)
//    //    {
//    //        _healthBarDisplay.Initialize(playerHealth);
//    //    }
//    //    else
//    //    {
//    //        Debug.LogError("[RaceCarSelector] HealthBarDisplay is not assigned!");
//    //    }

//    //    if (item.carUpgrades != null)
//    //    {
//    //        item.carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);
//    //        item.carUpgrades.ApplyPurchasedStats(
//    //            YandexGame.savesData.HasCarUpgrade,
//    //            item.carObject.GetComponent<ArcadeVehicleController>(),
//    //            item.carObject.GetComponent<Health>()
//    //        );
//    //    }

//    //    if (item.carModifications != null)
//    //    {
//    //        item.carModifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
//    //        item.carModifications.ApplyPurchasedMods(
//    //            YandexGame.savesData.GetCarModificationCount,
//    //            item.carObject.GetComponent<ArcadeVehicleController>(),
//    //            item.carObject.GetComponent<Health>()
//    //        );
//    //    }

//    //    _playerRacer = item.carObject.GetComponent<Racer>();

//    //    if (_uiCarBinder != null)
//    //    {
//    //        var rigidbody = item.carObject.GetComponent<Rigidbody>();
//    //        var health = item.carObject.GetComponent<Health>();
//    //        var carTransform = item.carObject.transform;

//    //        _uiCarBinder.BindPlayerCar(rigidbody, health, carTransform);
//    //    }

//    //    if (item.carObject.TryGetComponent(out ArcadeVehicleController driftCar) && _driftScoreUIDisplayer != null)
//    //    {
//    //        _driftScoreUIDisplayer.SetPlayerCar(driftCar);
//    //    }
//    //    else
//    //    {
//    //        Debug.LogWarning("[RaceCarSelector] ArcadeVehicleController не найден");
//    //    }
//    //}
//}