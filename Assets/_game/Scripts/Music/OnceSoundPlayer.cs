using UnityEngine;

public class OnceSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _isButtonDownPlay = false;

    private void Start()
    {
        if (_audioSource == null)
        {
            Debug.LogWarning("audioSource == null");
            enabled = false;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (_isButtonDownPlay)
        {
            _audioSource.Play();
        }
    }

    public void Play()
    {
        _audioSource.Play();
    }
}