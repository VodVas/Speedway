using UnityEngine;
using Zenject;

public class GarageManagerInstaller : MonoInstaller
{
    [SerializeField] private GarageNavigator _garageManager;

    public override void InstallBindings()
    {
        Container.Bind<GarageNavigator>().FromInstance(_garageManager).NonLazy();
    }
}