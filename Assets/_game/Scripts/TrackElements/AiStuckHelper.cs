using ArcadeVP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStuckHelper : MonoBehaviour
{
    [SerializeField] private ArcadeAiVehicleController[] _vehicles = default;
    [SerializeField] private Transform[] _respawnPoints = default;
    [SerializeField] private float _offsetY = 5f;
    [SerializeField] private float _minSpeed = 20f;
    [SerializeField] private float _stuckTimeout = 5f;
    [SerializeField] private float _maxHeightToStuck = -10f;
    [SerializeField] private float _checkInterval = 0.5f;

    private float[] _stuckTimers;
    private Dictionary<ArcadeAiVehicleController, WaypointProgressTracker> _trackerMap;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_checkInterval);

        if (_vehicles == null || _vehicles.Length == 0)
        {
            Debug.LogError($"AiStuckHelper: поле _vehicles не заполнено или пустое.", this);
            enabled = false;
            return;
        }

        if (_respawnPoints == null || _respawnPoints.Length == 0)
        {
            Debug.LogWarning($"AiStuckHelper: в поле _respawnPoints нет точек респауна!", this);
        }

        _stuckTimers = new float[_vehicles.Length];

        for (int i = 0; i < _vehicles.Length; i++)
        {
            _stuckTimers[i] = 0f;
        }

        _trackerMap = new Dictionary<ArcadeAiVehicleController, WaypointProgressTracker>(_vehicles.Length);

        for (int i = 0; i < _vehicles.Length; i++)
        {
            ArcadeAiVehicleController ai = _vehicles[i];

            if (ai == null) continue;

            WaypointProgressTracker tracker = ai.GetComponent<WaypointProgressTracker>();

            if (tracker != null)
            {
                _trackerMap[ai] = tracker;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(CheckStuckRoutine());
    }

    private IEnumerator CheckStuckRoutine()
    {
        while (true)
        {
            for (int i = 0; i < _vehicles.Length; i++)
            {
                ArcadeAiVehicleController vehicle = _vehicles[i];

                if (vehicle == null) continue;

                if (vehicle.carBody.position.y < _maxHeightToStuck)
                {
                    TeleportStuckVehicle(vehicle);
                    _stuckTimers[i] = 0f;

                    continue;
                }

                float speed = vehicle.carBody.velocity.magnitude;

                if (speed < _minSpeed)
                {
                    _stuckTimers[i] += _checkInterval;

                    if (_stuckTimers[i] >= _stuckTimeout)
                    {
                        TeleportStuckVehicle(vehicle);
                        _stuckTimers[i] = 0f;
                    }
                }
                else
                {
                    _stuckTimers[i] = 0f;
                }
            }

            yield return _wait;
        }
    }

    private void TeleportStuckVehicle(ArcadeAiVehicleController vehicle)
    {
        Transform nearest = FindNearestRespawnPoint(vehicle.transform.position);

        vehicle.rb.velocity = Vector3.zero;
        vehicle.rb.angularVelocity = Vector3.zero;
        vehicle.carBody.velocity = Vector3.zero;
        vehicle.carBody.angularVelocity = Vector3.zero;
        vehicle.rb.useGravity = false;

        Quaternion stuckOrientation;

        if (_trackerMap.TryGetValue(vehicle, out WaypointProgressTracker tracker) && tracker != null)
        {
            if (tracker.Circuit != null)
            {
                float dist = tracker.progressDistance;
                WaypointCircuit.RoutePoint routePoint = tracker.Circuit.GetRoutePoint(dist);
                Vector3 forwardDir = routePoint.direction;

                if (forwardDir.sqrMagnitude < Mathf.Epsilon)
                {
                    stuckOrientation = nearest.rotation * Quaternion.Euler(45, 0, 0);
                }
                else
                {
                    stuckOrientation = Quaternion.LookRotation(forwardDir, Vector3.up);
                }
            }
            else
            {
                stuckOrientation = nearest.rotation * Quaternion.Euler(45, 0, 0);
            }
        }
        else
        {
            stuckOrientation = nearest.rotation * Quaternion.Euler(45, 0, 0);
        }

        vehicle.transform.position = nearest.position + Vector3.up * _offsetY;
        vehicle.carBody.position = nearest.position + Vector3.up * _offsetY;
        vehicle.carBody.rotation = stuckOrientation;

        //StartCoroutine(EnableGravityAfterDelay(vehicle));
    }

    private IEnumerator EnableGravityAfterDelay(ArcadeAiVehicleController vehicle)
    {
        yield return new WaitForSeconds(0.5f);
        vehicle.rb.useGravity = true;
    }

    private Transform FindNearestRespawnPoint(Vector3 position)
    {
        Transform nearest = null;
        float minDistSqr = float.MaxValue;

        for (int i = 0; i < _respawnPoints.Length; i++)
        {
            Transform respawn = _respawnPoints[i];
            if (respawn == null)
            {
                continue;
            }

            float distSqr = (respawn.position - position).sqrMagnitude;

            if (distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                nearest = respawn;
            }
        }

        return nearest;
    }
}