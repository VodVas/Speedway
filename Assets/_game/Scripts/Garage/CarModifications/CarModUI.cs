using UnityEngine;
using TMPro;
using UnityEngine.UI;
using YG;

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
            enabled = false;
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

        if (!valid) Debug.LogError("[CarModUI] Не все ссылки настроены!");
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

        if (mod.Type == CarModification.ModificationType.Color)
        {
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
            CarModification.ModificationType.Turn => $"Маневры +{mod.Value}",
            CarModification.ModificationType.Health => $"Броня +{mod.Value}",
            CarModification.ModificationType.Color => "Цвет кузова",
            _ => string.Empty
        };
    }

    private void HandleColorModUI(CarModification mod, int carId)
    {
        //_buyButton.gameObject.SetActive(false);
        _buyButtonGO.SetActive(false);
        _modificationPriceText.text = "N/A";
        _nextMaterialButtonGO.SetActive(true);
        _prevMaterialButtonGO.SetActive(true);
        //_nextMaterialButton.gameObject.SetActive(true);
        //_prevMaterialButton.gameObject.SetActive(true);

        int selectedIndex = YandexGame.savesData.GetSelectedMaterialIndex(carId, mod.ModificationId);
        _countText.text = $"{selectedIndex + 1}/{mod.Materials.Length}";
        _feedbackText.text = string.Empty;

        ApplyPreviewMaterial(mod, selectedIndex);
    }

    private void ApplyPreviewMaterial(CarModification mod, int index)
    {
        if (mod.TargetRenderer != null && mod.Materials != null && index < mod.Materials.Length)
        {
            mod.TargetRenderer.material = mod.Materials[index];
        }
    }

    private void HandleStandardModUI(CarModification mod, int carId)
    {
        _buyButtonGO.SetActive(true);
        //_buyButton.gameObject.SetActive(true);
        //_nextMaterialButton.gameObject.SetActive(false);
        //_prevMaterialButton.gameObject.SetActive(false);
        _nextMaterialButtonGO.SetActive(false);
        _prevMaterialButtonGO.SetActive(false);
        _modificationPriceText.text = mod.Price.ToString();

        int purchasedCount = YandexGame.savesData.GetCarModificationCount(carId, mod.ModificationId);
        _countText.text = $"{purchasedCount}/5";
        _buyButton.interactable = purchasedCount < 5;
        _feedbackText.text = purchasedCount >= 5 ? "Достигнут лимит" : string.Empty;
    }

    private void NextMaterial()
    {
        var mod = GetCurrentModification();
        if (mod?.Type != CarModification.ModificationType.Color) return;

        int carId = _navigator.GetCurrentCarModifications().CarId;
        int currentIndex = YandexGame.savesData.GetSelectedMaterialIndex(carId, mod.ModificationId);
        int newIndex = (currentIndex + 1) % mod.Materials.Length;
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
        int newIndex = (currentIndex - 1 + mod.Materials.Length) % mod.Materials.Length;
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
            _feedbackText.text = "Список пуст";
            return;
        }

        var mod = allMods[_currentIndex];

        if (mod == null)
        {
            _feedbackText.text = "Неверная модификация";
            return;
        }

        int carId = modsComp.CarId;
        int alreadyBought = YandexGame.savesData.GetCarModificationCount(carId, mod.ModificationId);

        if (alreadyBought >= 5)
        {
            _feedbackText.text = "Уже куплено максимум (5)";
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
            _feedbackText.text = "Недостаточно средств!";
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










//public class CarModUI : MonoBehaviour
//{
//    [SerializeField] private GarageNavigator _navigator;

//    [Header("UI")]
//    [SerializeField] private TextMeshProUGUI _modificationNameText;
//    [SerializeField] private TextMeshProUGUI _modificationPriceText;
//    [SerializeField] private TextMeshProUGUI _countText;
//    [SerializeField] private TextMeshProUGUI _feedbackText;
//    [SerializeField] private TextMeshProUGUI _modificationEffectText;
//    [SerializeField] private Button _buyButton;

//    [Header("Buttons")]
//    [SerializeField] private Button _nextButton;
//    [SerializeField]private Button _prevButton;

//    private int _currentIndex;

//    private void Awake()
//    {
//        if (_modificationNameText == null || _modificationPriceText == null || _countText == null ||
//            _feedbackText == null || _buyButton == null || _nextButton == null || _prevButton == null || _modificationEffectText == null)
//        {
//            Debug.LogError("[CarModUI] Ссылки на UI-элементы не настроены!", this);
//            enabled = false;
//            return;
//        }
//    }

//    private void Start()
//    {
//        _buyButton.onClick.AddListener(BuyCurrentModification);
//        _nextButton.onClick.AddListener(NextModification);
//        _prevButton.onClick.AddListener(PrevModification);

//        _currentIndex = 0;
//        UpdateUI();
//    }

//    private void OnEnable()
//    {
//        if (_navigator != null)
//            _navigator.OnGarageReady += UpdateUI;
//    }

//    private void OnDisable()
//    {
//        if (_navigator != null)
//            _navigator.OnGarageReady -= UpdateUI;
//    }

//    private void UpdateUI()
//    {
//        var modificationsComp = _navigator.GetCurrentCarModifications();

//        if (modificationsComp == null)
//        {
//            _feedbackText.text = "Модификаций нет";
//            _modificationNameText.text = "-";
//            _modificationPriceText.text = "-";
//            _modificationEffectText.text = "";
//            _countText.text = "0/5";
//            _buyButton.interactable = false;
//            return;
//        }

//        var allMods = modificationsComp.GetAll();

//        if (allMods == null || allMods.Count == 0)
//        {
//            _feedbackText.text = "Список пуст";
//            _modificationNameText.text = "-";
//            _modificationPriceText.text = "-";
//            _modificationEffectText.text = "";
//            _countText.text = "0/5";
//            _buyButton.interactable = false;
//            return;
//        }

//        _currentIndex = Mathf.Clamp(_currentIndex, 0, allMods.Count - 1);
//        var modification = allMods[_currentIndex];

//        if (modification == null)
//        {
//            _feedbackText.text = "Ошибка модификации";
//            _modificationNameText.text = "-";
//            _modificationPriceText.text = "-";
//            _modificationEffectText.text = "";
//            _countText.text = "0/5";
//            _buyButton.interactable = false;
//            return;
//        }

//        _modificationNameText.text = modification.ModificationName;
//        _modificationPriceText.text = modification.Price.ToString();

//        int carId = modificationsComp.CarId;
//        int purchasedCount = YandexGame.savesData.GetCarModificationCount(carId, modification.ModificationId);
//        _countText.text = $"{purchasedCount}/5";

//        string effectDescription = "";

//        switch (modification.Type)
//        {
//            case CarModification.ModificationType.Speed:
//                effectDescription = $"Скорость +{modification.Value}";
//                break;
//            case CarModification.ModificationType.Acceleration:
//                effectDescription = $"Ускорение +{modification.Value}";
//                break;
//            case CarModification.ModificationType.Turn:
//                effectDescription = $"Маневры +{modification.Value}";
//                break;
//            case CarModification.ModificationType.Health:
//                effectDescription = $"Броня +{modification.Value}";
//                break;
//        }
//        _modificationEffectText.text = effectDescription;

//        if (purchasedCount >= 5)
//        {
//            _buyButton.interactable = false;
//            _feedbackText.text = "Достигнут лимит";
//        }
//        else
//        {
//            _buyButton.interactable = true;
//            _feedbackText.text = "";
//        }
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
//            _feedbackText.text = "Список пуст";
//            return;
//        }

//        var mod = allMods[_currentIndex];

//        if (mod == null)
//        {
//            _feedbackText.text = "Неверная модификация";
//            return;
//        }

//        int carId = modsComp.CarId;
//        int alreadyBought = YandexGame.savesData.GetCarModificationCount(carId, mod.ModificationId);

//        if (alreadyBought >= 5)
//        {
//            _feedbackText.text = "Уже куплено максимум (5)";
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
//            _feedbackText.text = "Недостаточно средств!";
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