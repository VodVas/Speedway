using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class CarButtonBase : MonoBehaviour
{
    protected const string InvalidButtonMessage = "Button component is missing on the game object.";

    [Inject] private CarShop _ñarShop;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (_button == null)
        {
            Debug.LogError(InvalidButtonMessage, this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    protected abstract void OnButtonClicked();

    protected CarShop GetCarShop()
    {
        return _ñarShop;
    }
}