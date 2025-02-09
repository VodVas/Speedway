using UnityEngine;

public class CarSpinner : MonoBehaviour
{
    [SerializeField] private float _spinForce = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Vehicle vehicle))
        {
            if (vehicle.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddTorque(transform.up * _spinForce, ForceMode.Impulse);
            }
        }
    }
}