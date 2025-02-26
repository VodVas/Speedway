using TMPro;
using UnityEngine;
using YG;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        UpdateMoneyDisplay();
    }

    private void OnEnable()
    {
        YandexGame.savesData.OnMoneyChanged += UpdateMoneyDisplay;
    }

    private void OnDisable()
    {
        YandexGame.savesData.OnMoneyChanged -= UpdateMoneyDisplay;
    }

    public void UpdateMoneyDisplay()
    {
        moneyText.text = $"{YandexGame.savesData.Money}";
    }
}