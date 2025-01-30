using UnityEngine;

public class ParticleDamageReceiver : MonoBehaviour
{
    private DamageHandler _damageHandler;



    private void Awake()
    {
        _damageHandler = GetComponent<DamageHandler>();

        OnAwake();
    }

    private void OnParticleCollision(GameObject other)
    {
        IWeapon weapon = FindWeapon(other);

        if (weapon != null)
        {
            ApplyDamage(weapon.DamageAmount);
            HandleAdditionalEffects(weapon);
        }
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void ApplyDamage(float damageAmount)
    {
        if (_damageHandler != null)
        {
            Debug.LogWarning("ApplyDamage");
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

    protected virtual void HandleAdditionalEffects(IWeapon weapon)
    {
    }
}