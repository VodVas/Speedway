using System.Collections;
using UnityEngine;

public class ObjectShaker : MonoBehaviour
{
    [SerializeField] private Vector3 _shakeAxes = new Vector3(1, 1, 1);
    [SerializeField] private float _maxAmplitude = 0.001f;
    [SerializeField] private float _frequency = 1f;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(1 / _frequency);
    }

    void Start()
    {
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
    }

    void Update()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-_maxAmplitude, _maxAmplitude) * _shakeAxes.x,
            Random.Range(-_maxAmplitude, _maxAmplitude) * _shakeAxes.y,
            Random.Range(-_maxAmplitude, _maxAmplitude) * _shakeAxes.z
        );

        transform.localPosition = _originalPosition + randomOffset;

        Vector3 randomRotation = new Vector3(
            Random.Range(-_maxAmplitude, _maxAmplitude) * _shakeAxes.x,
            Random.Range(-_maxAmplitude, _maxAmplitude) * _shakeAxes.y,
            Random.Range(-_maxAmplitude, _maxAmplitude) * _shakeAxes.z
        );
        transform.localRotation = _originalRotation * Quaternion.Euler(randomRotation);

        if (_frequency > 0)
        {
            StartCoroutine(ApplyShakeWithFrequency());
        }
    }

    private IEnumerator ApplyShakeWithFrequency()
    {
        yield return _wait;
    }
}