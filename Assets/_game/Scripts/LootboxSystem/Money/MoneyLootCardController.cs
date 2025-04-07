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
        SetText(item.DisplayName);
        SetBackground(item.CardSprite);
        ConfigureRarityUI(item.Rarity);

        if (_amountText) _amountText.text = item.Amount;
        if (_currencyIcon) _currencyIcon.sprite = item.CurrencyIcon;
    }

    public void HideCard() => ResetCard();
}





//public class MoneyLootCardController : MonoBehaviour
//{
//    [SerializeField] private TextMeshProUGUI _amountText;
//    [SerializeField] private TextMeshProUGUI _rarityText;
//    [SerializeField] private TextMeshProUGUI _epicRarityText;
//    [SerializeField] private Image _cardBackground;
//    [SerializeField] private Image _currencyIcon;

//    public void ShowCard(MoneyLootItem item)
//    {
//        _amountText.text = item.Amount;
//        _rarityText.text = item.DisplayName;
//        _epicRarityText.text = item.DisplayName;
//        _cardBackground.sprite = item.CardSprite;
//        _currencyIcon.sprite = item.CurrencyIcon;

//        if (item.Rarity == Rarity.Epic)
//        {
//            _rarityText.gameObject.SetActive(false);
//            _epicRarityText.gameObject.SetActive(true);
//        }
//        else
//        {
//            _rarityText.gameObject.SetActive(true);
//            _epicRarityText.gameObject.SetActive(false);
//        }

//    }
//}