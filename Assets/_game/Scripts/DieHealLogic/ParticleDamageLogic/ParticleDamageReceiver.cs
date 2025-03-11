using UnityEngine;

public class ParticleDamageReceiver : MonoBehaviour
{
    private DamageHandler _damageHandler;
    protected IWeapon LastWeaponUsed;

    protected virtual void Awake()
    {
        _damageHandler = GetComponent<DamageHandler>();
    }

    protected virtual void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out ParticleWeaponMarker marker))
        {
            IWeapon weapon = marker.WeaponRef;

            if (weapon != null)
            {
                LastWeaponUsed = weapon;

                ApplyDamage(weapon.DamageAmount);
            }
        }
    }

    protected virtual void ApplyDamage(float damageAmount)
    {
        if (_damageHandler != null)
        {
            _damageHandler.TakeDamage(damageAmount);
        }
    }
}