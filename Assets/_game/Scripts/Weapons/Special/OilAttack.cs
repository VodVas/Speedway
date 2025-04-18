using UnityEngine;

public class OilAttack : MonoBehaviour
{
    [SerializeField] private ParticleSystem _oil;
    [SerializeField] private LayerMask _playerLayer;

    private int _playerLayerID;

    private void Awake()
    {
        int layerMask = _playerLayer.value;
        _playerLayerID = 0;
        while ((layerMask >>= 1) > 0) _playerLayerID++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayerID && !_oil.isPlaying)
            _oil.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerLayerID && _oil.isPlaying)
            _oil.Stop();
    }
}