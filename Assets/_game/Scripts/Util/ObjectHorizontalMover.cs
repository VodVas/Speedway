using UnityEngine;

public class ObjectHorizontalMover : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private bool _isMoving = true;
    [SerializeField] private float _threshold = 0.1f;

    private Vector3 targetPosition;

    void Start()
    {
        if (_pointA == null || _pointB == null)
        {
            Debug.LogError("Point A or Point B is not assigned!");
            enabled = false;
            return;
        }

        targetPosition = _pointB.position;
    }

    void Update()
    {
        if (_isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < _threshold)
            {
                transform.position = targetPosition;
                targetPosition = targetPosition == _pointB.position ? _pointA.position : _pointB.position;
            }
        }
    }
}