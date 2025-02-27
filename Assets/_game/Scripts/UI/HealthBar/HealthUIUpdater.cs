using TMPro;
using UnityEngine;


public class HealthUIUpdater : MonoBehaviour
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