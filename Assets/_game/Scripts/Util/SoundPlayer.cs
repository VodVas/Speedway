using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private bool _isEnablePlay = false;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (_isEnablePlay && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void Play()
    {
        if (_audioSource!= null && !_audioSource.isPlaying) 
        {
            _audioSource.Play();
        }
    }

    public void Stop()
    {
        if(_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}