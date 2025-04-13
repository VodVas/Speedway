using UnityEngine;
using YG;

[RequireComponent(typeof(AudioSource))]
public class RespectPitchModulator : MonoBehaviour
{
    [Header("Настройки pitch")]
    [SerializeField] private float _minPitch = 0.6f;
    [SerializeField] private float _maxPitch = 2.7f;
    [SerializeField] private int _maxRespect = 100;

    private AudioSource _audioSource;
    private bool _isInitialized;

    private void Awake()
    {
        CacheComponents();
        InitializeAudioSource();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
        UpdatePitch(YandexGame.savesData.Respect);
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void CacheComponents()
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }

    private void InitializeAudioSource()
    {
        if (_audioSource != null)
        {
            _audioSource.playOnAwake = false;
            _audioSource.loop = true;
            _isInitialized = true;
        }
    }

    private void SubscribeToEvents()
    {
        if (YandexGame.savesData != null)
            YandexGame.savesData.OnRespectChanged += UpdatePitch;
    }

    private void UnsubscribeFromEvents()
    {
        if (YandexGame.savesData != null)
            YandexGame.savesData.OnRespectChanged -= UpdatePitch;
    }

    private void UpdatePitch(int respectValue)
    {
        if (!_isInitialized) return;

        float normalizedRespect = Mathf.Clamp01((float)respectValue / _maxRespect);
        _audioSource.pitch = Mathf.Lerp(_minPitch, _maxPitch, normalizedRespect);

        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }
}