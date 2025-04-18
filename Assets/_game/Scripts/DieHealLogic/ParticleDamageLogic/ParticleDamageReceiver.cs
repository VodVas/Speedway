using System.Collections.Generic;
using UnityEngine;


public abstract class ParticleDamageReceiver : MonoBehaviour
{
    [SerializeField] private ParticleWeaponLinker _weaponLinker;
    [SerializeField] private ParticleSystem[] _dirtParticles;

    private DamageHandler _damageHandler;
    private Vehicle _vehicle;
    private HashSet<ParticleSystem> _dirtParticleSet;
    protected IWeapon LastWeaponUsed;

    protected virtual void Awake()
    {
        InitializeComponents();
        InitializeDirtSet();
    }

    private void InitializeComponents()
    {
        _damageHandler = GetComponent<DamageHandler>();
        _vehicle = GetComponent<Vehicle>();
    }

    private void InitializeDirtSet()
    {
        _dirtParticleSet = new HashSet<ParticleSystem>(_dirtParticles.Length);
        for (int i = 0; i < _dirtParticles.Length; i++)
        {
            if (_dirtParticles[i] != null)
                _dirtParticleSet.Add(_dirtParticles[i]);
        }
    }

    protected void HandleParticleCollision(ParticleSystem particle)
    {
        if (_weaponLinker.TryGetWeapon(particle, out IWeapon weapon))
        {
            ProcessWeaponImpact(weapon);
        }
        else if (_dirtParticleSet.Contains(particle))
        {
            ProcessDirtImpact();
        }
    }

    private void ProcessWeaponImpact(IWeapon weapon)
    {
        if (weapon.OwnerVehicle == _vehicle) return;

        LastWeaponUsed = weapon;
        ApplyDamage(weapon.DamageAmount, weapon);
    }

    protected virtual void ApplyDamage(float damageAmount, IWeapon weapon)
    {
        _damageHandler?.TakeDamage(damageAmount, weapon);
    }

    protected abstract void ProcessDirtImpact();
}



//public abstract class ParticleDamageReceiver : MonoBehaviour
//{
//    private DamageHandler _damageHandler;
//    private Vehicle _vehicle;
//    protected IWeapon LastWeaponUsed; //TODO: private

//    protected virtual void Awake()
//    {
//        _damageHandler = GetComponent<DamageHandler>();
//        _vehicle = GetComponent<Vehicle>();
//    }

//    protected virtual void OnParticleCollision(GameObject other)
//    {
//        if (other.TryGetComponent(out ParticleWeaponMarker marker))
//        {
//            IWeapon weapon = marker.WeaponRef;

//            if (weapon != null && weapon.OwnerVehicle != _vehicle)
//            {
//                LastWeaponUsed = weapon;
//                ApplyDamage(weapon.DamageAmount, weapon);
//            }
//        }
//    }

//    protected virtual void ApplyDamage(float damageAmount, IWeapon weapon)
//    {
//        if (_damageHandler != null)
//        {
//            _damageHandler.TakeDamage(damageAmount, weapon);
//        }
//    }
//}