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
            Debug.Log("[CarShop] Нет машин для показа или покупки!");
        }

        UpdateUI();
    }

    public void BuyCurrentCar()
    {
        // Проверка на null и пустые коллекции
        if (_purchaseValidator == null || _selectionCycler == null)
        {
            Debug.LogWarning("[CarShop] Системы покупки не инициализированы!");
            return;
        }

        // Проверка доступных машин
        if (_purchaseValidator.AvailableCarIndices.Count == 0)
        {
            Debug.Log("[CarShop] Нет доступных машин для покупки!");
            _carShopUI.DisplayNoCarsAvailable();
            return;
        }

        // Получаем текущую машину с проверкой
        CarData currentCar = _selectionCycler.GetCurrentCarData();
        if (currentCar == null)
        {
            Debug.LogError("[CarShop] Не удалось получить данные текущей машины!");
            return;
        }

        // Проверка блокировки эпической машины
        bool isEpic = _selectionCycler.IsCurrentCarEpic();
        bool isUnlocked = !isEpic || _epicCarsDatabase.IsCarUnlocked(currentCar.Id);

        if (isEpic && !isUnlocked)
        {
            _carShopUI.DisplayEpicCarLocked();
            return;
        }

        // Пытаемся купить машину
        int realIndex = _purchaseValidator.AvailableCarIndices[_selectionCycler.CurrentIndex];
        bool purchaseSuccess = _purchaseValidator.TryBuyCar(realIndex);

        if (purchaseSuccess)
        {
            // Успешная покупка
            _selectionCycler.SetCarActive(false);

            // Обновляем список доступных машин
            _purchaseValidator.RecalculateAvailability();
            _selectionCycler.RevalidateCurrentIndex();

            // Проверяем остались ли машины для показа
            if (_purchaseValidator.AvailableCarIndices.Count > 0)
            {
                // Переключаем на следующую доступную машину
                _selectionCycler.SetCarActive(true);
            }
            else
            {
                Debug.Log("[CarShop] Все машины куплены!");
                _carShopUI.DisplayNoCarsAvailable();
            }

            // Обновляем UI
            UpdateUI();
        }
        else
        {
            Debug.Log("[CarShop] Недостаточно денег для покупки!");
        }
    }

    //public void BuyCurrentCar()
    //{
    //    if (_purchaseValidator == null || _selectionCycler == null)
    //        return;

    //    if (_purchaseValidator.AvailableCarIndices.Count == 0)
    //    {
    //        Debug.Log("[CarShop] Нет доступных машин для покупки!");
    //        return;
    //    }

    //    CarData currentCar = _selectionCycler.GetCurrentCarData();
    //    if (currentCar == null)
    //        return;

    //    // Удален блок с прямой проверкой IsCurrentCarEpic()
    //    // Перенесена логика проверки в общий поток

    //    int realIndex = _purchaseValidator.AvailableCarIndices[_selectionCycler.CurrentIndex];
    //    bool canBuy = _purchaseValidator.TryBuyCar(realIndex);

    //    bool isEpic = _selectionCycler.IsCurrentCarEpic();
    //    bool isUnlocked = !isEpic || _epicCarsDatabase.IsCarUnlocked(currentCar.Id);

    //    if (isEpic && !isUnlocked)
    //    {
    //        _carShopUI.DisplayEpicCarLocked();
    //        return;
    //    }

    //    if (canBuy)
    //    {
    //        // Существующая логика обработки успешной покупки
    //        CarData data = _selectionCycler.GetCurrentCarData();
    //        _selectionCycler.SetCarActive(false);
    //        _selectionCycler.RevalidateCurrentIndex();

    //        if (_purchaseValidator.AvailableCarIndices.Count > 0)
    //        {
    //            _selectionCycler.SetCarActive(true);
    //        }
    //        else
    //        {
    //            Debug.Log("[CarShop] Все машины куплены.");
    //        }

    //        UpdateUI();
    //    }
    //    else
    //    {
    //        Debug.Log("[CarShop] Недостаточно денег для покупки.");
    //    }

    //    UpdateUI();
    //}






    //public void BuyCurrentCar()
    //{
    //    if (_purchaseValidator == null || _selectionCycler == null)
    //        return;

    //    if (_purchaseValidator.AvailableCarIndices.Count == 0)
    //    {
    //        Debug.Log("[CarShop] Нет доступных машин для покупки!");
    //        return;
    //    }

    //    CarData currentCar = _selectionCycler.GetCurrentCarData();
    //    if (currentCar == null)
    //        return;

    //    if (_selectionCycler.IsCurrentCarEpic())
    //    {
    //        _carShopUI.DisplayEpicCarLocked();
    //        return;
    //    }

    //    int realIndex = _purchaseValidator.AvailableCarIndices[_selectionCycler.CurrentIndex];
    //    bool canBuy = _purchaseValidator.TryBuyCar(realIndex);


    //    bool isEpic = _selectionCycler.IsCurrentCarEpic();
    //    bool isUnlocked = !isEpic || _epicCarsDatabase.IsCarUnlocked(currentCar.Id);

    //    if (isEpic && !isUnlocked)
    //    {
    //        _carShopUI.DisplayEpicCarLocked();
    //        return;
    //    }



    //    if (canBuy)
    //    {
    //        CarData data = _selectionCycler.GetCurrentCarData();
    //        _selectionCycler.SetCarActive(false);
    //        _selectionCycler.RevalidateCurrentIndex();

    //        if (_purchaseValidator.AvailableCarIndices.Count > 0)
    //        {
    //            _selectionCycler.SetCarActive(true);
    //        }
    //        else
    //        {
    //            Debug.Log("[CarShop] Все машины куплены.");
    //        }

    //        UpdateUI();
    //    }
    //    else
    //    {
    //        Debug.Log("[CarShop] Недостаточно денег для покупки.");
    //    }

    //    UpdateUI();
    //}

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
            Debug.Log("[CarShop] Нет следующей машины, все машины куплены.");
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
            Debug.Log("[CarShop] Нет предыдущей машины, все машины куплены.");
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
        Debug.Log($"[CarShop] Текущая машина: {currentCar.CarName} (ID: {currentCar.Id})");
        Debug.Log($"[CarShop] Это эпическая машина: {isEpic}");
        Debug.Log($"[CarShop] Разблок: {_epicCarsDatabase.IsCarUnlocked(currentCar.Id)}");

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