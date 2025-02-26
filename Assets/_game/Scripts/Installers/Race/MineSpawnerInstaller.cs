using Reflex.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawnerInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private Detonator _mine;

    private void Awake()
    {
        if (_mine == null)
        {
            Debug.LogError("one or more components are not assigned.");
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
        Dictionary<Type, Detonator> mines = new Dictionary<Type, Detonator>
    {
        { typeof(Detonator), _mine }
    };

        builder.AddSingleton(mines);

        builder.AddSingleton<IFactory<Detonator>>(resolver =>
        {
            var container = resolver.Resolve<Container>();
            return new Factory<Detonator>(container, mines);
        });
    }
}