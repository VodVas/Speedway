using UnityEngine;
using TMPro;
using UnityEngine.UI;
using YG;
using static CarModification;

public class CarModUI : MonoBehaviour
{
    [SerializeField] private GarageNavigator _navigator;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _modificationNameText;
    [SerializeField] private TextMeshProUGUI _modificationPriceText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _feedbackText;
    [SerializeField] private TextMeshProUGUI _modificationEffectText;
    [SerializeField] private Button _buyButton;
    [Header("Navigation Buttons")]
    [SerializeField] private Button _nextModButton;
    [SerializeField] private Button _prevModButton;
    [SerializeField] private Button _nextMaterialButton;
    [SerializeField] private Button _prevMaterialButton;
    [SerializeField] private GameObject _nextMaterialButtonGO;
    [SerializeField] private GameObject _prevMaterialButtonGO;
    [SerializeField] private GameObject _buyButtonGO;

    private int _currentIndex;

    private void Awake()
    {
        if (!ValidateReferences())
        { 
            enabled = false;
            return;
        }
    }

    [SerializeField] private PaintIntegrationSystem _paintSystem;
    private int _selectedIndex = 0;



    private void ApplyPreviewMaterial(CarModification mod, int index)
    {
        if (mod == null || _paintSystem == null) return;

        Material mat = mod.GetRuntimeMaterial(index);
        if (mat != null && mod.TargetRenderer != null)
        {
            mod.TargetRenderer.material = mat;
        }
    }

    private void UpdateColorModUI(CarModification mod)
    {
        mod.UpdateRuntimeMaterials(_paintSystem);
        int count = mod.GetRuntimeMaterialsCount();
        _countText.text = $"{_selectedIndex + 1}/{count}";
    }







    private bool ValidateReferences()
    {
        bool valid = _navigator != null &&
                    _modificationNameText != null &&
                    _modificationPriceText != null &&
                    _countText != null &&
                    _feedbackText != null &&
                    _modificationEffectText != null &&
                    _buyButton != null &&
                    _nextModButton != null &&
                    _prevModButton != null &&
                    _nextMaterialButton != null &&
                    _prevMaterialButton != null;

        if (!valid)
        {
            Debug.LogError("[CarModUI] Неверные ссылки!");
        }

        return valid;
    }

    private void Start()
    {
        _buyButton.onClick.AddListener(BuyCurrentModification);
        _nextModButton.onClick.AddListener(NextModification);
        _prevModButton.onClick.AddListener(PrevModification);
        _nextMaterialButton.onClick.AddListener(NextMaterial);
        _prevMaterialButton.onClick.AddListener(PrevMaterial);
        UpdateUI();
    }

    private void OnEnable() => _navigator.OnGarageReady += UpdateUI;
    private void OnDisable() => _navigator.OnGarageReady -= UpdateUI;

    private void UpdateUI()
    {
        var modsComp = _navigator.GetCurrentCarModifications();
        if (modsComp == null || modsComp.GetAll().Count == 0)
        {
            SetEmptyState();
            return;
        }

        _currentIndex = Mathf.Clamp(_currentIndex, 0, modsComp.GetAll().Count - 1);
        var currentMod = modsComp.GetAll()[_currentIndex];

        if (currentMod == null)
        {
            SetErrorState();
            return;
        }

        UpdateUIForModification(currentMod, modsComp.CarId);
    }

    private void UpdateUIForModification(CarModification mod, int carId)
    {
        _modificationNameText.text = mod.ModificationName;
        _modificationEffectText.text = GetEffectDescription(mod);

        if (mod.Type == ModificationType.Color)
        {
            if (_paintSystem == null)
            {
                Debug.LogError("[CarModUI] PaintSystem не назначен!");
                return;
            }

            if (!_paintSystem.IsInitialized)
            {
                _paintSystem.ForceRefresh();
            }

            mod.UpdateRuntimeMaterials(_paintSystem);
            HandleColorModUI(mod, carId);
        }
        else
        {
            HandleStandardModUI(mod, carId);
        }
    }

    private string GetEffectDescription(CarModification mod)
    {
        return mod.Type switch
        {
            CarModification.ModificationType.Speed => $"Скорость +{mod.Value}",
            CarModification.ModificationType.Acceleration => $"Ускорение +{mod.Value}",
            CarModification.ModificationType.Turn => $"Поворот +{mod.Value}",
            CarModification.ModificationType.Health => $"Жизнь +{mod.Value}",
            CarModification.ModificationType.Color => "Цвет машины",
            _ => string.Empty
        };
    }

    private void HandleColorModUI(CarModification mod, int carId)
    {
        _buyButtonGO.SetActive(false);
        _modificationPriceText.text = "N/A";
        _nextMaterialButtonGO.SetActive(true);
        _prevMaterialButtonGO.SetActive(true);

        if (!_paintSystem.IsInitialized)
        {
            _paintSystem.ForceRefresh();
            mod.UpdateRuntimeMaterials(_paintSystem);
        }

        // �������� mod.Materials.Length �� GetRuntimeMaterialsCount()
        int selectedIndex = YandexGame.savesData.GetSelectedMaterialIndex(carId, mod.ModificationId);
        int materialsCount = mod.GetRuntimeMaterialsCount();

        // ��������� �������� �������
        selectedIndex = Mathf.Clamp(selectedIndex, 0, materialsCount - 1);

        _countText.text = $"{selectedIndex + 1}/{materialsCount}";
        _feedbackText.text = string.Empty;

        ApplyPreviewMaterial(mod, selectedIndex);
    }

    private void HandleStandardModUI(CarModification mod, int carId)
    {
        _buyButtonGO.SetActive(true);
        _nextMaterialButtonGO.SetActive(false);
        _prevMaterialButtonGO.SetActive(false);
        _modificationPriceText.text = mod.Price.ToString();

        int purchasedCount = YandexGame.savesData.GetCarModificationCount(carId, mod.ModificationId);
        _countText.text = $"{purchasedCount}/5";
        _buyButton.interactable = purchasedCount < 5;
        _feedbackText.text = purchasedCount >= 5 ? "Максимум куплено (5)" : string.Empty;
    }

    private void NextMaterial()
    {
        var mod = GetCurrentModification();
        if (mod?.Type != CarModification.ModificationType.Color) return;

        int carId = _navigator.GetCurrentCarModifications().CarId;
        int currentIndex = YandexGame.savesData.GetSelectedMaterialIndex(carId, mod.ModificationId);
        int materialsCount = mod.GetRuntimeMaterialsCount();
        int newIndex = (currentIndex + 1) % materialsCount;
        YandexGame.savesData.SetSelectedMaterialIndex(carId, mod.ModificationId, newIndex);
        YandexGame.SaveProgress();
        UpdateUI();
    }

    private void PrevMaterial()
    {
        var mod = GetCurrentModification();
        if (mod?.Type != CarModification.ModificationType.Color) return;

        int carId = _navigator.GetCurrentCarModifications().CarId;
        int currentIndex = YandexGame.savesData.GetSelectedMaterialIndex(carId, mod.ModificationId);
        int materialsCount = mod.GetRuntimeMaterialsCount();
        int newIndex = (currentIndex - 1 + materialsCount) % materialsCount;
        YandexGame.savesData.SetSelectedMaterialIndex(carId, mod.ModificationId, newIndex);
        YandexGame.SaveProgress();
        UpdateUI();
    }

    private CarModification GetCurrentModification()
    {
        var modsComp = _navigator.GetCurrentCarModifications();
        return modsComp?.GetAll()[_currentIndex];
    }

    private void SetEmptyState()
    {
        _modificationNameText.text = "-";
        _modificationPriceText.text = "-";
        _countText.text = "0/0";
        _feedbackText.text = "Нет модификаций";
        _buyButton.interactable = false;
    }

    private void SetErrorState()
    {
        _modificationNameText.text = "ERROR";
        _modificationPriceText.text = "ERROR";
        _countText.text = "0/0";
        _feedbackText.text = "Ошибка загрузки";
        _buyButton.interactable = false;
    }

    private void BuyCurrentModification()
    {
        var modsComp = _navigator.GetCurrentCarModifications();

        if (modsComp == null)
        {
            _feedbackText.text = "Нет модификаций";
            return;
        }

        var allMods = modsComp.GetAll();

        if (allMods == null || allMods.Count == 0)
        {
            _feedbackText.text = "Ошибка данных";
            return;
        }

        var mod = allMods[_currentIndex];

        if (mod == null)
        {
            _feedbackText.text = "Неизвестная модификация";
            return;
        }

        int carId = modsComp.CarId;
        int alreadyBought = YandexGame.savesData.GetCarModificationCount(carId, mod.ModificationId);

        if (alreadyBought >= 5)
        {
            _feedbackText.text = "Максимум куплено (5)";
            return;
        }

        if (YandexGame.savesData.TrySpendMoney(mod.Price))
        {
            YandexGame.savesData.AddCarModification(carId, mod.ModificationId);
            YandexGame.SaveProgress();
            _feedbackText.text = $"Куплено: {mod.ModificationName}";
        }
        else
        {
            _feedbackText.text = "Недостаточно денег!";
        }

        UpdateUI();
    }

    private void NextModification()
    {
        var modsComp = _navigator.GetCurrentCarModifications();

        if (modsComp == null) return;

        var allMods = modsComp.GetAll();

        if (allMods == null || allMods.Count == 0) return;

        _currentIndex = (_currentIndex + 1) % allMods.Count;
        UpdateUI();
    }

    private void PrevModification()
    {
        var modsComp = _navigator.GetCurrentCarModifications();

        if (modsComp == null) return;

        var allMods = modsComp.GetAll();

        if (allMods == null || allMods.Count == 0) return;

        _currentIndex = (_currentIndex - 1 + allMods.Count) % allMods.Count;
        UpdateUI();
    }
}