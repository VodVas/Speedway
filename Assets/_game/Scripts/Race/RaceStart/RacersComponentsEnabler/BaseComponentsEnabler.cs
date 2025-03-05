using Reflex.Attributes;
using UnityEngine;

public abstract class BaseComponentsEnabler : MonoBehaviour
{
    [SerializeField] private Collider[] _colliders;

    protected abstract bool IsBoss { get; }

    [Inject] private RaceStartTimeCounter _raceTimer;

    private void OnEnable()
    {
        if (_raceTimer != null)
        {
            if (IsBoss)
            {
                _raceTimer.BossStarted += OnRaceStarted;
            }
            else
            {
                _raceTimer.Started += OnRaceStarted;
            }
        }
    }

    private void OnDisable()
    {
        if (_raceTimer != null)
        {
            if (IsBoss)
            {
                _raceTimer.BossStarted -= OnRaceStarted;
            }
            else
            {
                _raceTimer.Started -= OnRaceStarted;
            }
        }
    }

    private void OnRaceStarted()
    {
        EnableComponents();
        EnableColliders();
    }

    protected void EnableColliders()
    {
        if (_colliders == null) return;
        for (int i = 0; i < _colliders.Length; i++)
        {
            if (_colliders[i] != null)
            {
                _colliders[i].enabled = true;
            }
        }
    }

    protected abstract void EnableComponents();
}