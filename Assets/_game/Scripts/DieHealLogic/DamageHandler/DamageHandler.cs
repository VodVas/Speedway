using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageHandler : MonoBehaviour, IDamageable
{

    private Vehicle _vehicle;

    public event Action<Vehicle> Died;

    public Health Health { get; private set; }

    public bool IsDead => Health.Value <= 0f;

    protected virtual void Awake()
    {
        Health = GetComponent<Health>();
        _vehicle = GetComponent<Vehicle>();
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            damage = 0;
        }

        Health.ChangeHealth(-damage);

        if (IsDead)
        {
            OnDeath();
        }
    }

    protected void ResetHealth()
    {
        Health.Init(Health.Max);
    }

    protected virtual void OnDeath()
    {
        Died?.Invoke(_vehicle);
    }
}