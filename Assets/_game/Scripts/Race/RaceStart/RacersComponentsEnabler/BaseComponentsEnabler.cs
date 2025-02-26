using Reflex.Attributes;
using UnityEngine;

public abstract class BaseComponentsEnabler : MonoBehaviour
{
    [SerializeField] private Collider[] _colliders;

    [Inject] private RaceStartTimeCounter _raceTimer;

    [Inject]
    private void InjectDependencies(RaceStartTimeCounter raceTimer)
    {
        _raceTimer = raceTimer;
        _raceTimer.Started += OnRaceStarted;
    }

    private void OnDisable()
    {
        if (_raceTimer != null)
        {
            _raceTimer.Started -= OnRaceStarted;
        }
    }

    private void OnRaceStarted()
    {
        EnableComponents();
        EnableColliders();
    }

    protected void EnableColliders()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = true;
        }
    }

    protected abstract void EnableComponents();
}