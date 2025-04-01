using UnityEngine;
using TMPro;
using YG;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PlayerMoneyDisplayer : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        YandexGame.savesData.OnMoneyChanged += UpdateMoneyText;

        UpdateMoneyText();
    }

    private void OnDisable()
    {
        YandexGame.savesData.OnMoneyChanged -= UpdateMoneyText;
    }

    private void UpdateMoneyText()
    {
        _textMeshPro.text = FormatMoney(YandexGame.savesData.Money);
    }

    private string FormatMoney(int amount)
    {
        return amount.ToString("#,##0");
    }
}