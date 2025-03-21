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
    [SerializeField] private EffectOnScreenUIApplier _desktopEffectOnScreenUIApplier = null;
    [SerializeField] private EffectOnScreenUIApplier _mobileEffectOnScreenUIApplier = null;

    private void Awake()
    {
        if (_desktopUICarBinder == null || _mobileUICarBinder == null || _mobileHealthBarDisplay == null ||
            _mobileHealthBarDisplay == null ||
            _mobileUICounter == null || _mobileUICounter == null || _mobileEffectOnScreenUIApplier == null || _desktopEffectOnScreenUIApplier == null)
        {
            Debug.LogError(ErrorMissingField, this);
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
        builder.AddSingleton(_desktopEffectOnScreenUIApplier, typeof(EffectOnScreenUIApplier));
        builder.AddSingleton(_mobileEffectOnScreenUIApplier, typeof(EffectOnScreenUIApplier));
    }
}