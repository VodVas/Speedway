using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ParticleSoundSynchronizer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private AudioSource _audioSource;
    private bool _wasPlaying;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_particleSystem != null)
        {
            var mainModule = _particleSystem.main;
            _audioSource.loop = mainModule.loop;
        }
        else 
        {
            Debug.Log("ParticleSystem not assign", this);
            enabled = false;
        }
    }

    private void Update()
    {
        bool isPlaying = _particleSystem.isPlaying;

        if (isPlaying == _wasPlaying) return;

        if (isPlaying)
        {
            if (!_audioSource.isPlaying) _audioSource.Play();
        }
        else
        {
            if (_audioSource.isPlaying) _audioSource.Stop();
        }

        _wasPlaying = isPlaying;
    }
}



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