using Reflex.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamBombInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private IceCreamBomb _bomb;

    private void Awake()
    {
        if (_bomb == null)
        {
            Debug.LogError("one or more components are not assigned.");
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
        Dictionary<Type, IceCreamBomb> bombs = new Dictionary<Type, IceCreamBomb>
    {
        { typeof(IceCreamBomb), _bomb }
    };

        builder.AddSingleton(bombs);

        builder.AddSingleton<IFactory<IceCreamBomb>>(resolver =>
        {
            var container = resolver.Resolve<Container>();
            return new Factory<IceCreamBomb>(container, bombs);
        });
    }
}