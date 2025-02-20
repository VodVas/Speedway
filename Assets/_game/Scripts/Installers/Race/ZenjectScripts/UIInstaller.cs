using UnityEngine;
using Zenject;
using UnityEngine.Scripting;

[Preserve]
public class UIInstaller : MonoInstaller
{
    [SerializeField] private UiCarBinder _uiCarBinder;
    [SerializeField] private SmoothSliderHealthBarDisplay _healthBarDisplay;
    [SerializeField] private DriftScoreUIDisplayer _driftScoreUIDisplayer;
    [SerializeField] private GameObject _UICounter;

    public override void InstallBindings()
    {
        Container.Bind<UiCarBinder>().FromInstance(_uiCarBinder).NonLazy();
        Container.Bind<SmoothSliderHealthBarDisplay>().FromInstance(_healthBarDisplay).NonLazy();
        Container.Bind<DriftScoreUIDisplayer>().FromInstance(_driftScoreUIDisplayer).NonLazy();
        Container.Bind<GameObject>().FromInstance(_UICounter).NonLazy();
    }
}