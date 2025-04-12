using UnityEngine;

public class ObjectContinuousShaker : MonoBehaviour
{
    [SerializeField] private Vector3 _shakeAxes = new Vector3(1, 1, 1);
    [SerializeField] private float _maxAmplitude = 0.01f;
    [SerializeField] private float _frequency = 100f;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _amplitudeAxes;
    private float _timeAccumulator;

    private void Awake()
    {
        _amplitudeAxes = new Vector3(
            _shakeAxes.x * _maxAmplitude,
            _shakeAxes.y * _maxAmplitude,
            _shakeAxes.z * _maxAmplitude
        );
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (_frequency <= Mathf.Epsilon) return;

        float deltaTime = Time.deltaTime;
        _timeAccumulator += deltaTime;

        float interval = 1f / _frequency;
        while (_timeAccumulator >= interval)
        {
            ApplyShake();
            _timeAccumulator -= interval;
        }
    }
    private void OnDisable()
    {
        transform.localPosition = _originalPosition;
        transform.localRotation = _originalRotation;
        _timeAccumulator = 0f;
    }

    public void ApplyShake()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-1f, 1f) * _amplitudeAxes.x,
            Random.Range(-1f, 1f) * _amplitudeAxes.y,
            Random.Range(-1f, 1f) * _amplitudeAxes.z
        );

        transform.localPosition = _originalPosition + randomOffset;

        Vector3 randomRotation = new Vector3(
            Random.Range(-1f, 1f) * _amplitudeAxes.x,
            Random.Range(-1f, 1f) * _amplitudeAxes.y,
            Random.Range(-1f, 1f) * _amplitudeAxes.z
        );

        transform.localRotation = _originalRotation * Quaternion.Euler(randomRotation);
    }

    public void StopShakeImmediately()
    {
        enabled = false;
    }
}