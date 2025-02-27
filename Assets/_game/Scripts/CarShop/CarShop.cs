using UnityEngine;
using YG;

[RequireComponent(typeof(CarShopUI))]
public class CarShop : MonoBehaviour
{
    [SerializeField] private CarCollection _carCollection = null;

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

        _purchaseValidator = new CarPurchaseValidator(carsInScene);

        if (_purchaseValidator.AvailableCarIndices.Count > 0)
        {
            _selectionCycler = new CarSelectionCycler(carsInScene, _purchaseValidator);

            _selectionCycler.SetCarActive(true);
        }
        else
        {
            Debug.Log("[CarShop] Все машины уже куплены или недоступны!");
        }

        UpdateUI();
    }

    public void BuyCurrentCar()
    {
        if (_purchaseValidator == null || _selectionCycler == null)
            return;

        if (_purchaseValidator.AvailableCarIndices.Count == 0)
        {
            Debug.Log("[CarShop] Нет доступных машин для покупки!");
            return;
        }

        bool canBuy = _purchaseValidator.TryBuyCar(_selectionCycler.CurrentIndex);

        if (canBuy)
        {
            CarData data = _selectionCycler.GetCurrentCarData();
            _selectionCycler.SetCarActive(false);
            YandexGame.savesData.AddCar(data.Id);
            YandexGame.SaveProgress();
            _purchaseValidator.RecalculateAvailability();
            _selectionCycler.RevalidateCurrentIndex();

            if (_purchaseValidator.AvailableCarIndices.Count > 0)
            {
                _selectionCycler.SetCarActive(true);
            }
            else
            {
                Debug.Log("[CarShop] Все машины куплены, больше ничего нет.");
            }

            UpdateUI();
        }
        else
        {
            Debug.Log("[CarShop] Недостаточно денег или ошибка в покупке.");
        }

        UpdateUI();
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
            Debug.Log("[CarShop] Нет других машин, чтобы переключиться вперёд.");
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
            Debug.Log("[CarShop] Нет других машин, чтобы переключиться назад.");
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
    }
}