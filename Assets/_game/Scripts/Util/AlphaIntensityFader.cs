using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class AlphaIntensityFader : MonoBehaviour
{
    [SerializeField] private float _startValue = 1f;
    [SerializeField] private float _endValue = 2f;
    [SerializeField] private float _duration = 2f;

    private Image _targetImage;
    private Material _materialInstance;
    private Coroutine _fadeRoutine;
    private bool _initialized;

    private void Awake()
    {
        _targetImage = GetComponent<Image>();

        if (_targetImage != null)
        {
            _materialInstance = new Material(_targetImage.material);
            _targetImage.material = _materialInstance;
            _initialized = true;
        }
        else
        {
            Debug.LogError("[AlphaIntensityFader] Target Image is not assigned.", this);
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        StartFade();
    }

    private void OnDisable()
    {
        if (_initialized)
        {
            _materialInstance.SetFloat("_AlphaIntensity_Fade_1", _startValue);
        }
    }

    public void StartFade()
    {
        if (!_initialized || _duration <= 0f)
        {
            Debug.LogWarning("[AlphaIntensityFader] Not initialized or invalid duration!", this);
            enabled = false;
            return;
        }

        if (_fadeRoutine != null)
        {
            StopCoroutine(_fadeRoutine);
        }

        _fadeRoutine = StartCoroutine(FadePingPongCoroutine());
    }

    private IEnumerator FadePingPongCoroutine()
    {
        bool forwardDirection = true;
        float currentValue = _startValue;

        while (true)
        {
            float start = forwardDirection ? _startValue : _endValue;
            float end = forwardDirection ? _endValue : _startValue;
            float timePassed = 0f;

            while (timePassed < _duration)
            {
                timePassed += Time.deltaTime;
                float t = Mathf.Clamp01(timePassed / _duration);
                currentValue = Mathf.Lerp(start, end, t);
                _materialInstance.SetFloat("_AlphaIntensity_Fade_1", currentValue);
                yield return null;
            }

            _materialInstance.SetFloat("_AlphaIntensity_Fade_1", end);
            forwardDirection = !forwardDirection;
        }
    }
}