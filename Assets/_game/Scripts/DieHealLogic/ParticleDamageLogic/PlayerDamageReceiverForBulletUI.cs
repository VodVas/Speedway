using UnityEngine;

[RequireComponent(typeof(DamageHandler))]
public class PlayerDamageReceiverForBulletUI : ParticleDamageReceiver
{
    private const string InvalidReferenceError = "[PlayerDamageReceiverForBulletUI] Не назначен BulletHoleUI!";

    [SerializeField] private BulletHoleUI _bulletHoleUI = null;

    protected override void Awake()
    {
        base.Awake();

        if (_bulletHoleUI == null)
        {
            Debug.LogError(InvalidReferenceError, this);
            enabled = false;
            return;
        }
    }

    protected override void ApplyDamage(float damageAmount)
    {
        base.ApplyDamage(damageAmount);
        IWeapon weapon = LastWeaponUsed;

        if (weapon == null)
        {
            return;
        }

        if (IsBulletWeapon(weapon))
        {
            _bulletHoleUI.ShowBulletHole();
        }
    }

    private bool IsBulletWeapon(IWeapon weapon)
    {
        if (weapon is SmartWeapon)
        {
            return true;
        }

        if (weapon is StraightShootingWeapon straight)
        {
            return straight.IsMediumMachineGun;
        }

        return false;
    }
}