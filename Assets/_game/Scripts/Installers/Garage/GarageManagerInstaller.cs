using UnityEngine;
using Zenject;

public class GarageManagerInstaller : MonoInstaller
{
    [SerializeField] private GarageManager _garageManager;

    public override void InstallBindings()
    {
        Container.Bind<GarageManager>().FromInstance(_garageManager).NonLazy();
    }
}