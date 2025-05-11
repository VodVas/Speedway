using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarShopUI : MonoBehaviour
{
    private const string EpicCarLocked = "EpicCarLocked";
    private const string PriceFormat = "PriceFormat";
    private const string NoCarsAvailable = "NoCarsAvailable";
    private const string CarNotFound = "CarNotFound";
    private const string DefaultPrice = "DefaultPrice";
    private const string DefaultStatValue = "DefaultStatValue";
    private const string MoneyFormat = "MoneyFormat";

    [Header("Localization")]
    [SerializeField] private SystemLocalization _localization;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _carNameText;
    [SerializeField] private TextMeshProUGUI _carPriceText;
    [SerializeField] private TextMeshProUGUI _playerMoneyText;
    [SerializeField] private TextMeshProUGUI _SpeedText;
    [SerializeField] private TextMeshProUGUI _AccelerationText;
    [SerializeField] private TextMeshProUGUI _TurnText;
    [SerializeField] private TextMeshProUGUI _ArmorText;
    [SerializeField] private GameObject _lockPanel;
    [SerializeField] private Button _buyButton;

    private void Awake()
    {
        ValidateLocalization();
        InitializeLockState();
    }

    private void ValidateLocalization()
    {
        if (_localization == null)
        {
            Debug.Log("[CarShopUI] SystemLocalization not found!");
            enabled = false;
            return;
        }

    }

    private void InitializeLockState()
    {
        if (_lockPanel != null)
        {
            _lockPanel.SetActive(false);
            UpdateLockPanelText();
        }
    }

    private void UpdateLockPanelText()
    {
        var textComponent = _lockPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
            textComponent.text = _localization.GetPhrase(EpicCarLocked);
    }

    public void DisplayCarData(CarData carData)
    {
        _carNameText.text = _localization.GetPhrase(carData.CarName);
        _carPriceText.text = FormatPrice(carData.Price);

        UpdateStatDisplays(carData);
        HideLockedState();
    }

    private string FormatPrice(int price) =>
        _localization.GetPhrase(PriceFormat, price);

    private void UpdateStatDisplays(CarData data)
    {
        _SpeedText.text = data.Speed.ToString();
        _AccelerationText.text = data.Acceleration.ToString();
        _TurnText.text = data.Turn.ToString();
        _ArmorText.text = data.Armor.ToString();
    }

    public void DisplayNoCarsAvailable()
    {
        SetDefaultState(_localization.GetPhrase(NoCarsAvailable));
    }

    public void DisplayCarNotFound()
    {
        SetDefaultState(_localization.GetPhrase(CarNotFound));
    }

    private void SetDefaultState(string message)
    {
        _carNameText.text = message;
        _carPriceText.text = _localization.GetPhrase(DefaultPrice);
        ResetStatsDisplay();
        HideLockedState();
    }

    private void ResetStatsDisplay()
    {
        string defaultValue = _localization.GetPhrase(DefaultStatValue);
        _SpeedText.text = defaultValue;
        _AccelerationText.text = defaultValue;
        _TurnText.text = defaultValue;
        _ArmorText.text = defaultValue;
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
            _buyButton.interactable = interactable;
        else
            Debug.LogWarning("[CarShopUI] Buy button reference missing!");
    }

    public void UpdatePlayerMoney(int money)
    {
        _playerMoneyText.text = _localization.GetPhrase(MoneyFormat, money);
    }
}