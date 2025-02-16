using TMPro;
using UnityEngine;


public sealed class HealthUIUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText = null;
    private Health _health = null;

    private void Awake()
    {
        if (_healthText == null)
        {
            Debug.LogError("[HealthUIUpdater] Текст для здоровья не назначен!", this);
            enabled = false;
            return;
        }

        // Если _health уже назначен в инспекторе — подпишемся
        if (_health != null)
        {
            SubscribeHealth(_health);
        }
    }

    public void AssignTargetHealth(Health newHealth)
    {
        if (newHealth == null)
        {
            Debug.LogError("[HealthUIUpdater] Назначен null в AssignTargetHealth!", this);
            enabled = false;
            return;
        }

        // Отписываемся от старого
        if (_health != null)
        {
            _health.Changed -= UpdateHealthDisplay;
        }

        _health = newHealth;
        SubscribeHealth(_health);
        enabled = true;
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.Changed -= UpdateHealthDisplay;
        }
    }

    private void SubscribeHealth(Health target)
    {
        target.Changed += UpdateHealthDisplay;
        UpdateHealthDisplay(target.Value / target.Max);
    }

    private void UpdateHealthDisplay(float healthPercentage)
    {
        if (_healthText != null && _health != null)
        {
            _healthText.text = _health.Value.ToString();
        }
    }
}





//public class HealthUIUpdater : MonoBehaviour
//{
//    [SerializeField] private Health _health;
//    [SerializeField] private TextMeshProUGUI _healthText;

//    private void Awake()
//    {
//        _health.Changed += UpdateHealthDisplay;

//        UpdateHealthDisplay(_health.Value / _health.Max);
//    }

//    private void OnDisable()
//    {
//        _health.Changed -= UpdateHealthDisplay;
//    }

//    private void UpdateHealthDisplay(float healthPercentage)
//    {
//        _healthText.text = $"{_health.Value}";
//    }
//}