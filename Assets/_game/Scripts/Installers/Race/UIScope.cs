using UnityEngine;
using Reflex.Core;

public class UIScope : MonoBehaviour, IInstaller
{
    [SerializeField] private UiCarBinder _uiCarBinder = null;
    [SerializeField] private SmoothSliderHealthBarDisplay _healthBarDisplay = null;
    [SerializeField] private DriftScoreUIDisplayer _driftScoreUIDisplayer = null;
    [SerializeField] private GameObject _uiCounter = null;

    private const string ErrorMissingField = "[UIInstaller] Не все поля назначены в инспекторе!";

    private void Awake()
    {
        if (_uiCarBinder == null ||
            _healthBarDisplay == null ||
            _driftScoreUIDisplayer == null ||
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
        builder.AddSingleton(_driftScoreUIDisplayer, typeof(DriftScoreUIDisplayer));
        builder.AddSingleton(_uiCounter, typeof(GameObject));
    }
}