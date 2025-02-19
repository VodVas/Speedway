using UnityEngine;

public class SpeedBoostPlatform : MonoBehaviour
{
    [SerializeField] private float boostAmount = 50f; 
    [SerializeField] private float duration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ISpeedBoostable speedBoostable))
        {
            speedBoostable.ApplySpeedBoost(boostAmount, duration);
        }
    }
}