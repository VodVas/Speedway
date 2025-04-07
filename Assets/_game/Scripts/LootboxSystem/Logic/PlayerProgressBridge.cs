using System;
using UnityEngine;
using YG;

public class PlayerProgressBridge : MonoBehaviour
{
    private const string ERROR_SAVE_DATA_NULL = "[PlayerProgressBridge] YandexGame.savesData is null!";

    public event Action<LootRewardType, object> PlayerRewarded;

    public void UnlockCar(int carId)
    {
        if (YandexGame.savesData == null)
        {
            Debug.Log(ERROR_SAVE_DATA_NULL);
            enabled = false;
            return;
        }

        YandexGame.savesData.UnlockEpicCar(carId);
        YandexGame.SaveProgress();

        PlayerRewarded?.Invoke(LootRewardType.Car, carId);
    }

    public void AddMoney(string amountString)
    {
        if (YandexGame.savesData == null)
        {
            Debug.Log(ERROR_SAVE_DATA_NULL);
            enabled |= false;
            return;
        }

        if (int.TryParse(amountString, out int amount))
        {
            YandexGame.savesData.AddMoney(amount);
            YandexGame.SaveProgress();

            PlayerRewarded?.Invoke(LootRewardType.Money, amount);
        }
        else
        {
            Debug.Log($"[PlayerProgressBridge] Failed to parse money amount: {amountString}");
            enabled = false;
            return;
        }
    }

    public void UnlockPaint(int paintId)
    {
        if (YandexGame.savesData == null)
        {
            Debug.Log(ERROR_SAVE_DATA_NULL);
            enabled = false;
            return;
        }

        YandexGame.savesData.UnlockPaint(paintId);
        YandexGame.SaveProgress();

        TryRefreshPaintSystem();

        PlayerRewarded?.Invoke(LootRewardType.Paint, paintId);
    }

    private void TryRefreshPaintSystem()
    {
        var paintSystem = FindObjectOfType<PaintIntegrationSystem>();
        if (paintSystem != null)
        {
            paintSystem.ForceRefresh();
        }
    }
}