using UnityEngine;
using Reflex.Core;

public class UIInstaller : MonoBehaviour, IInstaller
{
    private const string ErrorMissingField = "[UIInstaller] Не все поля назначены в инспекторе!";

    [SerializeField] private UiCarBinder _desktopUICarBinder = null;
    [SerializeField] private UiCarBinder _mobileUICarBinder = null;
    [SerializeField] private GameObject _mobileUICounter = null;
    [SerializeField] private GameObject _desktopUICounter = null;
    [SerializeField] private SystemLocalization _localization;
    [SerializeField] private MobileInputController _mobileInput;

    private void Awake()
    {
        if (_desktopUICarBinder == null || _mobileUICarBinder == null ||
        _mobileUICounter == null || _desktopUICounter == null || _localization == null || _mobileInput == null)
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
        builder.AddSingleton(_mobileUICounter, typeof(GameObject));
        builder.AddSingleton(_desktopUICounter, typeof(GameObject));
        builder.AddSingleton(_localization, typeof(SystemLocalization));
        builder.AddSingleton(_mobileInput, typeof(MobileInputController));
    }
}