using UnityEngine;




public class StraightShootingWeapon : ParticleWeapon
{
    [field: SerializeField] public bool IsBulletWeapon { get; private set; } = false;

    protected override void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Vehicle _))
        {
            if (ParticleShoot.isPlaying == false)
            {
                PlayParticleEffect();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Vehicle _))
        {
            if (ParticleShoot.isPlaying)
            {
                StopParticleEffect();
            }
        }
    }
}