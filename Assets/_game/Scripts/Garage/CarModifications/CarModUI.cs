using UnityEngine;
using TMPro;
using UnityEngine.UI;
using YG;


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
        mod.RefreshMaterials(_paintSystem);

        int selectedIndex = YandexGame.savesData.GetSelectedMaterialIndex(
            _navigator.CurrentCarID, mod.ModificationId
        );

        _countText.text = $"{selectedIndex + 1}/{mod.MaterialsCount}";
        ApplyPreviewMaterial(mod, selectedIndex);
    }

    private void UpdateStatsUI(StatsCarModification mod)
    {
        _colorButtonsGroup.SetActive(false);
        _buyButtonGO.SetActive(true);

        int purchased = YandexGame.savesData.GetCarModificationCount(
            _navigator.CurrentCarID, mod.ModificationId
        );

        _countText.text = $"{purchased}/5";
        _priceText.text = mod.Price.ToString();
        _buyButton.interactable = purchased < 5;
    }

    private void ApplyPreviewMaterial(ColorCarModification mod, int index)
    {
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

    private void SetEmptyState()
    {
        _modNameText.text = "0";
        _priceText.text = "0";
        _countText.text = "0/0";
        _effectText.text = "No modifications";
        _feedbackText.text = string.Empty;
    }
}






//public class CarModUI : MonoBehaviour
//{
//    [SerializeField] private GarageNavigator _navigator;
//    [Header("UI Elements")]
//    [SerializeField] private TextMeshProUGUI _modificationNameText;
//    [SerializeField] private TextMeshProUGUI _modificationPriceText;
//    [SerializeField] private TextMeshProUGUI _countText;
//    [SerializeField] private TextMeshProUGUI _feedbackText;
//    [SerializeField] private TextMeshProUGUI _modificationEffectText;
//    [SerializeField] private Button _buyButton;
//    [Header("Navigation Buttons")]
//    [SerializeField] private Button _nextModButton;
//    [SerializeField] private Button _prevModButton;
//    [SerializeField] private Button _nextMaterialButton;
//    [SerializeField] private Button _prevMaterialButton;
//    [SerializeField] private GameObject _nextMaterialButtonGO;
//    [SerializeField] private GameObject _prevMaterialButtonGO;
//    [SerializeField] private GameObject _buyButtonGO;
//    [SerializeField] private PaintIntegrationSystem _paintSystem;

//    private int _currentIndex;
//    //private int _selectedIndex = 0;

//    private void Awake()
//    {
//        if (!ValidateReferences())
//        { 
//            enabled = false;
//            return;
//        }
//    }

//    private void ApplyPreviewMaterial(CarModification mod, int index)
//    {
//        if (mod == null || _paintSystem == null) return;

//        Material mat = mod.GetRuntimeMaterial(index);
//        if (mat != null && mod.TargetRenderer != null)
//        {
//            // Создаем массив материалов с одинаковым материалом
//            Material[] newMaterials = new Material[mod.TargetRenderer.materials.Length];
//            for (int i = 0; i < newMaterials.Length; i++)
//            {
//                newMaterials[i] = mat;
//            }
//            mod.TargetRenderer.materials = newMaterials;
//        }
//    }

//    //private void ApplyPreviewMaterial(CarModification mod, int index)
//    //{
//    //    if (mod == null || _paintSystem == null) return;

//    //    Material mat = mod.GetRuntimeMaterial(index);
//    //    if (mat != null && mod.TargetRenderer != null)
//    //    {
//    //        mod.TargetRenderer.material = mat;
//    //    }
//    //}

//    private bool ValidateReferences()
//    {
//        bool valid = _navigator != null &&
//                    _modificationNameText != null &&
//                    _modificationPriceText != null &&
//                    _countText != null &&
//                    _feedbackText != null &&
//                    _modificationEffectText != null &&
//                    _buyButton != null &&
//                    _nextModButton != null &&
//                    _prevModButton != null &&
//                    _nextMaterialButton != null &&
//                    _prevMaterialButton != null;

//        if (!valid)
//        {
//            Debug.LogError("[CarModUI] Неверные ссылки!");
//        }

//        return valid;
//    }

//    private void Start()
//    {
//        _buyButton.onClick.AddListener(BuyCurrentModification);
//        _nextModButton.onClick.AddListener(NextModification);
//        _prevModButton.onClick.AddListener(PrevModification);
//        _nextMaterialButton.onClick.AddListener(NextMaterial);
//        _prevMaterialButton.onClick.AddListener(PrevMaterial);
//        UpdateUI();
//    }

//    private void OnEnable() => _navigator.OnGarageReady += UpdateUI;
//    private void OnDisable() => _navigator.OnGarageReady -= UpdateUI;

//    private void UpdateUI()
//    {
//        var modsComp = _navigator.GetCurrentCarModifications();
//        if (modsComp == null || modsComp.GetAll().Count == 0)
//        {
//            SetEmptyState();
//            return;
//        }

//        _currentIndex = Mathf.Clamp(_currentIndex, 0, modsComp.GetAll().Count - 1);
//        var currentMod = modsComp.GetAll()[_currentIndex];

//        if (currentMod == null)
//        {
//            SetErrorState();
//            return;
//        }

//        UpdateUIForModification(currentMod, modsComp.CarId);
//    }

//    private void UpdateUIForModification(CarModification mod, int carId)
//    {
//        _modificationNameText.text = mod.ModificationName;
//        _modificationEffectText.text = GetEffectDescription(mod);

//        if (mod.Type == CarModification.ModificationType.Color)
//        {
//            if (_paintSystem == null)
//            {
//                Debug.LogError("[CarModUI] PaintSystem не назначен!");
//                return;
//            }

//            if (!_paintSystem.IsInitialized)
//            {
//                _paintSystem.ForceRefresh();
//            }

//            mod.UpdateRuntimeMaterials(_paintSystem);
//            HandleColorModUI(mod, carId);
//        }
//        else
//        {
//            HandleStandardModUI(mod, carId);
//        }
//    }

//    private string GetEffectDescription(CarModification mod)
//    {
//        return mod.Type switch
//        {
//            CarModification.ModificationType.Speed => $"Скорость +{mod.Value}",
//            CarModification.ModificationType.Acceleration => $"Ускорение +{mod.Value}",
//            CarModification.ModificationType.Turn => $"Поворот +{mod.Value}",
//            CarModification.ModificationType.Health => $"Жизнь +{mod.Value}",
//            CarModification.ModificationType.Color => "Цвет машины",
//            _ => string.Empty
//        };
//    }

//    private void HandleColorModUI(CarModification mod, int carId)
//    {
//        _buyButtonGO.SetActive(false);
//        _modificationPriceText.text = "0";
//        _nextMaterialButtonGO.SetActive(true);
//        _prevMaterialButtonGO.SetActive(true);

//        if (!_paintSystem.IsInitialized)
//        {
//            _paintSystem.ForceRefresh();
//            mod.UpdateRuntimeMaterials(_paintSystem);
//        }

//        int selectedIndex = YandexGame.savesData.GetSelectedMaterialIndex(carId, mod.ModificationId);
//        int materialsCount = mod.GetRuntimeMaterialsCount();

//        selectedIndex = Mathf.Clamp(selectedIndex, 0, materialsCount - 1);

//        _countText.text = $"{selectedIndex + 1}/{materialsCount}";
//        _feedbackText.text = string.Empty;

//        ApplyPreviewMaterial(mod, selectedIndex);
//    }

//    private void HandleStandardModUI(CarModification mod, int carId)
//    {
//        _buyButtonGO.SetActive(true);
//        _nextMaterialButtonGO.SetActive(false);
//        _prevMaterialButtonGO.SetActive(false);
//        _modificationPriceText.text = mod.Price.ToString();

//        int purchasedCount = YandexGame.savesData.GetCarModificationCount(carId, mod.ModificationId);
//        _countText.text = $"{purchasedCount}/5";
//        _buyButton.interactable = purchasedCount < 5;
//        _feedbackText.text = purchasedCount >= 5 ? "Максимум куплено (5)" : string.Empty;
//    }

//    private void NextMaterial()
//    {
//        var mod = GetCurrentModification();
//        if (mod?.Type != CarModification.ModificationType.Color) return;

//        int carId = _navigator.GetCurrentCarModifications().CarId;
//        int currentIndex = YandexGame.savesData.GetSelectedMaterialIndex(carId, mod.ModificationId);
//        int materialsCount = mod.GetRuntimeMaterialsCount();
//        int newIndex = (currentIndex + 1) % materialsCount;
//        YandexGame.savesData.SetSelectedMaterialIndex(carId, mod.ModificationId, newIndex);
//        YandexGame.SaveProgress();
//        UpdateUI();
//    }

//    private void PrevMaterial()
//    {
//        var mod = GetCurrentModification();
//        if (mod?.Type != CarModification.ModificationType.Color) return;

//        int carId = _navigator.GetCurrentCarModifications().CarId;
//        int currentIndex = YandexGame.savesData.GetSelectedMaterialIndex(carId, mod.ModificationId);
//        int materialsCount = mod.GetRuntimeMaterialsCount();
//        int newIndex = (currentIndex - 1 + materialsCount) % materialsCount;
//        YandexGame.savesData.SetSelectedMaterialIndex(carId, mod.ModificationId, newIndex);
//        YandexGame.SaveProgress();
//        UpdateUI();
//    }

//    private CarModification GetCurrentModification()
//    {
//        var modsComp = _navigator.GetCurrentCarModifications();
//        return modsComp?.GetAll()[_currentIndex];
//    }

//    private void SetEmptyState()
//    {
//        _modificationNameText.text = "-";
//        _modificationPriceText.text = "-";
//        _countText.text = "0/0";
//        _feedbackText.text = "Нет модификаций";
//        _buyButton.interactable = false;
//    }

//    private void SetErrorState()
//    {
//        _modificationNameText.text = "ERROR";
//        _modificationPriceText.text = "ERROR";
//        _countText.text = "0/0";
//        _feedbackText.text = "Ошибка загрузки";
//        _buyButton.interactable = false;
//    }

//    private void BuyCurrentModification()
//    {
//        var modsComp = _navigator.GetCurrentCarModifications();

//        if (modsComp == null)
//        {
//            _feedbackText.text = "Нет модификаций";
//            return;
//        }

//        var allMods = modsComp.GetAll();

//        if (allMods == null || allMods.Count == 0)
//        {
//            _feedbackText.text = "Ошибка данных";
//            return;
//        }

//        var mod = allMods[_currentIndex];

//        if (mod == null)
//        {
//            _feedbackText.text = "Неизвестная модификация";
//            return;
//        }

//        int carId = modsComp.CarId;
//        int alreadyBought = YandexGame.savesData.GetCarModificationCount(carId, mod.ModificationId);

//        if (alreadyBought >= 5)
//        {
//            _feedbackText.text = "Максимум куплено (5)";
//            return;
//        }

//        if (YandexGame.savesData.TrySpendMoney(mod.Price))
//        {
//            YandexGame.savesData.AddCarModification(carId, mod.ModificationId);
//            YandexGame.SaveProgress();
//            _feedbackText.text = $"Куплено: {mod.ModificationName}";
//        }
//        else
//        {
//            _feedbackText.text = "Недостаточно денег!";
//        }

//        UpdateUI();
//    }

//    private void NextModification()
//    {
//        var modsComp = _navigator.GetCurrentCarModifications();

//        if (modsComp == null) return;

//        var allMods = modsComp.GetAll();

//        if (allMods == null || allMods.Count == 0) return;

//        _currentIndex = (_currentIndex + 1) % allMods.Count;
//        UpdateUI();
//    }

//    private void PrevModification()
//    {
//        var modsComp = _navigator.GetCurrentCarModifications();

//        if (modsComp == null) return;

//        var allMods = modsComp.GetAll();

//        if (allMods == null || allMods.Count == 0) return;

//        _currentIndex = (_currentIndex - 1 + allMods.Count) % allMods.Count;
//        UpdateUI();
//    }
//}