using System.Collections;
using UnityEngine;

public abstract class SmoothHealthBarBase : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _smoothSpeed = 0.3f;

    private float _currentValue;
    private float _targetValue;
    private Coroutine _fillingCoroutine;

    private void Awake()
    {
        _currentValue = _health.Value / _health.Max;
        _targetValue = _currentValue;

        SetInstantValue(_currentValue);
    }

    private void OnEnable()
    {
        _health.Changed += OnHealthChanged;

        UpdateHealthBarInstant();
    }

    private void UpdateHealthBarInstant()
    {
        _currentValue = _health.Value / _health.Max;
        _targetValue = _currentValue;

        SetInstantValue(_currentValue);
    }

    private void OnDisable()
    {
        _health.Changed -= OnHealthChanged;

        if (_fillingCoroutine != null)
        {
            StopCoroutine(_fillingCoroutine);

            _fillingCoroutine = null;
        }
    }

    protected abstract void SetInstantValue(float value);

    protected abstract void SetDisplayValue(float value);

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
}