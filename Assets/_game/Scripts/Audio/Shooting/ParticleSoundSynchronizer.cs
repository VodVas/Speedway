using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ParticleSoundSynchronizer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool _isOneShot = false;

    private bool _hasPlayedSound = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_particleSystem == null)
        {
            Debug.LogError("ParticleSystem not assigned.", this);
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        bool isParticlePlaying = _particleSystem.isPlaying;

        if (isParticlePlaying)
        {
            if (!_audioSource.isPlaying && !_hasPlayedSound)
            {
                if (_isOneShot)
                {
                    _audioSource.PlayOneShot(_audioClip);
                    _hasPlayedSound = true;
                }
                else
                {
                    _audioSource.Play();
                }
            }
        }
        else
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }

            _hasPlayedSound = false;
        }
    }
}