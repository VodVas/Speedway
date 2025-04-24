using UnityEngine;

public class ParticleWeaponLinker : MonoBehaviour
{
    [SerializeField] private WeaponParticlePair[] _linkedPairs;

    private int[] _particleInstanceIDs;
    private ParticleSystem[] _particleSystems;
    private IWeapon[] _weapons;
    private int _count;

    private void Awake()
    {
        _count = _linkedPairs.Length;
        _particleInstanceIDs = new int[_count];
        _particleSystems = new ParticleSystem[_count];
        _weapons = new IWeapon[_count];

        for (int i = 0; i < _count; i++)
        {
            var pair = _linkedPairs[i];
            var go = pair.ParticleSystem.gameObject;

            _particleInstanceIDs[i] = go.GetInstanceID();
            _particleSystems[i] = pair.ParticleSystem;
            _weapons[i] = pair.Weapon;
        }
    }

    public bool TryGetWeapon(GameObject particleOwner, out IWeapon weapon, out ParticleSystem particle)
    {
        int searchID = particleOwner.GetInstanceID();

        for (int i = 0; i < _count; i++)
        {
            if (_particleInstanceIDs[i] == searchID)
            {
                weapon = _weapons[i];
                particle = _particleSystems[i];
                return true;
            }
        }

        weapon = null;
        particle = null;
        return false;
    }
}