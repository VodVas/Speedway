using UnityEngine;

public class SmartWeapon : Weapon
{
    private Transform _currentEnemy;

    protected override void Update()
    {
        if (_currentEnemy != null)
        {
            transform.LookAt(_currentEnemy);

            Vector3 direction = _currentEnemy.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Vehicle _))
        {
            _currentEnemy = other.transform;

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
            if (_currentEnemy == other.transform)
            {
                _currentEnemy = null;

                StopParticleEffect();
            }
        }
    }
}