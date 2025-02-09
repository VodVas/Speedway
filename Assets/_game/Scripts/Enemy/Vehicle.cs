using UnityEngine;

[RequireComponent(typeof(VehiclesPartsSpawner))]
[RequireComponent(typeof(Health))]
public abstract class Vehicle : MonoBehaviour
{
    private VehiclesPartsSpawner _vehiclesPartsSpawner;
    private Health _health;
    private Vector3 _deathPosition;

    private void Awake()
    {
        _vehiclesPartsSpawner = GetComponent<VehiclesPartsSpawner>();
        _health = GetComponent<Health>();
    }

    public void SetPosition()
    {
        _deathPosition = transform.position;
    }

    public void Respawn()
    {
        if (_health != null)
        {
            _health.ResetValue();
        }

        transform.position = _deathPosition;
    }

    public void SpawnParts()
    {
        _vehiclesPartsSpawner.StartSpawn(transform.position, GetType());
    }
}