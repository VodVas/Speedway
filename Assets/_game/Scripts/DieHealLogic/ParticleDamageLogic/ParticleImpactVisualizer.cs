using Reflex.Attributes;
using UnityEngine;
using YG;

[RequireComponent(typeof(DamageHandler))]
public class ParticleImpactVisualizer : ParticleDamageReceiver
{
    private const string InvalidReferenceError = "[PlayerDamageReceiverForBulletUI] Не назначен EffectOnScreenUIApplier!";

    [Header("Platform References")]
    [Inject] private EffectOnScreenUIApplier _desktopEffectOnScreenUIApplier;
    [Inject] private EffectOnScreenUIApplier _mobileEffectOnScreenUIApplier;

    private EffectOnScreenUIApplier _currentEffectApplier;
    private bool _isInitialized;

    protected override void Awake()
    {
        base.Awake();
        InitializePlatformComponents();
        ValidateReferences();
    }

    private void InitializePlatformComponents()
    {
        bool isMobile = YandexGame.EnvironmentData.isMobile;
        _currentEffectApplier = isMobile ? _mobileEffectOnScreenUIApplier : _desktopEffectOnScreenUIApplier;
        _isInitialized = _currentEffectApplier != null;
    }

    private void ValidateReferences()
    {
        if (!_isInitialized)
        {
            Debug.LogError(InvalidReferenceError, this);
            enabled = false;
        }
    }

    protected override void OnParticleCollision(GameObject other)
    {
        base.OnParticleCollision(other);

        if (_isInitialized && other.TryGetComponent<DirtParticleMarker>(out _))
        {
            _currentEffectApplier.ShowDirtSmudge();
        }
    }

    protected override void ApplyDamage(float damageAmount)
    {
        base.ApplyDamage(damageAmount);

        if (!_isInitialized) return;

        IWeapon weapon = LastWeaponUsed;
        if (weapon == null) return;

        if (IsBulletWeapon(weapon))
        {
            _currentEffectApplier.ShowBulletHole();
        }
    }

    private bool IsBulletWeapon(IWeapon weapon)
    {
        return weapon switch
        {
            SmartWeapon => true,
            StraightShootingWeapon straight => straight.IsBulletWeapon,
            _ => false
        };
    }
}




//public class ParticleImpactVisualizer : ParticleDamageReceiver
//{
//    private const string InvalidReferenceError = "[PlayerDamageReceiverForBulletUI] Не назначен BulletHoleUI!";

//    [Inject] private EffectOnScreenUIApplier _desktopEffectOnScreenUIApplier = null;
//    [Inject] private EffectOnScreenUIApplier _mobileEffectOnScreenUIApplier = null;

//    protected override void Awake()
//    {
//        base.Awake();

//        if (_desktopEffectOnScreenUIApplier == null)
//        {
//            Debug.LogError(InvalidReferenceError, this);
//            enabled = false;
//            return;
//        }
//    }

//    protected override void OnParticleCollision(GameObject other)
//    {
//        base.OnParticleCollision(other);

//        if (other.TryGetComponent<DirtParticleMarker>(out _))
//        {
//            _desktopEffectOnScreenUIApplier.ShowDirtSmudge();
//        }
//    }

//    protected override void ApplyDamage(float damageAmount)
//    {
//        base.ApplyDamage(damageAmount);
//        IWeapon weapon = LastWeaponUsed;

//        if (weapon == null)
//        {
//            return;
//        }

//        if (IsBulletWeapon(weapon))
//        {
//            _desktopEffectOnScreenUIApplier.ShowBulletHole();
//        }
//    }

//    private bool IsBulletWeapon(IWeapon weapon)
//    {
//        if (weapon is SmartWeapon)
//        {
//            return true;
//        }

//        if (weapon is StraightShootingWeapon straight)
//        {
//            return straight.IsBulletWeapon;
//        }

//        return false;
//    }
//}