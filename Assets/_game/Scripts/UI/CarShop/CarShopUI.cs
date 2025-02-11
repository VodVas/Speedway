using TMPro;
using UnityEngine;

public class CarShopUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro _carNameText = null;
    [SerializeField] private TextMeshPro _carPriceText = null;
    [SerializeField] private TextMeshProUGUI _playerMoneyText = null;
    [SerializeField] private TextMeshProUGUI _SpeedText = null;
    [SerializeField] private TextMeshProUGUI _AccelerationText = null;
    [SerializeField] private TextMeshProUGUI _TurnText = null;
    [SerializeField] private TextMeshProUGUI _ArmorText = null;
    [SerializeField] private TextMeshProUGUI _WeaponText = null;

    public void DisplayCarData(CarData carData)
    {
        _carNameText.text = carData.CarName;
        _carPriceText.text = carData.Price.ToString();
        _SpeedText.text = carData.Speed.ToString();
        _AccelerationText.text = carData.Acceleration.ToString();
        _TurnText.text = carData.Turn.ToString();
        _ArmorText.text = carData.Armor.ToString();
        _WeaponText.text = carData.Weapon.ToString();
    }

    public void DisplayNoCarsAvailable()
    {
        _carNameText.text = "Машин нет!";
        _carPriceText.text = "0";
        _SpeedText.text = "0";
        _AccelerationText.text = "0";
        _TurnText.text = "0";
        _ArmorText.text = "0";
        _WeaponText.text = "0";
    }

    public void DisplayCarNotFound()
    {
        _carNameText.text = "Машина не найдена";
        _carPriceText.text = "0";
    }

    public void UpdatePlayerMoney(int money)
    {
        _playerMoneyText.text = money.ToString();
    }
}