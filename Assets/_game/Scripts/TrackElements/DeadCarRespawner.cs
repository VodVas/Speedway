using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCarRespawner : MonoBehaviour
{
    [SerializeField] private List<Vehicle> _vehicles = new List<Vehicle>();
    [SerializeField] private float _delayUntilRespawn = 3f;

    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delayUntilRespawn);

        foreach (var vehicle in _vehicles)
        {
            if (vehicle.TryGetComponent(out DamageHandler damageHandler))
            {
                damageHandler.VehicleTerminated += OnVehicleDied;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var vehicle in _vehicles)
        {
            if (vehicle != null && vehicle.TryGetComponent(out DamageHandler damageHandler))
            {
                damageHandler.VehicleTerminated -= OnVehicleDied;
            }
        }
    }

    public void AddVehicle(Vehicle vehicle)
    {
        if (!_vehicles.Contains(vehicle))
        {
            _vehicles.Add(vehicle);

            if (vehicle.TryGetComponent(out DamageHandler damageHandler))
            {
                damageHandler.VehicleTerminated += OnVehicleDied;
            }
        }
    }

    private void OnVehicleDied(Vehicle vehicle)
    {
        vehicle.SetPosition();

        if (vehicle.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
        }

        vehicle.gameObject.SetActive(false);

        StartCoroutine(DelayRespawn(vehicle));
    }

    private IEnumerator DelayRespawn(Vehicle vehicle)
    {
        if (!vehicle.TryStartRespawn())
            yield break;

        try
        {
            vehicle.SpawnParts();
            yield return _wait;

            vehicle.gameObject.SetActive(true);
            vehicle.Respawn();
        }
        finally
        {
            vehicle.FinishRespawn();
        }
    }
}