using Reflex.Core;
using UnityEngine;

public class WaypointInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private Transform _waypointContainer = null;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(_waypointContainer, typeof(Transform));
    }
}