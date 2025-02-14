using UnityEngine;
using Zenject;

public class CarShop : MonoBehaviour
{
    [SerializeField] private CarCatalog _carCatalog = null;
    [SerializeField] private ObjectSwitcher _objectSwitcher = null;

    [Inject] private SaveService _saveManager;
    private CarShopUI _carShopUI;

    private void Awake()
    {
        if (_carCatalog == null || _objectSwitcher == null)
        {
            Debug.LogError("Shop: Не назначены необходимые компоненты!", this);
            enabled = false;
            return;
        }

        _carShopUI = GetComponent<CarShopUI>();
        if (_carShopUI == null)
        {
            Debug.LogError("Shop: CarShopUI не найден!", this);
            enabled = false;
            return;
        }

        RemoveAlreadyPurchasedCars();

        _objectSwitcher.Init();

        UpdateUI();
    }

    public void BuyCurrentCar()
    {
        if (_carCatalog.GetCount() == 0)
        {
            Debug.Log("Shop: Все машины уже куплены!");
            return;
        }

        int currentIndex = _objectSwitcher.GetCurrentIndex();
        CarData data = _carCatalog.GetCarData(currentIndex);

        if (data == null)
        {
            Debug.LogError("Shop: CarData = null или некорректный индекс.", this);
            return;
        }

        int price = data.Price;

        if (_saveManager.TrySpendMoney(price))
        {
            Debug.Log($"Shop: Куплена машина '{data.CarName}' за {price}.");

            _saveManager.AddCar(data.Id);
            _carCatalog.RemoveCarAtIndex(currentIndex);
            _objectSwitcher.RemoveCarAtIndex(currentIndex);
            _saveManager.Save();

            UpdateUI();
        }
        else
        {
            Debug.Log("Shop: Недостаточно денег!");
        }
    }

    public void SwitchNextCar()
    {
        if (_carCatalog.GetCount() == 0)
        {
            Debug.Log("Shop: Все машины уже куплены!");
            return;
        }

        _objectSwitcher.SwitchToNextObject();
        UpdateUI();
    }

    public void SwitchPreviousCar()
    {
        if (_carCatalog.GetCount() == 0)
        {
            Debug.Log("Shop: Все машины уже куплены!");
            return;
        }

        _objectSwitcher.SwitchToPreviousObject();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_carCatalog.GetCount() == 0)
        {
            _carShopUI.DisplayNoCarsAvailable();
            return;
        }

        int idx = _objectSwitcher.GetCurrentIndex();
        if (idx < 0 || idx >= _carCatalog.GetCount())
        {
            Debug.LogError("Shop: текущий индекс недопустим!", this);
            return;
        }

        CarData carData = _carCatalog.GetCarData(idx);
        if (carData != null)
        {
            _carShopUI.DisplayCarData(carData);
        }
        else
        {
            _carShopUI.DisplayCarNotFound();
        }

        _carShopUI.UpdatePlayerMoney(_saveManager.Money);
    }

    private void RemoveAlreadyPurchasedCars()
    {
        for (int i = _carCatalog.GetCount() - 1; i >= 0; i--)
        {
            CarData data = _carCatalog.GetCarData(i);

            if (data == null)
            {
                Debug.LogError($"Shop: В каталоге на индексе {i} нет CarData!", this);
                continue;
            }

            if (_saveManager.HasCar(data.Id))
            {
                _carCatalog.RemoveCarAtIndex(i);
                _objectSwitcher.RemoveCarAtIndex(i);
            }
        }
    }
}