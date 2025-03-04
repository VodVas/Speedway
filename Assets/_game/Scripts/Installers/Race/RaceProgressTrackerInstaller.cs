using Reflex.Core;
using UnityEngine;

public class RaceProgressTrackerInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private RaceProgressTracker _progressTracker;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(_progressTracker, typeof(RaceProgressTracker));
    }
}