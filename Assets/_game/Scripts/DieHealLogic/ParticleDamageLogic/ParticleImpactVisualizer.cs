using UnityEngine;
using YG;

[RequireComponent(typeof(DamageHandler))]
public class ParticleImpactVisualizer : ParticleDamageReceiver
{
    [SerializeField] private EffectOnScreenUIApplier _desktopEffects;
    [SerializeField] private EffectOnScreenUIApplier _mobileEffects;

    private EffectOnScreenUIApplier _activeEffects;
    private bool _effectsInitialized;

    protected override void Awake()
    {
        base.Awake();
        InitializePlatformEffects();
    }

    private void InitializePlatformEffects()
    {
        bool isMobile = YandexGame.EnvironmentData.isMobile;
        _activeEffects = isMobile ? _mobileEffects : _desktopEffects;
        _effectsInitialized = _activeEffects != null;

        if (!_effectsInitialized)
        {
            Debug.LogError($"Effect applier not found for {(isMobile ? "mobile" : "desktop")} platform");
            enabled = false;
        }
    }

    protected override void ProcessDirtImpact()
    {
        if (_effectsInitialized)
            _activeEffects.ShowDirtSmudge();
    }

    protected override void ApplyDamage(float damageAmount, IWeapon weapon)
    {
        base.ApplyDamage(damageAmount, weapon);

        if (_effectsInitialized && IsBulletWeapon(weapon))
        {
            _activeEffects.ShowBulletHole();
        }
    }

    private bool IsBulletWeapon(IWeapon weapon)
    {
        return weapon is SmartWeapon ||
              (weapon is StraightShootingWeapon straight && straight.IsBulletWeapon);
    }

    private void OnParticleCollision(GameObject other)
    {
        var particleSystem = other.GetComponent<ParticleSystem>();
        if (particleSystem != null)
            HandleParticleCollision(particleSystem);
    }
}






//public class ParticleImpactVisualizer : ParticleDamageReceiver
//{
//    private const string InvalidReferenceError = "[ParticleImpactVisualizer] Не назначен EffectOnScreenUIApplier!";

//    [SerializeField] private EffectOnScreenUIApplier _desktopEffectOnScreenUIApplier;
//    [SerializeField] private EffectOnScreenUIApplier _mobileEffectOnScreenUIApplier;

//    private EffectOnScreenUIApplier _currentEffectApplier;
//    private bool _isInitialized;

//    protected override void Awake()
//    {
//        base.Awake();

//        InitializePlatformComponents();
//        ValidateReferences();
//    }

//    private void InitializePlatformComponents()
//    {
//        bool isMobile = YandexGame.EnvironmentData.isMobile;
//        _currentEffectApplier = isMobile ? _mobileEffectOnScreenUIApplier : _desktopEffectOnScreenUIApplier;

//        if (_currentEffectApplier == null)
//        {
//            Debug.LogError("Не удалось инициализировать _currentEffectApplier. " +
//                          (isMobile ? "Мобильный" : "Десктопный") + " аппликатор эффектов не назначен.", this);
//            _isInitialized = false;
//        }
//        else
//        {
//            Debug.Log("Успешно инициализирован " + (isMobile ? "мобильный" : "десктопный") + " аппликатор эффектов.", this);
//            _isInitialized = true;
//        }
//    }

//    private void ValidateReferences()
//    {
//        if (!_isInitialized)
//        {
//            Debug.Log(InvalidReferenceError, this);
//            enabled = false;
//            return;
//        }
//    }

//    protected override void OnParticleCollision(GameObject other)
//    {
//        base.OnParticleCollision(other);

//        if (_isInitialized && other.TryGetComponent<DirtParticleMarker>(out _))
//        {
//            _currentEffectApplier.ShowDirtSmudge();
//        }
//    }

//    protected override void ApplyDamage(float damageAmount, IWeapon weapon)
//    {
//        base.ApplyDamage(damageAmount, weapon);

//        if (IsBulletWeapon(weapon))
//        {
//            if (_currentEffectApplier != null)
//            {
//                _currentEffectApplier.ShowBulletHole();
//            }
//        }
//    }

//    private bool IsBulletWeapon(IWeapon weapon)
//    {
//        return weapon switch
//        {
//            SmartWeapon => true,
//            StraightShootingWeapon straight => straight.IsBulletWeapon,
//            _ => false
//        };
//    }
//}