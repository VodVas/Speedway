using UnityEngine;

public class StraightShootingWeapon : Weapon
{
    [field: SerializeField] public bool IsMediumMachineGun { get; private set; } = false;

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