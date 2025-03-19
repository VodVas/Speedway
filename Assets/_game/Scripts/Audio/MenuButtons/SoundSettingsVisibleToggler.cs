using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsVisibleToggler : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private LampLightToggler _lightToggler;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ToggleCanvas);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ToggleCanvas);
    }

    private void ToggleCanvas()
    {
        if (_settingsCanvas == null) return;

        bool currentState = _settingsCanvas.activeSelf;

        //_lightToggler.StartEffect();
        _lightToggler.StartBlinking();
        _settingsCanvas.SetActive(!currentState);
    }
}