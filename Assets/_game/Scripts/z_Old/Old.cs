public class Old
{
    #region RaceCarSelector

    //public class RaceCarSelector : MonoBehaviour
    //{
    //    [SerializeField] private List<RaceCarItem> _allCarsInRace;
    //    [SerializeField] private UiCarBinder _uiCarBinder = null;
    //    [SerializeField] private Transform _carPosition;

    //    [Inject] private SmoothSliderHealthBarDisplay _healthBarDisplay;
    //    [Inject] private DriftScoreUIDisplayer _driftScoreUIDisplayer;

    //    private RaceStartTimeCounter _raceTimer;
    //    private Racer _playerRacer;

    //    private void Start()
    //    {
    //        if (_allCarsInRace == null || _allCarsInRace.Count == 0)
    //        {
    //            Debug.LogWarning("[RaceCarSelector] —писок машин пуст!");
    //            return;
    //        }

    //        ActivateLastUsedCar();
    //    }

    //    public Racer GetPlayerRacer()
    //    {
    //        return _playerRacer;
    //    }

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

    //        if (!foundCar)
    //        {
    //            Debug.LogWarning($"[RaceCarSelector] ћашина с id={lastCarId} не найдена в списке!");

    //            ActivateCar(0);
    //        }
    //    }

    //    //private void ActivateLastUsedCar()
    //    //{
    //    //    int lastCarId = YandexGame.savesData.GetLastUsedCarId();

    //    //    if (lastCarId < 0)
    //    //    {
    //    //        lastCarId = 1;
    //    //        Debug.LogWarning("[RaceCarSelector] LastUsedCarId не найден, используем Car_1 по умолчанию.");
    //    //    }

    //    //    string resourceName = $"Cars/Player/Car_{lastCarId}";
    //    //    GameObject carPrefab = Resources.Load<GameObject>(resourceName);

    //    //    if (carPrefab == null)
    //    //    {
    //    //        Debug.LogWarning($"[RaceCarSelector] Ќе найден Resource: {resourceName}. »спользуем Car_1 вместо этого!");
    //    //        carPrefab = Resources.Load<GameObject>("Cars/Player/Car_1");
    //    //    }

    //    //    ActivateCar(carPrefab);
    //    //}

    //    private void ActivateCar(int index)
    //    {
    //        if (index < 0 || index >= _allCarsInRace.Count)
    //        {
    //            Debug.LogError($"[RaceCarSelector] Ќеверный индекс машины: {index}");
    //            enabled = false;
    //            return;
    //        }

    //        RaceCarItem item = _allCarsInRace[index];

    //        if (item == null || item.carObject == null)
    //        {
    //            Debug.LogError($"[RaceCarSelector] RaceCarItem или его prefab carObject не назначен дл€ index={index}");
    //            enabled = false;
    //            return;
    //        }

    //        GameObject carInstance = Instantiate(item.carObject, _carPosition.position, Quaternion.identity);
    //        carInstance.SetActive(true);

    //        PlayerComponentsEnabler playerEnabler = carInstance.GetComponent<PlayerComponentsEnabler>();
    //        if (playerEnabler != null)
    //        {
    //            playerEnabler.Initialize(_raceTimer);
    //        }
    //        else
    //        {
    //            Debug.LogError("PlayerComponentsEnabler not found on player car!");
    //        }

    //        if (!carInstance.TryGetComponent(out Health playerHealth))
    //        {
    //            Debug.LogError("[RaceCarSelector] Health component не найден на машине!");
    //            return;
    //        }

    //        if (!carInstance.TryGetComponent(out ArcadeVehicleController controller))
    //        {
    //            Debug.LogError("[RaceCarSelector] ArcadeVehicleController не найден на машине!");
    //            return;
    //        }

    //        if (_healthBarDisplay != null)
    //        {
    //            _healthBarDisplay.Initialize(playerHealth);
    //        }
    //        else
    //        {
    //            Debug.LogError("[RaceCarSelector] _healthBarDisplay не назначен в инспекторе!");
    //        }

    //        CarUpgrades carUpgrades = carInstance.GetComponent<CarUpgrades>();

    //        if (carUpgrades != null)
    //        {
    //            carUpgrades.InitializePurchasedUpgrades(YandexGame.savesData.HasCarUpgrade);

    //            carUpgrades.ApplyPurchasedStats(
    //                YandexGame.savesData.HasCarUpgrade,
    //                controller,
    //                playerHealth
    //            );
    //        }
    //        else
    //        {
    //            Debug.LogWarning("[RaceCarSelector] CarUpgrades не найден на инстанцированной машине!");
    //        }

    //        CarModifications carModifications = carInstance.GetComponent<CarModifications>();

    //        if (carModifications != null)
    //        {
    //            carModifications.InitializePurchasedMods(YandexGame.savesData.GetCarModificationCount);
    //            carModifications.ApplyPurchasedMods(
    //                YandexGame.savesData.GetCarModificationCount,
    //                controller,
    //                playerHealth
    //            );
    //        }
    //        else
    //        {
    //            Debug.LogWarning("[RaceCarSelector] CarModifications не найден на машине!");
    //        }

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
    //            Debug.LogWarning("[RaceCarSelector] Ћибо ArcadeVehicleController не найден повторно, либо _driftScoreUIDisplayer не назначен!");
    //        }
    //    }
    //}

    #endregion
}