using ArcadeVP;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private WaypointCircuit circuit;
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool smoothMovement = true;

    private float _currentDistance;
    private int _lastNearestPoint;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        if (circuit == null)
            circuit = GetComponent<WaypointCircuit>();

        InitializeStartingPosition();
    }

    private void InitializeStartingPosition()
    {
        if (circuit.Waypoints.Length == 0) return;

        _currentDistance = 0f;
        _lastNearestPoint = 0;
        _transform.position = circuit.Waypoints[0].position;
    }

    private void Update()
    {
        if (circuit == null || circuit.Waypoints.Length < 2) return;

        UpdateDistance();
        UpdatePosition();
    }

    private void UpdateDistance()
    {
        _currentDistance += speed * Time.deltaTime;
    }

    private void UpdatePosition()
    {
        if (smoothMovement)
        {
            _transform.position = circuit.GetRoutePosition(_currentDistance);
        }
        else
        {
            _transform.position = GetLinearPosition(ref _currentDistance);
        }
    }

    private Vector3 GetLinearPosition(ref float distance)
    {
        int segmentIndex = FindNearestSegmentIndex(distance);
        float segmentStart = circuit.distances[segmentIndex];
        float segmentEnd = circuit.distances[segmentIndex + 1];
        float t = Mathf.InverseLerp(segmentStart, segmentEnd, distance);

        return Vector3.Lerp(
            circuit.points[segmentIndex],
            circuit.points[segmentIndex + 1],
            t
        );
    }

    private int FindNearestSegmentIndex(float distance)
    {
        distance = Mathf.Repeat(distance, circuit.Length);

        int low = _lastNearestPoint;
        int high = circuit.distances.Length - 1;

        while (low < high)
        {
            int mid = (low + high) / 2;
            if (circuit.distances[mid] < distance)
                low = mid + 1;
            else
                high = mid;
        }

        _lastNearestPoint = low - 1;
        return Mathf.Clamp(low - 1, 0, circuit.distances.Length - 2);
    }
}