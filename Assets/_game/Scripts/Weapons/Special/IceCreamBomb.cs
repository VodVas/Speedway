using System;
using System.Collections;
using UnityEngine;

public class IceCreamBomb : MonoBehaviour, ITerminatable, IWeapon
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private float _delayBeforeExplosion = 3f;
    [SerializeField] private float _delayAfterExplosion = 3f;

    private WaitForSeconds _waitBeforeExplosion;
    private WaitForSeconds _waitAfterExplosion;

    public event Action<ITerminatable> Terminated;

    [field: SerializeField, Range(0, 100)] public float DamageAmount { get; private set; } = 25;
    public Vehicle OwnerVehicle
    {
        get { return GetComponentInParent<Vehicle>(); }
    }

    private void Awake()
    {
        _waitBeforeExplosion = new WaitForSeconds(_delayBeforeExplosion);
        _waitAfterExplosion = new WaitForSeconds(_delayAfterExplosion);
    }

    private void OnEnable()
    {
        StartCoroutine(DelayingExplosion());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            StartCoroutine(DelayingExplosion());
        }
    }

    private IEnumerator DelayingExplosion()
    {
        yield return _waitBeforeExplosion;

        if (_explosion.isPlaying == false)
        {
            _explosion.Play();
        }

        yield return _waitAfterExplosion;
        Terminate();
        //Terminated?.Invoke(this);
    }

    public void Terminate()
    {
        Terminated?.Invoke(this);
    }
}