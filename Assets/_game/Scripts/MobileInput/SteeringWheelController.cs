using UnityEngine;

public class SteeringWheelController : MonoBehaviour
{
    [SerializeField] private MobileInputController _inputController;
    [SerializeField, Range(0f, 360f)] private float _maxSteeringAngle = 180f;
    [SerializeField, Range(0f, 1000f)] private float _rotationSpeed = 300f;

    private RectTransform _steeringWheelPosition;
    private float _currentSteeringAngle;

    private void Awake()
    {
        _steeringWheelPosition = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float targetAngle = _inputController.Horizontal * _maxSteeringAngle;

        _currentSteeringAngle = Mathf.MoveTowards(_currentSteeringAngle, targetAngle, _rotationSpeed * Time.deltaTime);

        if (_steeringWheelPosition != null)
        {
            _steeringWheelPosition.localRotation = Quaternion.Euler(0f, _currentSteeringAngle, -0f);
        }
    }
}