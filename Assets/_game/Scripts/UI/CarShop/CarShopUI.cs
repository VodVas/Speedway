using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarShopUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro _carNameText = null;
    [SerializeField] private TextMeshPro _carPriceText = null;
    [SerializeField] private TextMeshProUGUI _playerMoneyText = null;
    [SerializeField] private TextMeshProUGUI _SpeedText = null;
    [SerializeField] private TextMeshProUGUI _AccelerationText = null;
    [SerializeField] private TextMeshProUGUI _TurnText = null;
    [SerializeField] private TextMeshProUGUI _ArmorText = null;
    [SerializeField] private GameObject _lockPanel;
    [SerializeField] private Button _buyButton;

    private void Awake()
    {
        if (_lockPanel != null)
            _lockPanel.SetActive(false);
    }

    public void DisplayCarData(CarData carData)
    {
        _carNameText.text = carData.CarName;
        _carPriceText.text = carData.Price.ToString();
        _SpeedText.text = carData.Speed.ToString();
        _AccelerationText.text = carData.Acceleration.ToString();
        _TurnText.text = carData.Turn.ToString();
        _ArmorText.text = carData.Armor.ToString();
        HideLockedState();
    }

    public void DisplayNoCarsAvailable()
    {
        _carNameText.text = "Машин нет!";
        _carPriceText.text = "0";
        _SpeedText.text = "0";
        _AccelerationText.text = "0";
        _TurnText.text = "0";
        _ArmorText.text = "0";
        HideLockedState();
    }

    public void DisplayCarNotFound()
    {
        _carNameText.text = "Машина не найдена";
        _carPriceText.text = "0";
        HideLockedState();
    }

    public void DisplayEpicCarLocked()
    {
        if (_lockPanel != null)
        {
            _lockPanel.SetActive(true);
            SetBuyButtonInteractable(false);
        }
    }

    public void HideLockedState()
    {
        if (_lockPanel != null)
        {
            _lockPanel.SetActive(false);
            SetBuyButtonInteractable(true);
        }
    }

    public void SetBuyButtonInteractable(bool interactable)
    {
        if (_buyButton != null)
        {
            _buyButton.interactable = interactable;
        }
        else
        {
            Debug.LogWarning("[CarShopUI] Кнопка покупки не назначена!", this);
        }
    }

    public void UpdatePlayerMoney(int money)
    {
        _playerMoneyText.text = money.ToString();
    }
}