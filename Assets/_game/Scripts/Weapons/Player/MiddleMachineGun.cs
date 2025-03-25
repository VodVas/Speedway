using UnityEngine;

public class MiddleMachineGun : ParticleWeapon
{
    protected override void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayParticleEffect();
        }
    }
}