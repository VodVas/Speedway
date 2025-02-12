using UnityEngine;
using Zenject;

public class GarageManagerInstaller : MonoInstaller
{
    [SerializeField] private GarageNavigator _garageManager;
    [SerializeField] private GarageCarOverview _garageCarOverview;

    public override void InstallBindings()
    {
        Container.Bind<GarageNavigator>().FromInstance(_garageManager).NonLazy();
        Container.Bind<GarageCarOverview>().FromInstance(_garageCarOverview).NonLazy();
    }
}