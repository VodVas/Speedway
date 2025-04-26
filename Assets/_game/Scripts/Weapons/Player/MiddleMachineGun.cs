using UnityEngine;

public class MiddleMachineGun : ParticleWeapon
{
    [SerializeField] private ObjectContinuousShaker _shaker;

    private bool _isShakeActive;

    protected override void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayParticleEffect();

            if (_shaker != null)
            {
                _shaker.enabled = IsParticlePlay;
                _isShakeActive = IsParticlePlay;
            }
        }

        if (_isShakeActive && !IsParticlePlay)
        {
            _shaker.StopShakeImmediately();

            if (_shaker != null)
            {
                _isShakeActive = false;
            }
        }
    }
}