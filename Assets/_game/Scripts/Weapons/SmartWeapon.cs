using UnityEngine;

[RequireComponent(typeof(ParticleSoundSynchronizer))]
public class SmartWeapon : ParticleWeapon
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
        if (!ParticleShoot.isPlaying) PlayParticleEffect();
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





//public class SmartWeapon : ParticleWeapon
//{
//    private Transform _currentEnemy;
//    private ShootSoundPlayer _shootSoundPlayer;

//    protected override void Awake()
//    {
//        base.Awake();
//        _shootSoundPlayer = GetComponent<ShootSoundPlayer>();
//    }

//    protected override void Update()
//    {
//        if (_currentEnemy != null)
//        {
//            transform.LookAt(_currentEnemy);

//            Vector3 direction = _currentEnemy.position - transform.position;
//            //direction.y = 0;
//            transform.rotation = Quaternion.LookRotation(direction);
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.TryGetComponent(out Vehicle _))
//        {
//            _currentEnemy = other.transform;

//            if (ParticleShoot.isPlaying == false)
//            {
//                _shootSoundPlayer.PlaySound();
//                PlayParticleEffect();
//            }
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.TryGetComponent(out Vehicle _))
//        {
//            if (_currentEnemy == other.transform)
//            {
//                _currentEnemy = null;

//                _shootSoundPlayer.StopSound();
//                StopParticleEffect();
//            }
//        }
//    }
//}