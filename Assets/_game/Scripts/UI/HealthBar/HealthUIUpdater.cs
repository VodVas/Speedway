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
            Debug.LogError("[HealthUIUpdater] ����� ��� �������� �� ��������!", this);
            enabled = false;
            return;
        }

        // ���� _health ��� �������� � ���������� � ����������
        if (_health != null)
        {
            SubscribeHealth(_health);
        }
    }

    public void AssignTargetHealth(Health newHealth)
    {
        if (newHealth == null)
        {
            Debug.LogError("[HealthUIUpdater] �������� null � AssignTargetHealth!", this);
            enabled = false;
            return;
        }

        // ������������ �� �������
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