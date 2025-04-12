using UnityEngine;
using TMPro;
using UnityEngine.UI;
using YG;
using System.Collections;


public class CarModUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GarageNavigator _navigator;
    [SerializeField] private PaintIntegrationSystem _paintSystem;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _modNameText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _feedbackText;
    [SerializeField] private TextMeshProUGUI _effectText;

    [Header("Navigation")]
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _nextModButton;
    [SerializeField] private Button _prevModButton;
    [SerializeField] private Button _nextColorButton;
    [SerializeField] private Button _prevColorButton;
    [SerializeField] private GameObject _colorButtonsGroup;
    [SerializeField] private GameObject _buyButtonGO;

    private int _currentModIndex;
    private BaseCarModification[] _currentMods;

    private void Awake()
    {
        ValidateReferences();
        BindButtons();
        _navigator.OnGarageReady += InitializeUI;
        _navigator.OnCarChanged += HandleCarChanged;
    }

    private void OnDisable()
    {
        _navigator.OnCarChanged -= HandleCarChanged;
    }

    private void ValidateReferences()
    {
        if (_navigator == null || _modNameText == null || _priceText == null ||
            _countText == null || _feedbackText == null || _effectText == null ||
            _buyButton == null || _nextModButton == null || _prevModButton == null)
        {
            Debug.LogError("[CarModUI] Missing references!");
            enabled = false;
        }
    }

    private void BindButtons()
    {
        _buyButton.onClick.AddListener(BuyCurrentMod);
        _nextModButton.onClick.AddListener(NextMod);
        _prevModButton.onClick.AddListener(PrevMod);
        _nextColorButton.onClick.AddListener(NextColor);
        _prevColorButton.onClick.AddListener(PrevColor);
    }

    private void InitializeUI()
    {
        _currentMods = _navigator.GetCurrentCarModifications().Modifications;
        _currentModIndex = Mathf.Clamp(_currentModIndex, 0, _currentMods.Length - 1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_currentMods == null || _currentMods.Length == 0)
        {
            SetEmptyState();
            return;
        }

        BaseCarModification mod = _currentMods[_currentModIndex];
        _modNameText.text = mod.ModificationName;
        _effectText.text = mod.GetEffectDescription();

        if (mod is ColorCarModification colorMod)
            UpdateColorUI(colorMod);
        else if (mod is StatsCarModification statsMod)
            UpdateStatsUI(statsMod);
    }

    private void UpdateColorUI(ColorCarModification mod)
    {
        _colorButtonsGroup.SetActive(true);
        _buyButtonGO.SetActive(false);

        if (!mod.TargetRenderer.gameObject.activeInHierarchy)
            mod.TargetRenderer.gameObject.SetActive(true);

        mod.RefreshMaterials(_paintSystem);

        int selectedIndex = YandexGame.savesData.GetSelectedMaterialIndex(
            _navigator.CurrentCarID,
            mod.ModificationId
        );

        _countText.text = $"{selectedIndex + 1}/{mod.MaterialsCount}";
        ApplyPreviewMaterial(mod, selectedIndex);
    }

    private IEnumerator HandleCarChangedRoutine()
    {
        yield return null; // Ждём один кадр для активации объектов
        InitializeUI();
        _currentModIndex = 0;
        UpdateUI();
    }

    private void UpdateStatsUI(StatsCarModification mod)
    {
        _colorButtonsGroup.SetActive(false);
        _buyButtonGO.SetActive(true);

        int purchased = YandexGame.savesData.GetCarModificationCount(_navigator.CurrentCarID, mod.ModificationId);

        _countText.text = $"{purchased}/5";
        _priceText.text = mod.Price.ToString();
        _buyButton.interactable = purchased < 5;
    }

    private void ApplyPreviewMaterial(ColorCarModification mod, int index)
    {
        Debug.Log("ApplyPreviewMaterial");

        Material mat = mod.GetMaterial(index);
        if (mat == null || mod.TargetRenderer == null) return;

        Material[] mats = mod.TargetRenderer.materials;
        for (int i = 0; i < mats.Length; i++) mats[i] = mat;
        mod.TargetRenderer.materials = mats;
    }

    private void BuyCurrentMod()
    {
        if (_currentMods == null || _currentModIndex >= _currentMods.Length)
        {
            _feedbackText.text = "Ошибка: модификация не найдена";
            return;
        }

        var mod = _currentMods[_currentModIndex];
        int carId = _navigator.CurrentCarID;

        if (mod is StatsCarModification statsMod)
            HandleStatsPurchase(statsMod, carId);
        else
            _feedbackText.text = "Фидбэк";
    }

    private void HandleStatsPurchase(StatsCarModification mod, int carId)
    {
        int purchased = YandexGame.savesData.GetCarModificationCount(carId, mod.ModificationId);

        if (purchased >= 5)
        {
            _feedbackText.text = "Максимум куплено (5)";
            return;
        }

        if (YandexGame.savesData.Money < mod.Price)
        {
            _feedbackText.text = "Недостаточно денег!";
            return;
        }

        YandexGame.savesData.TrySpendMoney(mod.Price);
        YandexGame.savesData.AddCarModification(carId, mod.ModificationId);
        YandexGame.SaveProgress();

        _feedbackText.text = $"Успешно куплено: {mod.ModificationName}";
        UpdateUI();
    }

    private void NextMod() => ChangeModIndex(1);
    private void PrevMod() => ChangeModIndex(-1);

    private void ChangeModIndex(int delta)
    {
        _currentModIndex = (_currentModIndex + delta + _currentMods.Length) % _currentMods.Length;
        UpdateUI();
    }

    private void NextColor() => ChangeColorIndex(1);
    private void PrevColor() => ChangeColorIndex(-1);

    private void ChangeColorIndex(int delta)
    {
        var mod = _currentMods[_currentModIndex] as ColorCarModification;
        if (mod == null) return;

        int current = YandexGame.savesData.GetSelectedMaterialIndex(
            _navigator.CurrentCarID, mod.ModificationId
        );

        int newIndex = (current + delta + mod.MaterialsCount) % mod.MaterialsCount;
        YandexGame.savesData.SetSelectedMaterialIndex(_navigator.CurrentCarID, mod.ModificationId, newIndex);
        YandexGame.SaveProgress();
        UpdateUI();
    }

    private void HandleCarChanged()
    {
        StartCoroutine(HandleCarChangedRoutine());
    }

    private void SetEmptyState()
    {
        _modNameText.text = "0";
        _priceText.text = "0";
        _countText.text = "0/0";
        _effectText.text = "No modifications";
        _feedbackText.text = string.Empty;
    }
}