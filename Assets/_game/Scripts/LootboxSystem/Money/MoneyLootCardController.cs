using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneyLootCardController : BaseLootCardController
{
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Image _currencyIcon;

    private void Awake() => AwakeBase();

    public void ShowCard(MoneyLootItem item)
    {
        ConfigureVisuals(item);
        gameObject.SetActive(true);
    }

    private void ConfigureVisuals(MoneyLootItem item)
    {
        SetItemName(item.DisplayName);
        SetBackground(item.CardSprite);
        ConfigureRarityUI(item.Rarity);
        SetRarityText(item.Rarity);

        if (_amountText) _amountText.text = item.Amount;
        if (_currencyIcon) _currencyIcon.sprite = item.CurrencyIcon;
    }
}