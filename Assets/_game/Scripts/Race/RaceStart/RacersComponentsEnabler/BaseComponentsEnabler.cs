using UnityEngine;
using Zenject;

public abstract class BaseComponentsEnabler : MonoBehaviour
{
    [SerializeField] private Collider[] _colliders;

    [Inject] protected RaceStartTimeCounter _raceStartTimeCounter;

    protected virtual void Awake()
    {
    }

    private void OnEnable()
    {
        if (_raceStartTimeCounter != null)
            _raceStartTimeCounter.Started += EnableComponents;
    }

    private void OnDisable()
    {
        if (_raceStartTimeCounter != null)
            _raceStartTimeCounter.Started -= EnableComponents;
    }

    protected void EnableColliders()
    {
        if (_colliders.Length > 0)
        {
            foreach (var collider in _colliders)
            {
                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
        }
    }

    protected abstract void EnableComponents();
}