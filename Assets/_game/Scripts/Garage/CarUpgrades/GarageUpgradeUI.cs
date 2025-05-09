using UnityEngine;
using TMPro;
using UnityEngine.UI;
using YG;

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

    [Header("Localization")]
    [SerializeField] private SystemLocalization _localization;

    [SerializeField] private GarageNavigator _navigator;

    private int _currentUpgradeIndex = 0;

    private void Start()
    {
        SetupButtons();
        UpdateUI();
    }

    private void OnEnable()
    {
        if (_navigator != null)
            _navigator.OnGarageReady += UpdateUI;
    }

    private void OnDisable()
    {
        if (_navigator != null)
            _navigator.OnGarageReady -= UpdateUI;
    }

    private void SetupButtons()
    {
        _buyButton.onClick.AddListener(OnBuyUpgrade);
        _nextUpgradeButton.onClick.AddListener(OnNextUpgrade);
        _prevUpgradeButton.onClick.AddListener(OnPrevUpgrade);

        _nextCarButton.onClick.AddListener(() =>
        {
            _navigator.NextCar();
            _currentUpgradeIndex = 0;
            UpdateUI();
        });

        _prevCarButton.onClick.AddListener(() =>
        {
            _navigator.PrevCar();
            _currentUpgradeIndex = 0;
            UpdateUI();
        });
    }

    private void UpdateUI()
    {
        var carUpgrades = _navigator.GetCurrentCarUpgrades();
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

        string effectKey = upgrade.UpgradeType switch
        {
            CarUpgrade.CarUpgradeType.Weapon => "WeaponEffect",
            CarUpgrade.CarUpgradeType.Speed => "SpeedEffect",
            CarUpgrade.CarUpgradeType.Acceleration => "AccelerationEffect",
            CarUpgrade.CarUpgradeType.Turn => "TurnEffect",
            CarUpgrade.CarUpgradeType.Health => "HealthEffect",
            _ => "DefaultEffect"
        };

        _upgradeEffectText.text = _localization.GetPhrase(effectKey, upgrade.UpgradeValue);
        _upgradeDescriptionText.text = upgrade.UpgradeDescription;

        bool hasUpgrade = YandexGame.savesData.HasCarUpgrade(carUpgrades.CarId, upgrade.UpgradeId);
        _buyButton.interactable = !hasUpgrade;
        _feedbackText.text = hasUpgrade ?
            _localization.GetPhrase("AlreadyPurchased") :
            string.Empty;

        foreach (var carUpgrade in carUpgrades.Upgrades)
        {
            bool purchased = YandexGame.savesData.HasCarUpgrade(
                carUpgrades.CarId,
                carUpgrade.UpgradeId
            );
            carUpgrade.SetActive(purchased);
        }

        upgrade.SetActive(true);
    }

    private void OnBuyUpgrade()
    {
        var carUpgrades = _navigator.GetCurrentCarUpgrades();
        if (carUpgrades == null) return;

        CarUpgrade upgrade = carUpgrades.Upgrades[_currentUpgradeIndex];
        int carId = carUpgrades.CarId;

        if (YandexGame.savesData.HasCarUpgrade(carId, upgrade.UpgradeId))
        {
            _feedbackText.text = _localization.GetPhrase("AlreadyPurchased");
            return;
        }

        if (YandexGame.savesData.TrySpendMoney(upgrade.Price))
        {
            YandexGame.savesData.AddCarUpgrade(carId, upgrade.UpgradeId);
            YandexGame.SaveProgress();
            upgrade.SetActive(true);
            _feedbackText.text = _localization.GetPhrase("Purchased", upgrade.UpgradeName);
        }
        else
        {
            _feedbackText.text = _localization.GetPhrase("NotEnoughMoney");
        }

        UpdateUI();
    }

    private void OnNextUpgrade()
    {
        var carUpgrades = _navigator.GetCurrentCarUpgrades();
        if (carUpgrades == null || carUpgrades.Upgrades.Count == 0) return;

        _currentUpgradeIndex = (_currentUpgradeIndex + 1) % carUpgrades.Upgrades.Count;
        UpdateUI();
    }

    private void OnPrevUpgrade()
    {
        var carUpgrades = _navigator.GetCurrentCarUpgrades();
        if (carUpgrades == null || carUpgrades.Upgrades.Count == 0) return;

        _currentUpgradeIndex = (_currentUpgradeIndex - 1 + carUpgrades.Upgrades.Count) % carUpgrades.Upgrades.Count;
        UpdateUI();
    }
}