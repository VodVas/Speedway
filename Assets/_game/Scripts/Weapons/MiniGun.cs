using UnityEngine;

[RequireComponent(typeof(ObjectCycleRotator))]
public class MiniGun : ParticleWeapon
{
    [SerializeField] private Transform _barrels;
    [SerializeField] private float _activeRotateSpeedX = 500f;
    [SerializeField] private float _activeRotateSpeedY = 0f;
    [SerializeField] private float _activeRotateSpeedZ = 0f;

    private ObjectCycleRotator _rotator;
    private Vector3 _baseRotationSpeeds;

    protected override void Awake()
    {
        base.Awake();
        _rotator = GetComponent<ObjectCycleRotator>();

        _baseRotationSpeeds = new Vector3(_rotator.SpeedX, _rotator.SpeedY, _rotator.SpeedZ);
    }

    protected override void Update()
    {
        base.Update();

       // if (ParticleShoot.isPlaying)
        if (IsParticlePlay)
        {
            _rotator.SetRotationSpeeds(_activeRotateSpeedX, _activeRotateSpeedY, _activeRotateSpeedZ);
        }
        else
        {
            _rotator.SetRotationSpeeds(_baseRotationSpeeds.x, _baseRotationSpeeds.y, _baseRotationSpeeds.z);
        }
    }
}




//public class MiniGun : ParticleWeapon
//{
//    [SerializeField] private Transform _barrels;
//    [SerializeField] private float _rotateSpeedX = 0f;
//    [SerializeField] private float _rotateSpeedY = 0f;
//    [SerializeField] private float _rotateSpeedZ = 0f;

//    private ObjectCycleRotator _rotator;

//    protected override void Awake()
//    {
//        base.Awake();

//        _rotator = GetComponent<ObjectCycleRotator>();
//    }

//    protected override void Update()
//    {
//        base.Update();

//        if (ParticleShoot.isPlaying)
//        {
//            _rotator?.StartRotate(_barrels, _rotateSpeedX, _rotateSpeedY, _rotateSpeedZ);
//        }
//        else
//        {
//            _rotator?.StartRotate(_barrels);
//        }
//    }
//}