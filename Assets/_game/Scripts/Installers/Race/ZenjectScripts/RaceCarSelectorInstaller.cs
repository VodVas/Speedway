using UnityEngine;
using Zenject;
using UnityEngine.Scripting;

[Preserve]
public class RaceCarSelectorInstaller : MonoInstaller
{
    [SerializeField] private RaceCarSelector _raceCarSelector;
    [SerializeField] private RaceStartTimeCounter _raceStartTimeCounter;

    public override void InstallBindings()
    {
        Container.Bind<RaceCarSelector>().FromInstance(_raceCarSelector).NonLazy();
        Container.Bind<RaceStartTimeCounter>().FromInstance(_raceStartTimeCounter).NonLazy();
    }
}