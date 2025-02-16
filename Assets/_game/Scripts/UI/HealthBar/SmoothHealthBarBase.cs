using System.Collections;
using UnityEngine;

public abstract class SmoothHealthBarBase : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _smoothSpeed = 1f;

    private float _currentValue;
    private float _targetValue;
    private Coroutine _fillingCoroutine;

    private void Awake()
    {
        if (_health != null)
        {
            _currentValue = CalculateNormalizedHealth(_health);
            _targetValue = _currentValue;
            SetInstantValue(_currentValue);
        }
        else
        {
            _currentValue = 0f;
            _targetValue = 0f;
            SetInstantValue(0f);
        }
    }

    private void OnEnable()
    {
        if (_health != null)
        {
            _health.Changed += OnHealthChanged;
            UpdateHealthBarInstant();
        }
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.Changed -= OnHealthChanged;
        }

        if (_fillingCoroutine != null)
        {
            StopCoroutine(_fillingCoroutine);

            _fillingCoroutine = null;
        }
    }

    public bool BindHealth(Health newHealth)
    {
        if (newHealth == null)
        {
            Debug.LogError("[SmoothHealthBarBase] BindHealth: newHealth is null.", this);
            enabled = false;
            return false;
        }

        if (_health != null)
        {
            _health.Changed -= OnHealthChanged;
        }

        _health = newHealth;

        if (isActiveAndEnabled)
        {
            _health.Changed += OnHealthChanged;
            ApplyInstantUpdate();
        }

        return true;
    }

    private void ApplyInstantUpdate()
    {
        _currentValue = CalculateNormalizedHealth(_health);
        _targetValue = _currentValue;
        SetInstantValue(_currentValue);
    }

    private float CalculateNormalizedHealth(Health health)
    {
        if (health.Max <= 0f)
        {
            Debug.LogError("[SmoothHealthBarBase] Health.Max <= 0 Ч некорректное значение!", this);
            enabled = false;
            return 0f;
        }

        return health.Value / health.Max;
    }

    private void UpdateHealthBarInstant()
    {
        _currentValue = _health.Value / _health.Max;
        _targetValue = _currentValue;

        SetInstantValue(_currentValue);
    }


    private void OnHealthChanged(float normalized)
    {
        _targetValue = normalized;

        if (_fillingCoroutine != null)
        {
            StopCoroutine(_fillingCoroutine);
        }

        _fillingCoroutine = StartCoroutine(FillingRoutine());
    }

    private IEnumerator FillingRoutine()
    {
        while (Mathf.Abs(_currentValue - _targetValue) > Mathf.Epsilon)
        {
            _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, _smoothSpeed * Time.deltaTime);

            SetDisplayValue(_currentValue);

            yield return null;
        }

        _currentValue = _targetValue;

        SetDisplayValue(_currentValue);

        _fillingCoroutine = null;
    }

    protected abstract void SetInstantValue(float value);

    protected abstract void SetDisplayValue(float value);
}