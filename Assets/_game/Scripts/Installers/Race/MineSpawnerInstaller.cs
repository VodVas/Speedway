using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MineSpawnerInstaller : MonoInstaller
{
    [SerializeField] private Detonator _mine;

    public override void InstallBindings()
    {
        if (_mine == null)
        {
            Debug.LogError("one or more components are not assigned.");
            return;
        }

        Dictionary<Type, Detonator> mines = new Dictionary<Type, Detonator>
        {
            { typeof(Detonator), _mine}
        };

        Container.Bind<IFactory<Detonator>>().To<Factory<Detonator>>().AsSingle().WithArguments(mines);
    }
}