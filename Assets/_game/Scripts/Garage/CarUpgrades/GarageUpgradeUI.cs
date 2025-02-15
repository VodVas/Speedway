using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.UI;

public class GarageUpgradeUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _upgradeNameText;
    [SerializeField] private TextMeshProUGUI _upgradePriceText;
    [SerializeField] private TextMeshProUGUI _feedbackText;
    [SerializeField] private TextMeshProUGUI _upgradeEffectText;
    [SerializeField] private TextMeshProUGUI _upgradeDescriptionText;

    [Header("Buttons")]
    [SerializeField] private Button _nextUpgradeButton;
    [SerializeField] private Button _prevUpgradeButton;
    [SerializeField] private Button _nextCarButton;
    [SerializeField] private Button _prevCarButton;
    [SerializeField] private Button _buyButton;

    [Inject] private SaveService _saveManager;
    [Inject] private GarageNavigator _garageManager;

    private int _currentUpgradeIndex = 0;

    private void Start()
    {
        SetupButtons();
        UpdateUI();
    }

    private void SetupButtons()
    {
        _buyButton.onClick.AddListener(OnBuyUpgrade);
        _nextUpgradeButton.onClick.AddListener(OnNextUpgrade);
        _prevUpgradeButton.onClick.AddListener(OnPrevUpgrade);

        _nextCarButton.onClick.AddListener(() =>
        {
            _garageManager.NextCar();
            _currentUpgradeIndex = 0;

            UpdateUI();
        });

        _prevCarButton.onClick.AddListener(() =>
        {
            _garageManager.PrevCar();
            _currentUpgradeIndex = 0;

            UpdateUI();
        });
    }

    private void UpdateUI()
    {
        var carUpgrades = _garageManager.GetCurrentCarUpgrades();
        if (carUpgrades == null || carUpgrades.Upgrades.Count == 0)
        {
            _upgradeNameText.text = "-";
            _upgradePriceText.text = "-";
            _upgradeEffectText.text = "";
            _upgradeDescriptionText.text = "";
            _buyButton.interactable = false;
            return;
        }

        _currentUpgradeIndex = Mathf.Clamp(_currentUpgradeIndex, 0, carUpgrades.Upgrades.Count - 1);

        CarUpgrade upgrade = carUpgrades.Upgrades[_currentUpgradeIndex];

        _upgradeNameText.text = upgrade.UpgradeName;
        _upgradePriceText.text = upgrade.Price.ToString();

        string effectDescription = "";

        switch (upgrade.UpgradeType)
        {
            case CarUpgrade.CarUpgradeType.Weapon:
                effectDescription = $"Урон {upgrade.UpgradeValue}";
                break;
            case CarUpgrade.CarUpgradeType.Acceleration:
                effectDescription = $"Ускорение +{upgrade.UpgradeValue}";
                break;
            case CarUpgrade.CarUpgradeType.Turn:
                effectDescription = $"Поворот +{upgrade.UpgradeValue}";
                break;
            case CarUpgrade.CarUpgradeType.Health:
                effectDescription = $"Броня +{upgrade.UpgradeValue}";
                break;
        }

        _upgradeEffectText.text = effectDescription;
        _upgradeDescriptionText.text = upgrade.UpgradeDescription;

        if (_saveManager.HasCarUpgrade(carUpgrades.CarId, upgrade.UpgradeId))
        {
            _buyButton.interactable = false;
            _feedbackText.text = "Уже куплено!";
        }
        else
        {
            _buyButton.interactable = true;
            _feedbackText.text = "";
        }

        foreach (var carUpgrade in carUpgrades.Upgrades)
        {
            if (!_saveManager.HasCarUpgrade(carUpgrades.CarId, carUpgrade.UpgradeId))
            {
                carUpgrade.SetActive(false);
            }
            else
            {
                carUpgrade.SetActive(true);
            }
        }

        upgrade.SetActive(true);
    }

    private void OnBuyUpgrade()
    {
        var carUpgrades = _garageManager.GetCurrentCarUpgrades();
        if (carUpgrades == null) return;

        CarUpgrade upgrade = carUpgrades.Upgrades[_currentUpgradeIndex];
        int carId = carUpgrades.CarId;

        if (_saveManager.HasCarUpgrade(carId, upgrade.UpgradeId))
        {
            _feedbackText.text = "Уже куплено!";
            return;
        }

        if (_saveManager.TrySpendMoney(upgrade.Price))
        {
            _saveManager.AddCarUpgrade(carId, upgrade.UpgradeId);
            _saveManager.Save();

            upgrade.SetActive(true);

            _feedbackText.text = $"Куплено: {upgrade.UpgradeName}";
        }
        else
        {
            _feedbackText.text = "Недостаточно средств!";
        }

        UpdateUI();
    }

    private void OnNextUpgrade()
    {
        var carUpgrades = _garageManager.GetCurrentCarUpgrades();

        if (carUpgrades == null || carUpgrades.Upgrades.Count == 0)
            return;

        _currentUpgradeIndex = (_currentUpgradeIndex + 1) % carUpgrades.Upgrades.Count;

        UpdateUI();
    }

    private void OnPrevUpgrade()
    {
        var carUpgrades = _garageManager.GetCurrentCarUpgrades();

        if (carUpgrades == null || carUpgrades.Upgrades.Count == 0)
            return;

        _currentUpgradeIndex = (_currentUpgradeIndex - 1 + carUpgrades.Upgrades.Count) % carUpgrades.Upgrades.Count;

        UpdateUI();
    }
}