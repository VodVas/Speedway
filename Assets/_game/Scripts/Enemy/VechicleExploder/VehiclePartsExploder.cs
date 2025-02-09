using System;
using System.Collections;
using UnityEngine;

public class VehiclePartsExploder : MonoBehaviour, ITerminatable, IResettable
{
    [SerializeField] private float _delay = 2f;
    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private Vector2 _horizontalExplosionRange = new Vector2(-1f, 1f);
    [SerializeField] private Vector2 _verticalExplosionRange = new Vector2(0f, 1f);

    private Rigidbody[] _childRigidbodies;
    private int _childCount;
    private bool _isTerminated = false;
    private WaitForSeconds _wait;

    public event Action<ITerminatable> Terminated;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
        _childRigidbodies = GetComponentsInChildren<Rigidbody>();
        _childCount = _childRigidbodies.Length;
    }

    private void OnEnable()
    {
        ExplodeChildObjects();
        StartCoroutine(DelayingDelete());
    }

    public void ResetState()
    {
        _isTerminated = false;
    }

    private Vector3 GetExplosionDirection()
    {
        float randomX = UnityEngine.Random.Range(_horizontalExplosionRange.x, _horizontalExplosionRange.y);
        float randomY = UnityEngine.Random.Range(_verticalExplosionRange.x, _verticalExplosionRange.y);
        float randomZ = UnityEngine.Random.Range(_horizontalExplosionRange.x, _horizontalExplosionRange.y);

        return new Vector3(randomX, randomY, randomZ).normalized;
    }

    private void ExplodeChildObjects()
    {
        for (int i = 0; i < _childCount; i++)
        {
            if (_childRigidbodies[i] != null)
            {
                Vector3 explosionDirection = GetExplosionDirection();

                _childRigidbodies[i].AddForce(explosionDirection * _explosionForce, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator DelayingDelete()
    {
        yield return _wait;

        if (!_isTerminated)
        {
            _isTerminated = true;

            Terminated?.Invoke(this);
        }
    }
}