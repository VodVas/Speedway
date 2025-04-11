using ArcadeVP;
using System;
using UnityEngine;

public class VehicleSpeedDependentRotator : MonoBehaviour
{
    [Serializable]
    private enum VehicleRotationAxis { X, Y, Z }

    [SerializeField] private ArcadeVehicleController _carController;
    [SerializeField] private VehicleRotationAxis _rotationAxis = VehicleRotationAxis.X;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private bool _useLocalSpace = true;
    [SerializeField] private bool _invertDirection = false;

    private Transform _transform;
    private Vector3 _currentEulerAngles;
    private float _directionMultiplier = 1f;

    private void Awake()
    {
        _transform = transform;
        _currentEulerAngles = _useLocalSpace ? _transform.localEulerAngles : _transform.eulerAngles;
        _directionMultiplier = _invertDirection ? -1f : 1f;
    }

    private void Update()
    {
        if (_carController == null) return;

        float speed = _carController.carVelocity.z;
        float rotationAmount = speed * _rotationSpeed * _directionMultiplier * Time.deltaTime;

        switch (_rotationAxis)
        {
            case VehicleRotationAxis.X:
                _currentEulerAngles.x += rotationAmount;
                break;
            case VehicleRotationAxis.Y:
                _currentEulerAngles.y += rotationAmount;
                break;
            case VehicleRotationAxis.Z:
                _currentEulerAngles.z += rotationAmount;
                break;
        }

        if (_useLocalSpace)
        {
            _transform.localEulerAngles = _currentEulerAngles;
        }
        else
        {
            _transform.eulerAngles = _currentEulerAngles;
        }
    }
}