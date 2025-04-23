using System.Collections.Generic;
using UnityEngine;

public class ParticleWeaponLinker : MonoBehaviour
{
    [SerializeField] private WeaponParticlePair[] _linkedParticles;

    private Dictionary<ParticleSystem, IWeapon> _particleWeaponMap;

    private void Awake()
    {
        InitializeWeaponMap();
    }

    private void InitializeWeaponMap()
    {
        _particleWeaponMap = new Dictionary<ParticleSystem, IWeapon>(_linkedParticles.Length);

        for (int i = 0; i < _linkedParticles.Length; i++)
        {
            var pair = _linkedParticles[i];

            if (pair.ParticleSystem != null && pair.Weapon != null)
            {
                _particleWeaponMap[pair.ParticleSystem] = pair.Weapon;
            }
        }
    }

    public bool TryGetWeapon(ParticleSystem particle, out IWeapon weapon)
    {
        return _particleWeaponMap.TryGetValue(particle, out weapon);
    }
}