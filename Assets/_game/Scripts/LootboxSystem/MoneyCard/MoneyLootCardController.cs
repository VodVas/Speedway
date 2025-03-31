using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyLootCardController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rarityText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _cardBackground;


    public void ShowCard(PaintLootItem item)
    {
        ConfigureVisuals(item);
        gameObject.SetActive(true);
    }

    private void ConfigureVisuals(PaintLootItem item)
    {
        _rarityText.text = item.DisplayName;
        _countText.text = item.DisplayName;
        _cardBackground.sprite = item.CardSprite;
    }
}