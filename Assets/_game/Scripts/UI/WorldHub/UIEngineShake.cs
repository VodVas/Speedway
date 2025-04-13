using UnityEngine;
using YG;

[RequireComponent(typeof(RectTransform))]
public class UIEngineShake : MonoBehaviour
{
    [Header("Настройки интенсивности")]
    [SerializeField] private float _maxShakePower = 20f;
    [SerializeField] private int _maxFuryForEffect = 100;
    [SerializeField] private AnimationCurve _powerCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Базовые параметры")]
    [SerializeField] private float _baseShakeSpeed = 100f;
    [SerializeField] private int _minFuryForShake = 10;

    private RectTransform _rectTransform;
    private Vector3 _originalPosition;
    private float _noiseOffsetX;
    private float _noiseOffsetY;
    private bool _isShaking;

    private void Awake()
    {
        CacheComponents();
        InitializeNoise();
        SaveOriginalPosition();
    }

    private void OnEnable() => StartShaking();
    private void OnDisable() => StopShaking();

    private void CacheComponents()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
    }

    private void InitializeNoise()
    {
        _noiseOffsetX = Random.Range(-100f, 100f);
        _noiseOffsetY = Random.Range(-100f, 100f);
    }

    private void SaveOriginalPosition() => _originalPosition = _rectTransform.localPosition;

    private void StartShaking()
    {
        _isShaking = true;
        StartCoroutine(ShakeProcess());
    }

    private void StopShaking()
    {
        _isShaking = false;
        ResetPosition();
        StopAllCoroutines();
    }

    private System.Collections.IEnumerator ShakeProcess()
    {
        WaitForSecondsRealtime frameDelay = new WaitForSecondsRealtime(0.0167f);

        while (_isShaking)
        {
            if (ShouldShake())
                ApplyShake();
            else
                ResetPosition();

            yield return frameDelay;
        }
    }

    private bool ShouldShake()
    {
        return YandexGame.savesData != null &&
               YandexGame.savesData.GetRespect() >= _minFuryForShake;
    }

    private void ApplyShake()
    {
        UpdateNoiseOffsets();
        CalculateShakeIntensity(out float x, out float y);
        ApplyPositionOffset(x, y);
    }

    private void UpdateNoiseOffsets()
    {
        float delta = Time.unscaledDeltaTime * _baseShakeSpeed;
        _noiseOffsetX += delta;
        _noiseOffsetY += delta;
    }

    private void CalculateShakeIntensity(out float x, out float y)
    {
        int currentFury = YandexGame.savesData.GetRespect();
        float furyPercent = Mathf.Clamp01((float)currentFury / _maxFuryForEffect);
        float power = _powerCurve.Evaluate(furyPercent) * _maxShakePower;

        x = (Mathf.PerlinNoise(_noiseOffsetX, 0) * 2 - 1) * power;
        y = (Mathf.PerlinNoise(0, _noiseOffsetY) * 2 - 1) * power;
    }

    private void ApplyPositionOffset(float x, float y)
    {
        Vector3 offset = new Vector3(x, y, 0f);
        _rectTransform.localPosition = _originalPosition + offset;
    }

    private void ResetPosition()
    {
        _rectTransform.localPosition = _originalPosition;
    }
}