using UnityEngine;

public class OilAttack : MonoBehaviour
{
    [SerializeField] private ParticleSystem _oil;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player _) && !_oil.isPlaying)
        _oil.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player _) && _oil.isPlaying)
            _oil.Stop();
    }
}