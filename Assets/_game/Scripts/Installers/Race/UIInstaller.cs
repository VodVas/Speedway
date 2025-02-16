using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private UiCarBinder _uiCarBinder;
    [SerializeField] private SmoothSliderHealthBarDisplay _healthBarDisplay;
    [SerializeField] private RaceStartTimeCounter _raceStartTimeCounter;

    public override void InstallBindings()
    {
        Container.Bind<UiCarBinder>().FromInstance(_uiCarBinder).NonLazy();
        Container.Bind<SmoothSliderHealthBarDisplay>().FromInstance(_healthBarDisplay).NonLazy();
        Container.Bind<RaceStartTimeCounter>().FromInstance(_raceStartTimeCounter).NonLazy();
    }
}