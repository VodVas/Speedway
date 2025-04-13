using TMPro;
using UnityEngine;
using YG;

public class PlayerResourcesDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _furyText;

    private void Start()
    {
        if (_moneyText != null)
            UpdateMoneyDisplay();
        if (_furyText != null)
            UpdateFuryDisplay(YandexGame.savesData.Respect);
    }

    private void OnEnable()
    {
        if (_moneyText != null)
            YandexGame.savesData.OnMoneyChanged += UpdateMoneyDisplay;
        if (_furyText != null)
            YandexGame.savesData.OnRespectChanged += UpdateFuryDisplay;
    }

    private void OnDisable()
    {
        if (_moneyText != null)
            YandexGame.savesData.OnMoneyChanged -= UpdateMoneyDisplay;
        if (_furyText != null)
            YandexGame.savesData.OnRespectChanged -= UpdateFuryDisplay;
    }

    public void UpdateMoneyDisplay()
    {
        if (_moneyText != null)
            _moneyText.text = $"{YandexGame.savesData.Money}";
    }

    public void UpdateFuryDisplay(int value)
    {
        _furyText.text = $"{value}";
    }
}