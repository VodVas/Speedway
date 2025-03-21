using UnityEngine;

public class RocketLauncher : ParticleWeapon
{
    protected override void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //if (!ParticleShoot.isPlaying)
            if (!IsParticlePlay)
            {
                PlayParticleEffect();
            }
        }
    }
}