using UnityEngine;
using Zenject;

public class RaceCarSelectorInstaller : MonoInstaller
{
    [SerializeField] private RaceCarSelector _raceCarSelector;

    public override void InstallBindings()
    {
        Container.Bind<RaceCarSelector>().FromInstance(_raceCarSelector).NonLazy();
    }
}