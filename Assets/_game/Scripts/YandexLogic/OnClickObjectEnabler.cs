using UnityEngine;
using UnityEngine.UI;

public class OnClickObjectEnabler : MonoBehaviour
{
    [SerializeField] private Transform _targetObject;
    [SerializeField] private Button _button;

    private bool _isActive = false;

    private void Awake()
    {
        if (!Validate())
        {
            return;
        }
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ToggleObject);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ToggleObject);
    }

    private void ToggleObject()
    {
        if (_targetObject == null)
        {
            Debug.Log("Target object is not assigned", this);
            enabled = false;
            return;
        }

        _isActive = !_isActive;

        _targetObject.gameObject.SetActive(_isActive);
    }

    private bool Validate()
    {
        if (_targetObject == null && _button == null)
        {
            Debug.LogError("Target object is not assigned", this);
            enabled = false;
            return false;
        }

        return true;
    }
}