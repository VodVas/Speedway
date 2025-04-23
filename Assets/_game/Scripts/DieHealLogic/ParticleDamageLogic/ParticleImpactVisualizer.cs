using UnityEngine;
using YG;

[RequireComponent(typeof(ParticleDamageReceiver))]
public class ParticleImpactVisualizer : MonoBehaviour, IDamageImpactListener
{
    [SerializeField] private EffectOnScreenUIApplier _desktopEffects;
    [SerializeField] private EffectOnScreenUIApplier _mobileEffects;
    [SerializeField] private ParticleSystem _dirtParticle;

    private EffectOnScreenUIApplier _activeEffects;
    private bool _isMobile;

    private void Awake()
    {
        _isMobile = YandexGame.EnvironmentData.isMobile;
        _activeEffects = _isMobile ? _mobileEffects : _desktopEffects;

        if (_activeEffects == null)
            Debug.LogError($"Effect applier missing for {(_isMobile ? "mobile" : "desktop")}");
    }

    public void OnWeaponImpact(float damage, IWeapon weapon)
    {
        if (IsValidBulletImpact(weapon))
            _activeEffects?.ShowBulletHole();
    }

    public void OnParticleImpact(ParticleSystem particle)
    {
        if (_dirtParticle != null && particle == _dirtParticle)
            _activeEffects?.ShowDirtSmudge();
    }

    private bool IsValidBulletImpact(IWeapon weapon)
    {
        return weapon is SmartWeapon ||
             (weapon is StraightShootingWeapon straightWeapon && straightWeapon.IsBulletWeapon);
    }
}