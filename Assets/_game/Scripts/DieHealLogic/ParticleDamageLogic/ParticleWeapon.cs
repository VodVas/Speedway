using UnityEngine;

public abstract class ParticleWeapon : Weapon
{
    [SerializeField] private ParticleSystem ParticleShoot;
    [SerializeField] private ParticleWeaponMarker _marker;

    protected bool IsParticlePlay { get; private set; }

    protected override void Awake()
    {
        if (ParticleShoot == null)
        {
            Debug.LogError("ParticleSystem reference is missing!", this);
            enabled = false;
            return;
        }

        if (_marker == null)
        {
            Debug.LogWarning("ParticleWeaponMarker reference is missing!", this);
            enabled = false;
            return;
        }
        else
        {
            _marker.SetWeapon(this);
        }

        IsParticlePlay = ParticleShoot.isPlaying;
    }

    protected override void Update()
    {
        HandleShooting();
        UpdateParticleState();
    }

    private void UpdateParticleState()
    {
        if (ParticleShoot != null)
        {
            IsParticlePlay = ParticleShoot.isPlaying;
        }
    }

    protected virtual void PlayParticleEffect()
    {
        if (ParticleShoot != null && !IsParticlePlay)
        {
            ParticleShoot.Play();
            IsParticlePlay = true;
        }
    }

    protected virtual void StopParticleEffect()
    {
        if (ParticleShoot != null && IsParticlePlay)
        {
            ParticleShoot.Stop();
            IsParticlePlay = false;
        }
    }
}