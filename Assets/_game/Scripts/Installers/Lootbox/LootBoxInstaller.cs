using Reflex.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private LootPaintSphere _lootSphere;
    [SerializeField] private LootCar _lootCar;
    [SerializeField] private LootMoney _lootMoney;

    private void Awake()
    {
        if (_lootSphere == null || _lootCar == null || _lootMoney == null)
        {
            Debug.LogError("one or more components are not assigned.", this);
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
        Dictionary<Type, LootPaintSphere> spheres = new Dictionary<Type, LootPaintSphere> { { typeof(LootPaintSphere), _lootSphere } };

        builder.AddSingleton(spheres);

        builder.AddSingleton<IFactory<LootPaintSphere>>(resolver =>
        {
            var container = resolver.Resolve<Container>();
            return new Factory<LootPaintSphere>(container, spheres);
        });




        Dictionary<Type, LootCar> lootCars = new Dictionary<Type, LootCar> { { typeof(LootCar), _lootCar } };

        builder.AddSingleton(lootCars);

        builder.AddSingleton<IFactory<LootCar>>(resolver =>
        {
            var container = resolver.Resolve<Container>();
            return new Factory<LootCar>(container, lootCars);
        });



        Dictionary<Type, LootMoney> lootMoney = new Dictionary<Type, LootMoney> { { typeof(LootMoney), _lootMoney } };

        builder.AddSingleton(lootMoney);

        builder.AddSingleton<IFactory<LootMoney>>(resolver =>
        {
            var container = resolver.Resolve<Container>();
            return new Factory<LootMoney>(container, lootMoney);
        });

    }
}


//public class LootBoxInstaller : MonoBehaviour, IInstaller
//{
//    [SerializeField] private LootSphere _lootSphere;

//    private void Awake()
//    {
//        if (_lootSphere == null)
//        {
//            Debug.LogError("one or more components are not assigned.");
//            enabled = false;
//            return;
//        }
//    }

//    public void InstallBindings(ContainerBuilder builder)
//    {
//        Dictionary<Type, LootSphere> spheres = new Dictionary<Type, LootSphere>
//    {
//        { typeof(LootSphere), _lootSphere }
//    };

//        builder.AddSingleton(spheres);

//        builder.AddSingleton<IFactory<LootSphere>>(resolver =>
//        {
//            var container = resolver.Resolve<Container>();
//            return new Factory<LootSphere>(container, spheres);
//        });
//    }
//}