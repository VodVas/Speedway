using UnityEngine;

public class MiniGun : ParticleWeapon
{
    [SerializeField] private Transform _barrels;
    [SerializeField] private float _activeRotateSpeedX = 0f;
    [SerializeField] private float _activeRotateSpeedY = 0f;
    [SerializeField] private float _activeRotateSpeedZ = 0f;

    private ObjectCycleRotator _rotator;
    private Vector3 _baseRotationSpeeds;
    private bool _wasParticlePlaying;

    protected override void Awake()
    {
        base.Awake();

        if (_barrels == null)
        {
            Debug.LogError("Barrels reference is not set in MiniGun!");
            enabled = false;
            return;
        }

        if (_barrels.TryGetComponent(out ObjectCycleRotator objectCycleRotator))
        {
            _rotator = objectCycleRotator;
            _baseRotationSpeeds = new Vector3(_rotator.SpeedX, _rotator.SpeedY, _rotator.SpeedZ);
        }
        else
        {
            Debug.Log("ObjectCycleRotator component missing on barrels!", this);
            enabled = false;
            return;
        }

        _wasParticlePlaying = true;
    }

    protected override void Update()
    {
        base.Update();

        bool isParticlePlaying = IsParticlePlay;

        if (isParticlePlaying != _wasParticlePlaying)
        {
            if (isParticlePlaying)
            {
                _rotator.SetRotationSpeeds(_activeRotateSpeedX, _activeRotateSpeedY, _activeRotateSpeedZ);
            }
            else
            {
                _rotator.SetRotationSpeeds(_baseRotationSpeeds.x, _baseRotationSpeeds.y, _baseRotationSpeeds.z);
            }

            _wasParticlePlaying = isParticlePlaying;
        }
    }

    protected override void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayParticleEffect();
        }
    }
}








//public class MiniGun : ParticleWeapon
//{
//    [SerializeField] private Transform _barrels;
//    [SerializeField] private float _activeRotateSpeedX = 0f;
//    [SerializeField] private float _activeRotateSpeedY = 0f;
//    [SerializeField] private float _activeRotateSpeedZ = 0f;

//    private ObjectCycleRotator _rotator;
//    private Vector3 _baseRotationSpeeds;

//    protected override void Awake()
//    {
//        base.Awake();
//        _rotator = GetComponent<ObjectCycleRotator>();

//        _baseRotationSpeeds = new Vector3(_rotator.SpeedX, _rotator.SpeedY, _rotator.SpeedZ);
//    }

//    protected override void Update()
//    {
//        base.Update();

//        if (IsParticlePlay)
//        {
//            _rotator.SetRotationSpeeds(_activeRotateSpeedX, _activeRotateSpeedY, _activeRotateSpeedZ);
//        }
//        else
//        {
//            _rotator.SetRotationSpeeds(_baseRotationSpeeds.x, _baseRotationSpeeds.y, _baseRotationSpeeds.z);
//        }
//    }

//    protected override void HandleShooting()
//    {
//        if (Input.GetKeyDown(KeyCode.Mouse0))
//        {
//            PlayParticleEffect();
//        }
//    }
//}




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