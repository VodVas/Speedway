using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneyLootCardController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _rarityText;
    [SerializeField] private TextMeshProUGUI _epicRarityText;
    [SerializeField] private Image _cardBackground;
    [SerializeField] private Image _currencyIcon;

    public void ShowCard(MoneyLootItem item)
    {
        _amountText.text = item.Amount;
        _rarityText.text = item.DisplayName;
        _epicRarityText.text = item.DisplayName;
        _cardBackground.sprite = item.CardSprite;
        _currencyIcon.sprite = item.CurrencyIcon;

        if (item.Rarity == Rarity.Epic)
        {
            _rarityText.gameObject.SetActive(false);
            _epicRarityText.gameObject.SetActive(true);
        }
        else
        {
            _rarityText.gameObject.SetActive(true);
            _epicRarityText.gameObject.SetActive(false);
        }

    }

    //public void ShowCard(MoneyLootItem item)
    //{
    //    if (item is MoneyLootItem moneyItem)
    //    {
    //        _amountText.text = moneyItem.Amount;
    //        _rarityText.text = moneyItem.DisplayName;
    //        _epicRarityText.text = moneyItem.DisplayName;
    //        _cardBackground.sprite = moneyItem.CardSprite;
    //        _currencyIcon.sprite = moneyItem.CurrencyIcon;

    //        if (moneyItem.Rarity == Rarity.Epic)
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
}