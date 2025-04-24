
using UnityEngine;

public interface IDamageImpactListener
{
    void OnWeaponImpact(float damage, IWeapon weapon);
    void OnParticleImpact(ParticleSystem particle);
}