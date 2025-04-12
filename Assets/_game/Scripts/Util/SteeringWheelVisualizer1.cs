using UnityEngine;

public partial class SteeringWheelVisualizer : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }

    [Header("Rotation Settings")]
    [SerializeField] private RotationAxis _rotationAxis = RotationAxis.Z;
    [SerializeField] private bool _invertDirection = true;

    private void ApplySteeringRotation()
    {
        float targetAngle = Input.GetAxis("Horizontal") * _maxSteerAngle;
        _currentSteerAngle = Mathf.MoveTowards(
            _currentSteerAngle,
            targetAngle,
            _steerSpeed * Time.deltaTime * 100f
        );

        Vector3 localEuler = _cachedTransform.localEulerAngles;
        float finalAngle = _invertDirection ? -_currentSteerAngle : _currentSteerAngle;

        localEuler.x = (_rotationAxis == RotationAxis.X) ? finalAngle : localEuler.x;
        localEuler.y = (_rotationAxis == RotationAxis.Y) ? finalAngle : localEuler.y;
        localEuler.z = (_rotationAxis == RotationAxis.Z) ? finalAngle : localEuler.z;

        _cachedTransform.localEulerAngles = localEuler;
    }
}