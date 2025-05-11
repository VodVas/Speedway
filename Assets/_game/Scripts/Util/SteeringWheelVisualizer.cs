using Reflex.Attributes;
using UnityEngine;
using YG;

public class SteeringWheelVisualizer : MonoBehaviour
{
    private enum RotationAxis { X, Y, Z }

    [SerializeField] private RotationAxis _rotationAxis = RotationAxis.Z;
    [SerializeField] private float _maxSteerAngle = 540f;
    [SerializeField] private float _steerSpeed = 6f;
    [SerializeField] private bool _invertDirection = true;

    [Inject] private MobileInputController _mobileInput;

    private Transform _cachedTransform;
    private float _currentSteerAngle;
    private bool _isMobile;

    private void Awake()
    {
        _cachedTransform = transform;
        _isMobile = YandexGame.EnvironmentData.isMobile;

        if (_isMobile && !_mobileInput)
        { 
            Debug.Log($"{name}: MobileInputController reference is missing!", this);
            enabled = false;
            return;
        }    
    }

    private void Update() => ApplySteeringRotation();

    private void ApplySteeringRotation()
    {
        float horizontalInput = GetHorizontalInput();
        float targetAngle = horizontalInput * _maxSteerAngle;

        _currentSteerAngle = Mathf.MoveTowards(
            _currentSteerAngle,
            targetAngle,
            _steerSpeed * Time.deltaTime * 100f
        );

        ApplyRotation(_currentSteerAngle);
    }

    private float GetHorizontalInput()
    {
        if (_isMobile)
            return _mobileInput ? _mobileInput.Horizontal : 0f;

        return Input.GetAxis("Horizontal");
    }

    private void ApplyRotation(float angle)
    {
        Vector3 localEuler = _cachedTransform.localEulerAngles;
        float finalAngle = _invertDirection ? -angle : angle;

        switch (_rotationAxis)
        {
            case RotationAxis.X: localEuler.x = finalAngle; break;
            case RotationAxis.Y: localEuler.y = finalAngle; break;
            case RotationAxis.Z: localEuler.z = finalAngle; break;
        }

        _cachedTransform.localEulerAngles = localEuler;
    }
}


//using UnityEngine;

//public class SteeringWheelVisualizer : MonoBehaviour
//{
//    private enum RotationAxis { X, Y, Z }

//    [SerializeField] private RotationAxis _rotationAxis = RotationAxis.Z;
//    [SerializeField] private float _maxSteerAngle = 540f;
//    [SerializeField] private float _steerSpeed = 6f;
//    [SerializeField] private bool _invertDirection = true;

//    private Transform _cachedTransform;
//    private float _currentSteerAngle;

//    private void Awake() => _cachedTransform = transform;

//    private void Update() => ApplySteeringRotation();

//    private void ApplySteeringRotation()
//    {
//        float targetAngle = Input.GetAxis("Horizontal") * _maxSteerAngle;
//        _currentSteerAngle = Mathf.MoveTowards(
//            _currentSteerAngle,
//            targetAngle,
//            _steerSpeed * Time.deltaTime * 100f
//        );

//        Vector3 localEuler = _cachedTransform.localEulerAngles;
//        float finalAngle = _invertDirection ? -_currentSteerAngle : _currentSteerAngle;

//        localEuler.x = (_rotationAxis == RotationAxis.X) ? finalAngle : localEuler.x;
//        localEuler.y = (_rotationAxis == RotationAxis.Y) ? finalAngle : localEuler.y;
//        localEuler.z = (_rotationAxis == RotationAxis.Z) ? finalAngle : localEuler.z;

//        _cachedTransform.localEulerAngles = localEuler;
//    }
//}