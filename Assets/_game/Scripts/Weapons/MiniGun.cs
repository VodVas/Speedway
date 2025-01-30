using UnityEngine;

[RequireComponent(typeof(ObjectCycleRotator))]
public class MiniGun : Weapon
{
    [SerializeField] private Transform _barrels;
    [SerializeField] private float _rotateSpeedX = 0f;
    [SerializeField] private float _rotateSpeedY = 0f;
    [SerializeField] private float _rotateSpeedZ = 0f;

    private ObjectCycleRotator _rotator;

    private void Awake()
    {
        _rotator = GetComponent<ObjectCycleRotator>();
    }

    protected override void Update()
    {
        base.Update();

        if (ParticleShoot.isPlaying)
        {
            _rotator?.StartRotate(_barrels, _rotateSpeedX, _rotateSpeedY, _rotateSpeedZ);
        }
        else
        {
            _rotator?.StartRotate(_barrels);
        }
    }
}