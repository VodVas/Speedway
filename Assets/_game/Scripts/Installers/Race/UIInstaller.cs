using UnityEngine;
using Reflex.Core;

public class UIInstaller : MonoBehaviour, IInstaller
{
    private const string ErrorMissingField = "[UIInstaller] Не все поля назначены в инспекторе!";

    [SerializeField] private UiCarBinder _uiCarBinder = null;
    [SerializeField] private SmoothSliderHealthBarDisplay _healthBarDisplay = null;
    [SerializeField] private GameObject _uiCounter = null;
    [SerializeField] private EffectOnScreenUIApplier _bulletHoleUI = null;

    private void Awake()
    {
        if (_uiCarBinder == null ||
            _healthBarDisplay == null ||
            _uiCounter == null)
        {
            Debug.LogError(ErrorMissingField, this);
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(_uiCarBinder, typeof(UiCarBinder));
        builder.AddSingleton(_healthBarDisplay, typeof(SmoothSliderHealthBarDisplay));
        builder.AddSingleton(_uiCounter, typeof(GameObject));
        builder.AddSingleton(_bulletHoleUI, typeof(EffectOnScreenUIApplier));
    }
}