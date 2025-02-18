using ArcadeVP;
using UnityEngine;

public class DriftZoneBonusController : MonoBehaviour
{
    private const string ErrorBonusInvalid = "[DriftZoneBonus] Некорректное значение bonusAmount!";

    [SerializeField] private float _bonusAmount = 1f;

    private void Awake()
    {
        if (_bonusAmount <= 0f)
        {
            Debug.LogError(ErrorBonusInvalid, this);
            enabled = false;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ArcadeVehicleController>(out var playerCar))
        {
            playerCar.AddDriftZoneBonus(_bonusAmount);
        }
        else if (other.TryGetComponent<ArcadeAiVehicleController>(out var aiCar))
        {
            aiCar.AddDriftZoneBonus(_bonusAmount);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ArcadeVehicleController>(out var playerCar))
        {
            playerCar.AddDriftZoneBonus(-_bonusAmount);
        }
        else if (other.TryGetComponent<ArcadeAiVehicleController>(out var aiCar))
        {
            aiCar.AddDriftZoneBonus(-_bonusAmount);
        }
    }
}