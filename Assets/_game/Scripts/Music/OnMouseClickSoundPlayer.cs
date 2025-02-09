using UnityEngine;

public class OnMouseClickSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

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
        _audioSource.Play();
    }
}