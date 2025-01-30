using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [field: SerializeField, Range(0, 100)] public float DamageAmount { get; private set; } = 10;
    [field: SerializeField] protected ParticleSystem ParticleShoot { get; private set; }

    public bool IsActive { get; private set; }

    private void Awake()
    {
        SetActive(true);
    }

    protected virtual void Update()
    {
        HandleShooting();

        if (IsActive)
        {
        }
    }

    public void SetActive(bool isActive)
    {
        if (IsActive != isActive)
        {
            IsActive = isActive;
        }

        if (IsActive == false)
        {
            StopParticleEffect();
        }
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