using System;
using System.Collections;
using UnityEngine;

public class Detonator : MonoBehaviour, ITerminatable
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private MineExplosionMarker _mineExplosionMarker;
    [SerializeField] private float _delayAfterExplosion = 0.5f;

    private WaitForSeconds _wait;
    public event Action<ITerminatable> Terminated;

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
        if (!_explosion.isPlaying) _explosion.Play();
        yield return _wait;
        Terminate();
    }

    public void Terminate() => Terminated?.Invoke(this);
}