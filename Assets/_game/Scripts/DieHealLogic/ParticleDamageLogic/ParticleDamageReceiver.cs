using UnityEngine;

public class ParticleDamageReceiver : MonoBehaviour
{
    private DamageHandler _damageHandler;
    protected IWeapon LastWeaponUsed; //TODO: поменять на закрытое

    protected virtual void Awake()
    {
        _damageHandler = GetComponent<DamageHandler>();
    }

    private void OnParticleCollision(GameObject other)
    {
        IWeapon weapon = FindWeapon(other);

        if (weapon != null)
        {
            LastWeaponUsed = weapon;

            ApplyDamage(weapon.DamageAmount);
        }
    }

    protected virtual void ApplyDamage(float damageAmount)
    {
        if (_damageHandler != null)
        {
            _damageHandler.TakeDamage(damageAmount);
        }
    }

    private IWeapon FindWeapon(GameObject other)
    {
        Transform weaponTransform = other.transform;

        while (weaponTransform != null)
        {
            if (weaponTransform.TryGetComponent(out IWeapon weapon))
            {
                return weapon;
            }

            weaponTransform = weaponTransform.parent;
        }

        return null;
    }
}