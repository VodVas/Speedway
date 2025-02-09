using UnityEngine;

public class SpeedReducePlatform : MonoBehaviour
{
    [SerializeField] private float _reduceAmount = 50f;
    [SerializeField] private float _duration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ISpeedReducable speedReducable))
        {
            speedReducable.ApplySpeedReduce(_reduceAmount, _duration);
        }
    }
}