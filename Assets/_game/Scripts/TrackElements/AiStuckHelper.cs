using ArcadeVP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStuckHelper : MonoBehaviour
{
    [SerializeField] private ArcadeAiVehicleController[] _vehicles = default;
    [Space(1)]
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
        _trackerMap = new Dictionary<ArcadeAiVehicleController, WaypointProgressTracker>();

        InitializeTrackers();
        InitializeTimers();
    }

    private void Start()
    {
        if (_vehicles.Length > 0)
        {
            StartCoroutine(CheckStuckRoutine());
        }
    }

    private void InitializeTrackers()
    {
        for (int i = 0; i < _vehicles.Length; i++)
        {
            ArcadeAiVehicleController vehicle = _vehicles[i];

            if (vehicle == null)
                continue;

            WaypointProgressTracker tracker = vehicle.GetComponent<WaypointProgressTracker>();

            if (tracker != null)
            {
                _trackerMap[vehicle] = tracker;
            }
        }
    }

    private void InitializeTimers()
    {
        _stuckTimers = new float[_vehicles.Length];
        for (int i = 0; i < _stuckTimers.Length; i++)
        {
            _stuckTimers[i] = 0f;
        }
    }

    private IEnumerator CheckStuckRoutine()
    {
        while (true)
        {
            for (int i = 0; i < _vehicles.Length; i++)
            {
                ArcadeAiVehicleController vehicle = _vehicles[i];

                if (vehicle == null || !vehicle.gameObject.activeSelf)
                    continue;

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
                }
                else
                {
                    _stuckTimers[i] = 0f;
                }

                if (_stuckTimers[i] >= _stuckTimeout)
                {
                    TeleportStuckVehicle(vehicle);
                    _stuckTimers[i] = 0f;
                }
            }

            yield return _wait;
        }
    }

    private void TeleportStuckVehicle(ArcadeAiVehicleController vehicle)
    {
        if (!_trackerMap.TryGetValue(vehicle, out WaypointProgressTracker tracker))
            return;

        WaypointCircuit circuit = tracker.Circuit;

        if (circuit == null || circuit.Waypoints == null || circuit.Waypoints.Length == 0)
            return;

        Transform nearestWp = FindNearestWaypoint(vehicle.transform.position, circuit.Waypoints);

        if (nearestWp == null)
            return;

        ResetVehiclePhysics(vehicle);
        SetVehiclePositionAndRotation(vehicle, nearestWp, _offsetY, tracker);
    }

    private Transform FindNearestWaypoint(Vector3 position, Transform[] waypoints)
    {
        Transform nearest = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform waypoint = waypoints[i];

            if (waypoint == null)
                continue;

            float sqrDistance = (waypoint.position - position).sqrMagnitude;

            if (sqrDistance < minDistance)
            {
                minDistance = sqrDistance;
                nearest = waypoint;
            }
        }
        return nearest;
    }

    private void ResetVehiclePhysics(ArcadeAiVehicleController vehicle)
    {
        vehicle.rb.velocity = Vector3.zero;
        vehicle.rb.angularVelocity = Vector3.zero;
        vehicle.rb.useGravity = false;
    }

    private void SetVehiclePositionAndRotation(ArcadeAiVehicleController vehicle, Transform waypoint, float offsetY, WaypointProgressTracker tracker)
    {
        Vector3 position = waypoint.position + Vector3.up * offsetY;
        Quaternion rotation = CalculateRespawnRotation(vehicle, tracker, waypoint);

        vehicle.transform.position = position;
        vehicle.carBody.position = position;
        vehicle.rb.position = position;
        vehicle.carBody.rotation = rotation;
        vehicle.rb.rotation = rotation;
    }

    private Quaternion CalculateRespawnRotation(ArcadeAiVehicleController vehicle, WaypointProgressTracker tracker, Transform fallbackWaypoint)
    {
        if (tracker.Circuit == null)
            return fallbackWaypoint.rotation;

        WaypointCircuit.RoutePoint routePoint = tracker.Circuit.GetRoutePoint(tracker.progressDistance);

        if (routePoint.direction.sqrMagnitude > Mathf.Epsilon)
        {
            return Quaternion.LookRotation(routePoint.direction, Vector3.up);
        }
        else
        {
            return fallbackWaypoint.rotation;
        }
    }
}