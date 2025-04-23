using UnityEngine;

[RequireComponent(typeof(ParticleSoundSynchronizer))]
public class SmartWeapon : ParticleWeapon, IWeapon
{
    private Transform _currentEnemy;

    protected override void Update()
    {
        base.Update();
        if (_currentEnemy != null)
        {
            Vector3 direction = _currentEnemy.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Vehicle _)) return;

        _currentEnemy = other.transform;

        if (!IsParticlePlay) PlayParticleEffect();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Vehicle _)) return;

        if (_currentEnemy == other.transform)
        {
            _currentEnemy = null;
            StopParticleEffect();
        }
    }
}