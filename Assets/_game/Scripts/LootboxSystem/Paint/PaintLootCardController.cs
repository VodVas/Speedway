using UnityEngine;

public class PaintLootCardController : BaseLootCardController
{
    private void Awake() => AwakeBase();

    public void ShowCard(PaintLootItemSO item, GameObject sphere)
    {
        ConfigureVisuals(item);
        ActivateObject(sphere);
        gameObject.SetActive(true);
    }

    private void ConfigureVisuals(PaintLootItemSO item)
    {
        SetItemName(item.DisplayName);
        SetBackground(item.CardSprite);
        ConfigureRarityUI(item.Rarity);
        SetRarityText(item.Rarity);
    }
}