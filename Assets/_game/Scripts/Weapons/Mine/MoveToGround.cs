using UnityEngine;

public class MoveToGround : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _offSet = 0.1f;

    private bool _isFalling = true;

    void Update()
    {
        if (!_isFalling) return;

        FindSurface();
    }

    private void FindSurface()
    {
        Vector3 origin = transform.position + Vector3.up * _offSet;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _maxDistance, _groundLayer))
        {
            float distanceToGround = hit.distance - _offSet;

            transform.position = Vector3.MoveTowards(transform.position, hit.point + Vector3.up * _offSet, _speed * Time.deltaTime);

            if (distanceToGround <= _offSet)
            {
                transform.position = hit.point + Vector3.up * _offSet;
                _isFalling = false;
            }
        }
        else
        {
            transform.position += Vector3.down * _speed * Time.deltaTime;
        }
    }
}