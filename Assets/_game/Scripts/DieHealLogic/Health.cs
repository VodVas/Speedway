using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField, Range(1, 1000)] public float Max { get; private set; } = 100;
    [field: SerializeField] public float Value { get; private set; }

    public event Action<float> Changed;

    private void Awake()
    {
        Init(Max);
    }

    public void Init(float maxHealth)
    {
        if (maxHealth <= 0)
        {
            Debug.LogError("Max health must be greater than zero.");
            enabled = false;
            return;
        }

        Max = maxHealth;
        Value = Max;
        Changed?.Invoke(Value / Max);
    }

    public void ChangeHealth(float amount)
    {
        Value = Mathf.Clamp(Value + amount, 0f, Max);

        Changed?.Invoke(Value / Max);
    }

    public void ResetValue()
    {
        Init(Max);
    }
}