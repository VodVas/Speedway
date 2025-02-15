using TMPro;
using UnityEngine;

public class HealthUIUpdater : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private TextMeshProUGUI _healthText;

    private void Awake()
    {
        _health.Changed += UpdateHealthDisplay;

        UpdateHealthDisplay(_health.Value / _health.Max);
    }

    private void OnDisable()
    {
        _health.Changed -= UpdateHealthDisplay;
    }

    private void UpdateHealthDisplay(float healthPercentage)
    {
        _healthText.text = $"{_health.Value}";
    }
}