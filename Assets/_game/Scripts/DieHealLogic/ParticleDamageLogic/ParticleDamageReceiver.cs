using UnityEngine;

[RequireComponent(typeof(DamageHandler), typeof(Vehicle))]
public class ParticleDamageReceiver : MonoBehaviour
{
    [SerializeField] private ParticleWeaponLinker _weaponLinker;

    private DamageHandler _damageHandler;
    private Vehicle _vehicle;
    private IDamageImpactListener[] _listeners;

    private void Awake()
    {
        _damageHandler = GetComponent<DamageHandler>();
        _vehicle = GetComponent<Vehicle>();
        _listeners = GetComponents<IDamageImpactListener>();
    }

    private void OnParticleCollision(GameObject other)
    {
        var particle = other.GetComponent<ParticleSystem>();
        if (!particle) return;

        if (_weaponLinker.TryGetWeapon(particle, out var weapon))
            ProcessWeaponImpact(weapon);
        else
            NotifyParticleImpact(particle);
    }

    private void ProcessWeaponImpact(IWeapon weapon)
    {
        if (weapon.OwnerVehicle == _vehicle) return;

        _damageHandler.TakeDamage(weapon.DamageAmount, weapon);
        NotifyWeaponImpact(weapon.DamageAmount, weapon);
    }

    private void NotifyWeaponImpact(float damage, IWeapon weapon)
    {
        foreach (var listener in _listeners)
            listener?.OnWeaponImpact(damage, weapon);
    }

    private void NotifyParticleImpact(ParticleSystem particle)
    {
        foreach (var listener in _listeners)
            listener?.OnParticleImpact(particle);
    }
}