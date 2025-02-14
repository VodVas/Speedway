using TMPro;
using UnityEngine;
using Zenject;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    [Inject] private SaveService _saveManager;

    private void Start()
    {
        UpdateMoneyDisplay();
    }

    private void OnEnable()
    {
        _saveManager.OnMoneyChanged += UpdateMoneyDisplay;
    }

    private void OnDisable()
    {
        _saveManager.OnMoneyChanged -= UpdateMoneyDisplay;
    }

    public void UpdateMoneyDisplay()
    {
        moneyText.text = $"{_saveManager.Money}";
    }
}