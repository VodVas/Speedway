using ArcadeVP;
using UnityEngine;
using Zenject;

public class ComponentsEnabler : MonoBehaviour
{
    [SerializeField] private Collider[] _colliders;
    [SerializeField] private MonoBehaviour[] _playerWeapon;

    [Inject] private RaceStartTimeCounter _raceStartTimeCounter;
    private ArcadeAiVehicleController _aiController;
    private ArcadeVehicleController _carController;

    private void Awake()
    {
        TryGetComponent(out _carController);
        TryGetComponent(out _aiController);
    }

    private void OnEnable()
    {
        if (_raceStartTimeCounter != null)
            _raceStartTimeCounter.Started += EnableComponent;
    }

    private void OnDisable()
    {
        if (_raceStartTimeCounter != null)
            _raceStartTimeCounter.Started -= EnableComponent;
    }

    private void EnableComponent()
    {
        if (_colliders.Length > 0)
        {
            foreach (var collider in _colliders)
            {
                collider.enabled = true;
            }
        }

        if (_playerWeapon.Length > 0)
        {
            foreach (var gameObjects in _playerWeapon)
            {
                gameObjects.enabled = true;
            }
        }

        if (_aiController != null)
        {
            _aiController.enabled = true;
        }

        if (_carController != null)
        {
            _carController.enabled = true;
        }
    }
}