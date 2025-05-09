using UnityEngine;

public class SpeedBoostPlatform : MonoBehaviour
{
    [SerializeField] private float _boostAmount = 50f; 
    [SerializeField] private float _duration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ISpeedBoostable speedBoostable))
        {
            speedBoostable.ApplySpeedBoost(_boostAmount, _duration);
        }
    }
}