using Reflex.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxPaintSphereInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private LootSphere _lootSphere;

    private void Awake()
    {
        if (_lootSphere == null)
        {
            Debug.LogError("one or more components are not assigned.");
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
        Dictionary<Type, LootSphere> spheres = new Dictionary<Type, LootSphere>
    {
        { typeof(LootSphere), _lootSphere }
    };

        builder.AddSingleton(spheres);

        builder.AddSingleton<IFactory<LootSphere>>(resolver =>
        {
            var container = resolver.Resolve<Container>();
            return new Factory<LootSphere>(container, spheres);
        });
    }
}