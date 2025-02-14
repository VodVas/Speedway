using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.UI;

public sealed class CarModUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _modNameText;
    [SerializeField] private TextMeshProUGUI _modPriceText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _feedbackText;
    [SerializeField] private Button _buyButton;

    [Header("Buttons")]
    [SerializeField] private Button _nextButton;
    [SerializeField]private Button _prevButton;

    [Inject] private SaveService _save;
    [Inject] private GarageNavigator _navigator;
    private int _currentIndex;

    private void Awake()
    {
        if (_modNameText == null || _modPriceText == null || _countText == null ||
            _feedbackText == null || _buyButton == null || _nextButton == null || _prevButton == null)
        {
            Debug.LogError("[CarModUI] Ссылки на UI-элементы не настроены!", this);
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        _buyButton.onClick.AddListener(BuyCurrentModification);
        _nextButton.onClick.AddListener(NextModification);
        _prevButton.onClick.AddListener(PrevModification);

        _currentIndex = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var modsComp = _navigator.GetCurrentCarModifications();
        if (modsComp == null)
        {
            _feedbackText.text = "Модификаций нет";
            _modNameText.text = "-";
            _modPriceText.text = "-";
            _countText.text = "0/5";
            _buyButton.interactable = false;
            return;
        }

        var allMods = modsComp.GetAll();
        if (allMods == null || allMods.Count == 0)
        {
            _feedbackText.text = "Список пуст";
            _modNameText.text = "-";
            _modPriceText.text = "-";
            _countText.text = "0/5";
            _buyButton.interactable = false;
            return;
        }

        _currentIndex = Mathf.Clamp(_currentIndex, 0, allMods.Count - 1);
        var mod = allMods[_currentIndex];
        if (mod == null)
        {
            _feedbackText.text = "Ошибка модификации";
            _modNameText.text = "-";
            _modPriceText.text = "-";
            _countText.text = "0/5";
            _buyButton.interactable = false;
            return;
        }

        _modNameText.text = mod.ModificationName;
        _modPriceText.text = mod.Price.ToString();

        int carId = modsComp.CarId;
        int purchasedCount = _save.GetCarModificationCount(carId, mod.ModificationId);
        _countText.text = $"{purchasedCount}/5";

        if (purchasedCount >= 5)
        {
            _buyButton.interactable = false;
            _feedbackText.text = "Достигнут лимит (5 из 5)";
        }
        else
        {
            _buyButton.interactable = true;
            _feedbackText.text = "";
        }
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
        int alreadyBought = _save.GetCarModificationCount(carId, mod.ModificationId);
        if (alreadyBought >= 5)
        {
            _feedbackText.text = "Уже куплено максимум (5)";
            return;
        }

        if (_save.TrySpendMoney(mod.Price))
        {
            _save.AddCarModification(carId, mod.ModificationId);
            _save.Save();
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