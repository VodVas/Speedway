using UnityEngine;
using Zenject;
using UnityEngine.Scripting;

[Preserve]
public class CarShopInstaller : MonoInstaller
{
    [SerializeField] private CarShop _carShop;

    public override void InstallBindings()
    {
        Container.Bind<CarShop>().FromInstance(_carShop).AsSingle().NonLazy();
    }
}