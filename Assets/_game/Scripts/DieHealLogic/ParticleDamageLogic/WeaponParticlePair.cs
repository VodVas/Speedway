using System;
using UnityEngine;
using VodVas.InterfaceSerializer;

[Serializable]
public sealed class WeaponParticlePair
{
    [SerializeField] private ParticleSystem _particle;
    [SerializeField, InterfaceConstraint(typeof(IWeapon))] private MonoBehaviour _iWeapon;

    public WeaponParticlePair(ParticleSystem particle, MonoBehaviour weapon)
    {
        if (!particle) throw new ArgumentNullException(nameof(particle));
        if (!(weapon is IWeapon)) throw new ArgumentException("Must implement IWeapon");

        _particle = particle;
        _iWeapon = weapon;
    }

    public ParticleSystem ParticleSystem => _particle;
    public IWeapon Weapon => _iWeapon as IWeapon;
}