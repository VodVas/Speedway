using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MuteToggler : MonoBehaviour
{
    [SerializeField] private AudioListener _audioListener;

    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        _toggle.isOn = _audioListener.enabled;
    }

    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(ToggleMusic);
    }

    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(ToggleMusic);
    }

    public void ToggleMusic(bool isEnabled)
    {
        if (_audioListener != null)
        {
            _audioListener.enabled = isEnabled;
        }
    }
}