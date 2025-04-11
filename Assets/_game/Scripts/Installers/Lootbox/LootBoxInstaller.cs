using Reflex.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private LootPaintSphere _lootSphere;
    [SerializeField] private LootCar _lootCar;

    private void Awake()
    {
        if (_lootSphere == null || _lootCar == null)
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
    }
}