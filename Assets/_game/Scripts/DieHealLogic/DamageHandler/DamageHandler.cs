using System;
using UnityEngine;

public class DamageHandler : MonoBehaviour, IDamageable
{
    private Vehicle _vehicle;
    private IWeapon _lastWeaponUsed;

    public event Action<Vehicle, IWeapon> VehicleKilled;
    public event Action<Vehicle> VehicleTerminated;

    public Health Health { get; private set; }
    public bool IsDead => Health.Value <= 0f;

    private void Awake()
    {
        Health = GetComponent<Health>();
        _vehicle = GetComponent<Vehicle>();
    }

    public void TakeDamage(float damage)
    {
        TakeDamage(damage, null);
    }

    public void TakeDamage(float damage, IWeapon weapon)
    {
        if (damage < 0)
            damage = 0;

        _lastWeaponUsed = weapon;

        Health.ChangeHealth(-damage);

        if (IsDead)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        VehicleKilled?.Invoke(_vehicle, _lastWeaponUsed);
        VehicleTerminated?.Invoke(_vehicle);
    }
}