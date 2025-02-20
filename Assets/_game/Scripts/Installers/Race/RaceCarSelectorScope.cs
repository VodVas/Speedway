using Reflex.Core;
using UnityEngine;

public class RaceCarSelectorScope : MonoBehaviour, IInstaller
{
    [SerializeField] private RaceCarSelector _raceCarSelector;
    [SerializeField] private RaceStartTimeCounter _raceStartTimeCounter;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(_raceCarSelector, typeof(RaceCarSelector));
        builder.AddSingleton(_raceStartTimeCounter, typeof(RaceStartTimeCounter));
    }
}