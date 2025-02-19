using UnityEngine;

public class UiCarBinder : MonoBehaviour
{
    [SerializeField] private SpeedDisplay _speedDisplay = null;
    [SerializeField] private HealthUIUpdater _healthUpdater = null;
    [SerializeField] private MinimapDisplay _minimapDisplay = null;
    [SerializeField] private RectTransform _playerIconPrefab = null;

    private void Awake()
    {
        if (_speedDisplay == null || _healthUpdater == null)
        {
            Debug.LogError("[UiCarBinder] Не все ссылки на UI скрипты назначены!", this);
            enabled = false;
            return;
        }
    }

    public void BindPlayerCar(Rigidbody playerRigidbody, Health playerHealth, Transform playerTransform)
    {
        if (playerRigidbody == null || playerTransform == null)
        {
            Debug.LogError("[UiCarBinder] BindPlayerCar получил некорректные аргументы!", this);
            enabled = false;
            return;
        }

        _speedDisplay.AssignTargetRigidbody(playerRigidbody);

        if (playerHealth != null)
        {
            _healthUpdater.AssignTargetHealth(playerHealth);
        }
        else
        {
            _healthUpdater.enabled = false;
        }

        if (_minimapDisplay != null && _playerIconPrefab != null)
        {
            _minimapDisplay.RegisterRacer(playerTransform, _playerIconPrefab);
        }
    }
}