using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [field: SerializeField, Range(0, 100)] public float DamageAmount { get; private set; } = 10;
    [field: SerializeField] protected ParticleSystem ParticleShoot { get; private set; }

    protected virtual void Update()
    {
        HandleShooting();
    }

    protected virtual void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayParticleEffect();
        }
    }

    protected virtual void PlayParticleEffect()
    {
        if (ParticleShoot.isPlaying == false)
        {
            ParticleShoot.Play();
        }
    }

    protected virtual void StopParticleEffect()
    {
        if (ParticleShoot.isPlaying)
        {
            ParticleShoot.Stop();
        }
    }
}