using UnityEngine;

public abstract class ParticleWeapon : Weapon
{
    [SerializeField] private ParticleSystem ParticleShoot;
    [SerializeField] private ParticleWeaponMarker _marker;

    protected bool IsParticlePlay => ParticleShoot.isPlaying;

    protected override void Awake()
    {
        if (ParticleShoot != null)
        {
            _marker.SetWeapon(this);
        }
    }

    protected override void Update()
    {
        HandleShooting();
    }

    protected virtual void PlayParticleEffect()
    {
        if (ParticleShoot != null && !ParticleShoot.isPlaying)
        {
            ParticleShoot.Play();
        }
    }

    protected virtual void StopParticleEffect()
    {
        if (ParticleShoot != null && ParticleShoot.isPlaying)
        {
            ParticleShoot.Stop();
        }
    }
}