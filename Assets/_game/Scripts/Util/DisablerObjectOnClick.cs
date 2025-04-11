using UnityEngine;
using UnityEngine.UI;

public class DisablerObjectOnClick : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(Execute);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Execute);
    }

    private void Execute()
    {
        _targetObject.SetActive(false);
    }
}