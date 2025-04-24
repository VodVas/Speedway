using UnityEngine;

[RequireComponent(typeof(DamageHandler), typeof(Vehicle))]
public class ParticleDamageReceiver : MonoBehaviour
{
    [SerializeField] private ParticleWeaponLinker _weaponLinker;

    private DamageHandler _damageHandler;
    private IDamageImpactListener[] _listeners;
    private int _listenerCount;

    private void Awake()
    {
        _damageHandler = GetComponent<DamageHandler>();
        _listeners = GetComponents<IDamageImpactListener>();
        _listenerCount = _listeners.Length;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (_weaponLinker.TryGetWeapon(other, out var weapon, out var particle))
        {
            ProcessWeaponImpact(weapon);
        }
        else if (other.TryGetComponent(out ParticleSystem fallbackParticle))
        {
            HandleFallbackImpact(fallbackParticle);
        }
    }

    private void HandleFallbackImpact(ParticleSystem particle)
    {
        var marker = particle.GetComponent<MineExplosionMarker>();

        if (marker != null)
        {
            _damageHandler.TakeDamage(marker.Damage);
        }

        NotifyParticleImpact(particle);
    }

    private void ProcessWeaponImpact(IWeapon weapon)
    {
        var owner = weapon.OwnerVehicle;
        if (owner != null && owner == GetComponent<Vehicle>()) return;

        _damageHandler.TakeDamage(weapon.DamageAmount, weapon);
        NotifyWeaponImpact(weapon.DamageAmount, weapon);
    }

    private void NotifyWeaponImpact(float damage, IWeapon weapon)
    {
        for (int i = 0; i < _listenerCount; i++)
        {
            _listeners[i]?.OnWeaponImpact(damage, weapon);
        }
    }

    private void NotifyParticleImpact(ParticleSystem particle)
    {
        for (int i = 0; i < _listenerCount; i++)
        {
            _listeners[i]?.OnParticleImpact(particle);
        }
    }
}





//using UnityEngine;

//[RequireComponent(typeof(DamageHandler), typeof(Vehicle))]
//public class ParticleDamageReceiver : MonoBehaviour
//{
//    [SerializeField] private ParticleWeaponLinker _weaponLinker;

//    private DamageHandler _damageHandler;
//    private Vehicle _vehicle;
//    private IDamageImpactListener[] _listeners;
//    private int _listenerCount;

//    private void Awake()
//    {
//        _damageHandler = GetComponent<DamageHandler>();
//        _vehicle = GetComponent<Vehicle>();
//        _listeners = GetComponents<IDamageImpactListener>();
//        _listenerCount = _listeners.Length;
//    }

//    private void OnParticleCollision(GameObject other)
//    {
//        if (_weaponLinker.TryGetWeapon(other, out var weapon, out var particle))
//        {
//            ProcessWeaponImpact(weapon);
//        }
//        else if (other.TryGetComponent(out ParticleSystem fallbackParticle))
//        {
//            if (fallbackParticle.TryGetComponent(out MineExplosionMarker marker))
//            {
//                _damageHandler.TakeDamage(marker.Damage, marker.Weapon);
//            }
//            else
//            {
//                NotifyParticleImpact(fallbackParticle);
//            }
//        }
//    }

//    //private void OnParticleCollision(GameObject other)
//    //{
//    //    if (_weaponLinker.TryGetWeapon(other, out var weapon, out var particle))
//    //    {
//    //        ProcessWeaponImpact(weapon);
//    //    }
//    //    else
//    //    {
//    //        var fallbackParticle = other.GetComponent<ParticleSystem>();
//    //        if (fallbackParticle != null)
//    //        {
//    //            NotifyParticleImpact(fallbackParticle);
//    //        }
//    //    }
//    //}

//    private void ProcessWeaponImpact(IWeapon weapon)
//    {
//        var owner = weapon.OwnerVehicle;

//        if (owner != null && owner == _vehicle) return;

//        var damage = weapon.DamageAmount;
//        _damageHandler.TakeDamage(damage, weapon);
//        NotifyWeaponImpact(damage, weapon);
//    }

//    private void NotifyWeaponImpact(float damage, IWeapon weapon)
//    {
//        for (int i = 0; i < _listenerCount; i++)
//        {
//            _listeners[i]?.OnWeaponImpact(damage, weapon);
//        }
//    }

//    private void NotifyParticleImpact(ParticleSystem particle)
//    {
//        for (int i = 0; i < _listenerCount; i++)
//        {
//            _listeners[i]?.OnParticleImpact(particle);
//        }
//    }
//}