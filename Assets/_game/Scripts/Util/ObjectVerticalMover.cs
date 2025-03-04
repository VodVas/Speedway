using UnityEngine;

public class ObjectVerticalMover : MonoBehaviour
{
    [SerializeField] private Transform _pointDawn;
    [SerializeField] private Transform _pointUp;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private bool _isMoving = true;
    [SerializeField] private float _threshold = 0.1f;
    [SerializeField] private float _pauseDurationUp = 2f;
    [SerializeField] private float _pauseDurationDown = 2f;

    private Vector3 _targetLocalPosition;
    private bool _isPaused;
    private float _pauseTimer;

    private void Start()
    {
        if (_pointDawn == null || _pointUp == null)
        {
            Debug.LogError("Point Dawn or Point Up is not assigned!");
            enabled = false;
            return;
        }

        _targetLocalPosition = _pointUp.localPosition;
    }

    private void Update()
    {
        if (!_isMoving) return;

        if (_isPaused)
        {
            _pauseTimer -= Time.deltaTime;

            if (_pauseTimer <= 0)
            {
                _isPaused = false;
                _targetLocalPosition = _targetLocalPosition == _pointUp.localPosition ? _pointDawn.localPosition : _pointUp.localPosition;
            }

            return;
        }

        Vector3 direction = (_targetLocalPosition - transform.localPosition).normalized;
        transform.localPosition += direction * _speed * Time.deltaTime;

        if (Vector3.Distance(transform.localPosition, _targetLocalPosition) < _threshold)
        {
            _isPaused = true;
            _pauseTimer = _targetLocalPosition == _pointUp.localPosition ? _pauseDurationUp : _pauseDurationDown;

            transform.localPosition = _targetLocalPosition;
        }
    }
}





//public class ObjectVerticalMover : MonoBehaviour
//{
//    [SerializeField] private Transform _pointDawn;
//    [SerializeField] private Transform _pointUp;
//    [SerializeField] private float _speed = 5f;
//    [SerializeField] private bool _isMoving = true;
//    [SerializeField] private float _threshold = 0.1f;

//    private Vector3 _targetLocalPosition;

//    private void Start()
//    {
//        if (_pointDawn == null || _pointUp == null)
//        {
//            Debug.LogError("Point Dawn or Point Up is not assigned!");
//            enabled = false;
//            return;
//        }

//        _targetLocalPosition = _pointUp.localPosition;
//    }

//    private void Update()
//    {
//        if (_isMoving)
//        {
//            Vector3 direction = (_targetLocalPosition - transform.localPosition).normalized;

//            transform.localPosition += direction * _speed * Time.deltaTime;

//            if (Vector3.Distance(transform.localPosition, _targetLocalPosition) < _threshold)
//            {
//                _targetLocalPosition = _targetLocalPosition == _pointUp.localPosition ? _pointDawn.localPosition : _pointUp.localPosition;
//            }
//        }
//    }
//}