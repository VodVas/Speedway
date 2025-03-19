using UnityEngine;

[RequireComponent(typeof(ParticleSoundSynchronizer))]
public class StraightShootingWeapon : ParticleWeapon
{
    [field: SerializeField] public bool IsBulletWeapon { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Vehicle _)) return;
        if (!ParticleShoot.isPlaying) PlayParticleEffect();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Vehicle _)) return;
        if (ParticleShoot.isPlaying) StopParticleEffect();
    }
}



//public class StraightShootingWeapon : ParticleWeapon
//{
//    private ShootSoundPlayer _shootSoundPlayer;

//    [field: SerializeField] public bool IsBulletWeapon { get; private set; } = false;

//    protected override void Awake()
//    {
//        base.Awake();
//        _shootSoundPlayer = GetComponent<ShootSoundPlayer>();
//    }

//    protected override void Update()
//    {
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.TryGetComponent(out Vehicle _))
//        {
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
//            if (ParticleShoot.isPlaying)
//            {
//                _shootSoundPlayer.StopSound();
//                StopParticleEffect();
//            }
//        }
//    }
//}