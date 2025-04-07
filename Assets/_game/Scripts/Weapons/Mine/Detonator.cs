using System;
using System.Collections;
using UnityEngine;

public class Detonator : MonoBehaviour, ITerminatable, IWeapon
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private ParticleWeaponMarker _explosionMarker;
    [SerializeField] private float _delayAfterExplosion = 0.5f;

    private WaitForSeconds _wait;

    public event Action<ITerminatable> Terminated;

    [field: SerializeField, Range(0, 100)] public float DamageAmount { get; private set; } = 25;

    public Vehicle OwnerVehicle
    {
        get { return GetComponentInParent<Vehicle>(); }
    }

    private void Awake()
    {
        _wait = new WaitForSeconds(_delayAfterExplosion);
    }

    private void OnEnable()
    {
        if (_explosionMarker != null)
        {
            _explosionMarker.SetWeapon(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Vehicle _))
        {
            StartCoroutine(DelayingExplosion());
        }
    }

    private IEnumerator DelayingExplosion()
    {
        if (_explosion.isPlaying == false)
        {
            _explosion.Play();
        }

        yield return _wait;
        Terminate();
        //Terminated?.Invoke(this);
    }

    public void Terminate()
    {
        Terminated?.Invoke(this);
    }
}