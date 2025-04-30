using ArcadeVP;
using UnityEngine;

public class DriftScoreUIDisplayer : MonoBehaviour
{
    private const string ERROR_NO_DATA = "[DriftScoreUIDisplayer] No data available for UI (carUIDataArray). Disabling the component.";
    private const string ERROR_NULL_DATA = "[DriftScoreUIDisplayer] Null reference in carUIDataArray. Disabling the component.";
    private const string ERROR_NO_SCORE_TEXT = "[DriftScoreUIDisplayer] ScoreText is not assigned. Disabling the component.";
    private const string ERROR_NULL_PLAYER = "[DriftScoreUIDisplayer] Attempt to assign a null player!";
    private const string WARNING_NO_SLOT = "[DriftScoreUIDisplayer] No available slots for the player.";

    [SerializeField] private CarUIData[] _carUIDataArray;

    private void Awake()
    {
        ValidateAndInitialize();
    }

    private void ValidateAndInitialize()
    {
        if (!ValidateData()) return;

        foreach (var carData in _carUIDataArray)
        {
            carData.CacheComponents();
            carData.CacheAiRacer();
        }
    }

    private bool ValidateData()
    {
        if (_carUIDataArray == null || _carUIDataArray.Length == 0)
        {
            Debug.LogError(ERROR_NO_DATA, this);
            enabled = false;
            return false;
        }

        foreach (var carData in _carUIDataArray)
        {
            if (carData == null)
            {
                Debug.LogError(ERROR_NULL_DATA, this);
                enabled = false;
                return false;
            }

            if (carData.ScoreText == null)
            {
                Debug.LogError(ERROR_NO_SCORE_TEXT, this);
                enabled = false;
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        if (!enabled) return;

        foreach (var carData in _carUIDataArray)
        {
            carData.UpdateScoreDisplay();
        }
    }

    public void SetPlayerCar(ArcadeVehicleController newPlayerCar)
    {
        if (newPlayerCar == null)
        {
            Debug.LogError(ERROR_NULL_PLAYER, this);
            enabled = false;
            return;
        }

        foreach (var carData in _carUIDataArray)
        {
            if (carData.AiCar != null) continue;

            carData.PlayerCar = newPlayerCar;
            enabled = true;
            return;
        }

        Debug.LogWarning(WARNING_NO_SLOT, this);
    }

    public RacerInfo[] CollectAllRacerInfo()
    {
        var infos = new RacerInfo[_carUIDataArray.Length];
        for (int i = 0; i < _carUIDataArray.Length; i++)
            infos[i] = _carUIDataArray[i].GetRacerInfo();
        return infos;
    }

#if UNITY_EDITOR
    public void ValidatePlayerAssignment()
    {
        foreach (var carData in _carUIDataArray)
        {
            if (carData.PlayerCar != null)
                Debug.Log($"Player not found: {carData.GetRacerInfo().Name}");
        }
    }
#endif
}