using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ParticleSoundSynchronizer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private AudioSource _audioSource;
    private bool _wasPlaying;
    private bool _isLooping;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_particleSystem != null)
        {
            var mainModule = _particleSystem.main;
            _isLooping = mainModule.loop;
            _audioSource.loop = _isLooping;
        }
        else
        {
            Debug.LogError("ParticleSystem not assigned.", this);
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        bool isPlaying = _particleSystem.isPlaying;

        if (isPlaying == _wasPlaying) return;

        if (isPlaying)
        {
            HandleParticleStart();
        }
        else
        {
            HandleParticleStop();
        }

        _wasPlaying = isPlaying;
    }

    private void HandleParticleStart()
    {
        if (_isLooping)
        {
            if (!_audioSource.isPlaying) _audioSource.Play();
        }
        else
        {
            if (_audioSource.isPlaying) _audioSource.Stop();
            _audioSource.Play();
        }
    }

    private void HandleParticleStop()
    {
        if (_isLooping && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}





//public class ParticleSoundSynchronizer : MonoBehaviour
//{
//    [SerializeField] private ParticleSystem _particleSystem;

//    private AudioSource _audioSource;
//    private bool _wasPlaying;

//    private void Awake()
//    {
//        _audioSource = GetComponent<AudioSource>();

//        if (_particleSystem != null)
//        {
//            var mainModule = _particleSystem.main;
//            _audioSource.loop = mainModule.loop;
//        }
//        else 
//        {
//            Debug.Log("ParticleSystem not assign", this);
//            enabled = false;
//        }
//    }

//    private void Update()
//    {
//        bool isPlaying = _particleSystem.isPlaying;

//        if (isPlaying == _wasPlaying) return;

//        if (isPlaying)
//        {
//            if (!_audioSource.isPlaying) _audioSource.Play();
//        }
//        else
//        {
//            if (_audioSource.isPlaying) _audioSource.Stop();
//        }

//        _wasPlaying = isPlaying;
//    }
//}



//public class ShootSoundPlayer : MonoBehaviour
//{
//    private AudioSource _shootSound;

//    private void Awake()
//    {
//        _shootSound = GetComponent<AudioSource>();
//    }

//    public void PlaySound()
//    {
//        if (!_shootSound.isPlaying)
//            _shootSound.Play();
//    }

//    public void StopSound()
//    {
//        if (_shootSound.isPlaying)
//            _shootSound.Stop();
//    }
//}