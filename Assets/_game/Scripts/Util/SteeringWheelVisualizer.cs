using UnityEngine;

public partial class SteeringWheelVisualizer : MonoBehaviour
{
    [SerializeField] private float _maxSteerAngle = 540f;
    [SerializeField] private float _steerSpeed = 6f;

    private Transform _cachedTransform;
    private float _currentSteerAngle;

    private void Awake() => _cachedTransform = transform;
    private void Update() => ApplySteeringRotation();
}