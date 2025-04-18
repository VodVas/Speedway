using UnityEngine;
using Reflex.Core;

public class UIInstaller : MonoBehaviour, IInstaller
{
    private const string ErrorMissingField = "[UIInstaller] Не все поля назначены в инспекторе!";

    [SerializeField] private UiCarBinder _desktopUICarBinder = null;
    [SerializeField] private UiCarBinder _mobileUICarBinder = null;
    [SerializeField] private SmoothSliderHealthBarDisplay _mobileHealthBarDisplay = null;
    [SerializeField] private SmoothSliderHealthBarDisplay _desktopHealthBarDisplay = null;
    [SerializeField] private GameObject _mobileUICounter = null;
    [SerializeField] private GameObject _desktopUICounter = null;

    private void Awake()
    {
        if (_desktopUICarBinder == null || _mobileUICarBinder == null ||
        _mobileHealthBarDisplay == null || _desktopHealthBarDisplay == null ||
        _mobileUICounter == null || _desktopUICounter == null)
        {
            Debug.Log(ErrorMissingField, this);
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(_desktopUICarBinder, typeof(UiCarBinder));
        builder.AddSingleton(_mobileUICarBinder, typeof(UiCarBinder));
        builder.AddSingleton(_mobileHealthBarDisplay, typeof(SmoothSliderHealthBarDisplay));
        builder.AddSingleton(_desktopHealthBarDisplay, typeof(SmoothSliderHealthBarDisplay));
        builder.AddSingleton(_mobileUICounter, typeof(GameObject));
        builder.AddSingleton(_desktopUICounter, typeof(GameObject));
    }
}