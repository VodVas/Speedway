using System;
using System.Collections;
using UnityEngine;

public class Detonator : MonoBehaviour, ITerminatable, IWeapon
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private float _delayAfterExplosion = 0.5f;

    private WaitForSeconds _wait;

    public event Action<ITerminatable> Terminated;

    [field: SerializeField, Range(0, 100)] public float DamageAmount { get; private set; } = 25;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delayAfterExplosion);
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

        Terminated?.Invoke(this);
    }
}