using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundOnButtonClickPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Button _button;

    private void Awake()
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() => _button?.onClick.AddListener(Play);
    private void OnDisable() => _button?.onClick.RemoveListener(Play);

    public void Play()
    {
        _audioSource.Play();
    }
}