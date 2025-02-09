using System;
using TMPro;
using UnityEngine;

public class CarShop : MonoBehaviour
{
    [SerializeField] private Wallet _wallet = null;
    [SerializeField] private CarCatalog _carCatalog = null;
    [SerializeField] private ObjectSwitcher _objectSwitcher = null;

    [Header("TextMeshPro References")]
    [SerializeField] private TextMeshPro _carNameText = null;
    [SerializeField] private TextMeshPro _carPriceText = null;
    [SerializeField] private TextMeshPro _playerMoneyText = null;

    [Header("Parameters")]
    [SerializeField] private TextMeshPro _SpeedText = null;
    [SerializeField] private TextMeshPro _AccelerationText = null;
    [SerializeField] private TextMeshPro _TurnText = null;
    [SerializeField] private TextMeshPro _ArmorText = null;
    [SerializeField] private TextMeshPro _WeaponText = null;

    public event Action<int> CarPurchased;

    private void Awake()
    {
        if (_wallet == null)
        {
            Debug.LogError("Shop: Wallet не назначен!", this);
            enabled = false;
            return;
        }

        if (_carCatalog == null)
        {
            Debug.LogError("Shop: CarCatalog не назначен!", this);
            enabled = false;
            return;
        }

        if (_objectSwitcher == null)
        {
            Debug.LogError("Shop: ObjectSwitcher не назначен!", this);
            enabled = false;
            return;
        }

        if (_carNameText == null || _carPriceText == null || _playerMoneyText == null || _SpeedText == null || _AccelerationText == null || _TurnText == null || _ArmorText == null || _WeaponText == null)
        {
            Debug.LogError("Shop: не все пол€ TextMeshPro заполнены!", this);
            enabled = false;
            return;
        }

        UpdateUI();
    }

    public void BuyCurrentCar()
    {
        int currentIndex = _objectSwitcher.GetCurrentIndex();

        CarData data = _carCatalog.GetCarData(currentIndex);

        if (data == null)
        {
            Debug.LogError("Shop: CarData = null или некорректный индекс.", this);
            return;
        }

        int price = data.Price;

        if (_wallet.TrySpendMoney(price))
        {
            Debug.Log($"Shop:  уплена машина '{data.CarName}' за {price}.");

            CarPurchased?.Invoke(currentIndex);

            _carCatalog.RemoveCarAtIndex(currentIndex);
            _objectSwitcher.RemoveCarAtIndex(currentIndex);

            UpdateUI();
        }
        else
        {
            Debug.Log("Shop: Ќедостаточно денег!");
        }
    }

    public void SwitchNextCar()
    {
        _objectSwitcher.SwitchToNextObject();

        UpdateUI();
    }

    public void SwitchPreviousCar()
    {
        _objectSwitcher.SwitchToPreviousObject();

        UpdateUI();
    }

    private void UpdateUI()
    {
        int idx = _objectSwitcher.GetCurrentIndex();

        if (idx < 0 || idx >= _carCatalog.GetCount())
        {
            Debug.LogError("Shop: текущий индекс недопустим!", this); //TODO: при покупке всех доступных машин вылезает ошибка конца списка
            return;
        }

        var carData = _carCatalog.GetCarData(idx);

        if (carData != null)
        {
            _carNameText.text = carData.CarName;
            _carPriceText.text = carData.Price.ToString();
            _SpeedText.text = carData.Speed.ToString();
            _AccelerationText.text = carData.Acceleration.ToString();
            _TurnText.text = carData.Turn.ToString();
            _ArmorText.text = carData.Armor.ToString();
            _WeaponText.text = carData.Weapon.ToString();
        }
        else
        {
            _carNameText.text = "ћашина не найдена";
            _carPriceText.text = "0";
        }

        _playerMoneyText.text = _wallet.CurrentMoney.ToString();
    }
}