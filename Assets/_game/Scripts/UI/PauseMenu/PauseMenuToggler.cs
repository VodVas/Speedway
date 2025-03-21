using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PauseMenuToggler : MonoBehaviour
{
    [SerializeField] private PauseManager _pauseManager;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (_pauseManager == null)
        {
            Debug.Log("_pauseManager not assign", this);
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Handle);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Handle);
    }

    private void Handle()
    {
        _pauseManager.TogglePause();
    }
}