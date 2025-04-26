using System;
using UnityEngine;
using YG;

public class PlayerProgressBridge : MonoBehaviour
{
    private const string ERROR_SAVE_DATA_NULL = "[PlayerProgressBridge] Saves data not available!";

    [SerializeField] private PaintIntegrationSystem _paintSystem;
    [SerializeField] private CarRegistry _carRegistry;

    public bool PaintUnlockedRecently { get; private set; }
    public int LastUnlockedPaintId { get; private set; } = -1;

    public event Action<LootRewardType, object> PlayerRewarded;

    private void Awake()
    {
        if (_paintSystem == null)
        {
            Debug.Log("PaintIntegrationSystem not assign",this);
            enabled = false;
            return;
        }    
            
        if (PaintUnlockedRecently)
        {
            RefreshPaintSystem();

            PaintUnlockedRecently = false;
        }
    }

    public void UnlockPaint(int paintId)
    {
        if (!TryValidateSaveData()) return;

        try
        {
            YandexGame.savesData.UnlockPaint(paintId);
            YandexGame.SaveProgress();
            
            PaintUnlockedRecently = true;
            LastUnlockedPaintId = paintId;
            
            RefreshPaintSystem();

            if (_carRegistry != null)
            {
                CarModifications[] cars = _carRegistry.Cars;

                for (int i = 0; i < cars.Length; i++)
                {
                    if (cars[i] != null && cars[i].isActiveAndEnabled)
                    {
                        cars[i].ApplyColors();
                    }
                }
            }

            PlayerRewarded?.Invoke(LootRewardType.Paint, paintId);
        }
        catch (Exception exception)
        {
            HandlePaintError(exception, paintId);
        }
    }

    public void UnlockCar(int carId)
    {
        if (YandexGame.savesData == null)
        {
            Debug.Log(ERROR_SAVE_DATA_NULL);
            enabled = false;
            return;
        }

        Debug.Log($"[PlayerProgressBridge] Разблокировка эпической машины с ID: {carId}");
        YandexGame.savesData.UnlockEpicCar(carId);
        YandexGame.SaveProgress();

        PlayerRewarded?.Invoke(LootRewardType.Car, carId);
    }

    public void AddMoney(string amountString)
    {
        if (YandexGame.savesData == null)
        {
            Debug.Log(ERROR_SAVE_DATA_NULL);
            enabled = false;
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


    private void RefreshPaintSystem()
    {
        if (_paintSystem != null)
        {
            _paintSystem.RefreshMaterials();
        }
        else
        {
            Debug.LogError("Paint system initialization failed after retries!");
            enabled = false;
            return;
        }
    }

    private bool TryValidateSaveData()
    {
        if (YandexGame.savesData != null) return true;

        Debug.LogError(ERROR_SAVE_DATA_NULL);
        enabled = false;
        return false;
    }

    private void HandlePaintError(Exception ex, int paintId)
    {
        Debug.LogError($"Failed to unlock paint {paintId}: {ex.Message}");
        enabled = false;
        return;
    }

    public void ForcePaintRefresh()
    {
        if (_paintSystem != null)
        {
            Debug.Log("[PlayerProgressBridge] Принудительное обновление системы красок");
            _paintSystem.RefreshMaterials();

            if (_carRegistry != null)
            {
                CarModifications[] cars = _carRegistry.Cars;

                for (int i = 0; i < cars.Length; i++)
                {
                    if (cars[i] != null && cars[i].isActiveAndEnabled)
                    {
                        cars[i].ApplyColors();
                    }
                }
            }
        }
    }
}