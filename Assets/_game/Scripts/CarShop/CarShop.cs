using UnityEngine;
using YG;

[RequireComponent(typeof(CarShopUI))]
public class CarShop : MonoBehaviour
{
    [SerializeField] private CarCollection _carCollection = null;
    [SerializeField] private CarLootDatabase _epicCarsDatabase = null;

    private CarShopUI _carShopUI;
    private CarPurchaseValidator _purchaseValidator;
    private CarSelectionCycler _selectionCycler;

    private void Awake()
    {
        _carShopUI = GetComponent<CarShopUI>();

        if (_carCollection == null)
        {
            Debug.LogError("[CarShop] CarCollection не назначен!", this);
            enabled = false;
            return;
        }

        if (_epicCarsDatabase == null)
        {
            Debug.LogError("[CarShop] CarLootDatabase не назначен!", this);
            enabled = false;
            return;
        }

        if (_carShopUI == null)
        {
            Debug.LogError("[CarShop] CarShopUI не найден!", this);
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        var carsInScene = _carCollection.SceneCars;

        _purchaseValidator = new CarPurchaseValidator(carsInScene, _carCollection);

        if (_purchaseValidator.AvailableCarIndices.Count > 0)
        {
            _selectionCycler = new CarSelectionCycler(carsInScene, _purchaseValidator, _carCollection);

            _selectionCycler.SetCarActive(true);
        }
        else
        {
            Debug.Log("[CarShop] Нет машин для показа или покупки!", this);
            enabled = false;
            return;
        }

        UpdateUI();
    }

    public void BuyCurrentCar()
    {
        if (_purchaseValidator == null || _selectionCycler == null)
        {
            Debug.LogWarning("[CarShop] Системы покупки не инициализированы!", this);
            enabled = false;
            return;
        }

        if (_purchaseValidator.AvailableCarIndices.Count == 0)
        {
            Debug.Log("[CarShop] Нет доступных машин для покупки!", this);
            _carShopUI.DisplayNoCarsAvailable();
            return;
        }

        CarData currentCar = _selectionCycler.GetCurrentCarData();

        if (currentCar == null)
        {
            Debug.LogError("[CarShop] Не удалось получить данные текущей машины!", this);
            enabled = false;
            return;
        }

        bool isEpic = _selectionCycler.IsCurrentCarEpic();
        bool isUnlocked = !isEpic || _epicCarsDatabase.IsCarUnlocked(currentCar.Id);

        if (isEpic && !isUnlocked)
        {
            _carShopUI.DisplayEpicCarLocked();
            return;
        }

        int realIndex = _purchaseValidator.AvailableCarIndices[_selectionCycler.CurrentIndex];
        bool purchaseSuccess = _purchaseValidator.TryBuyCar(realIndex);

        if (purchaseSuccess)
        {
            _selectionCycler.SetCarActive(false);

            _purchaseValidator.RecalculateAvailability();
            _selectionCycler.RevalidateCurrentIndex();

            if (_purchaseValidator.AvailableCarIndices.Count > 0)
            {
                _selectionCycler.SetCarActive(true);
            }
            else
            {
                Debug.Log("[CarShop] Все машины куплены!", this);
                _carShopUI.DisplayNoCarsAvailable();
            }

            UpdateUI();
        }
        else
        {
            Debug.Log("[CarShop] Недостаточно денег для покупки!", this);
        }
    }

    public void SwitchNextCar()
    {
        if (_purchaseValidator == null || _selectionCycler == null)
            return;

        if (_purchaseValidator.AvailableCarIndices.Count > 1)
        {
            _selectionCycler.SwitchCar(1);
        }
        else
        {
            Debug.Log("[CarShop] Нет следующей машины, все машины куплены.", this);
        }

        UpdateUI();
    }

    public void SwitchPreviousCar()
    {
        if (_purchaseValidator == null || _selectionCycler == null)
            return;

        if (_purchaseValidator.AvailableCarIndices.Count > 1)
        {
            _selectionCycler.SwitchCar(-1);
        }
        else
        {
            Debug.Log("[CarShop] Нет предыдущей машины, все машины куплены.", this);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_carShopUI == null)
            return;

        if (_purchaseValidator == null || _purchaseValidator.AvailableCarIndices.Count == 0)
        {
            _carShopUI.DisplayNoCarsAvailable();
            return;
        }

        CarData currentCar = _selectionCycler?.GetCurrentCarData();
        if (currentCar == null)
        {
            _carShopUI.DisplayCarNotFound();
            return;
        }

        _carShopUI.DisplayCarData(currentCar);
        _carShopUI.UpdatePlayerMoney(YandexGame.savesData.Money);

        bool isEpic = _selectionCycler.IsCurrentCarEpic();
        //Debug.Log($"[CarShop] Текущая машина: {currentCar.CarName} (ID: {currentCar.Id})");
       // Debug.Log($"[CarShop] Это эпическая машина: {isEpic}");
       // Debug.Log($"[CarShop] Разблок: {_epicCarsDatabase.IsCarUnlocked(currentCar.Id)}");

        bool isUnlocked = !isEpic || _epicCarsDatabase.IsCarUnlocked(currentCar.Id);

        _carShopUI.SetBuyButtonInteractable(isUnlocked && YandexGame.savesData.Money >= currentCar.Price);

        if (isEpic)
        {
            if (!isUnlocked)
            {
                _carShopUI.DisplayEpicCarLocked();
            }
            else
            {
                _carShopUI.HideLockedState();
            }
        }
        else
        {
            _carShopUI.HideLockedState();
        }
    }
}