using TMPro;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(RectTransform))]
public class MoneyLootCardController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rarityText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _cardBackground;
    [SerializeField] private Image _currencyIcon;

    private RectTransform _rectTransform;
    private MoneyLootItem _cachedItem;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        if (_rarityText == null || _countText == null || _cardBackground == null || _currencyIcon == null)
        {
            Debug.Log("Dependencies not assign", this);
            enabled = false;
            return;
        }
    }

    public void ShowCard(MoneyLootItem item)
    {
        _cachedItem = item;
        ConfigureVisuals();
        gameObject.SetActive(true);
    }

    private void ConfigureVisuals()
    {
        if (_cachedItem == null) return;

        _rarityText.text = _cachedItem.DisplayName;
        _countText.text = _cachedItem.Count;
        _cardBackground.sprite = _cachedItem.CardSprite;
        _currencyIcon.sprite = _cachedItem.CurrencyIcon;

        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }
}