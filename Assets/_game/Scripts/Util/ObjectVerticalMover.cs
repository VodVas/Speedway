using UnityEngine;

public class ObjectVerticalMover : MonoBehaviour
{
    [SerializeField] private Transform _pointDawn;
    [SerializeField] private Transform _pointUp;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private bool _isMoving = true;
    [SerializeField] private float _threshold = 0.1f;

    private Vector3 targetLocalPosition;

    void Start()
    {
        if (_pointDawn == null || _pointUp == null)
        {
            Debug.LogError("Point Dawn or Point Up is not assigned!");
            enabled = false;
            return;
        }

        targetLocalPosition = _pointUp.localPosition;
    }

    void Update()
    {
        if (_isMoving)
        {
            Vector3 direction = (targetLocalPosition - transform.localPosition).normalized;

            transform.localPosition += direction * _speed * Time.deltaTime;

            if (Vector3.Distance(transform.localPosition, targetLocalPosition) < _threshold)
            {
                targetLocalPosition = targetLocalPosition == _pointUp.localPosition ? _pointDawn.localPosition : _pointUp.localPosition;
            }
        }
    }
}