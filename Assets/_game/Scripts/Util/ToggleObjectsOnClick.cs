using UnityEngine;
using UnityEngine.UI;

public class ToggleObjectsOnClick : MonoBehaviour
{
    [SerializeField] private GameObject[] _activateObject;
    [SerializeField] private GameObject[] _deactivateObject;

    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(SwitchVisibilityGroups);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SwitchVisibilityGroups);
    }

    private void SwitchVisibilityGroups()
    {
        if (_activateObject != null || _activateObject.Length > 0)
        {
            for (int i = 0; i < _activateObject.Length; i++)
            {
                _activateObject[i].SetActive(true);
            }
        }

        if (_deactivateObject != null || _deactivateObject.Length > 0)
        {
            for (int i = 0; i < _deactivateObject.Length; i++)
            {
                _deactivateObject[i].SetActive(false);
            }
        }
    }
}