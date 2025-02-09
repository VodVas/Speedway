using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class VehiclePartsExploderInstaller : MonoInstaller
{
    [SerializeField] private VehiclePartsExploder _buggyParts;
    [SerializeField] private VehiclePartsExploder _hotRodParts;
    [SerializeField] private VehiclePartsExploder _crossroadParts;
    [SerializeField] private VehiclePartsExploder _mustangParts;
    [SerializeField] private VehiclePartsExploder _redNeckParts;
    [SerializeField] private VehiclePartsExploder _newsVanParts;


    public override void InstallBindings()
    {
        if (_buggyParts == null || _hotRodParts == null || _crossroadParts == null|| _mustangParts == null || _redNeckParts == null || _newsVanParts == null) 
        {
            Debug.LogError("_buggyPartsOne or more components are not assigned.");
            return;
        }

        Dictionary<Type, VehiclePartsExploder> partsDictionary = new Dictionary<Type, VehiclePartsExploder>
        {
            { typeof(BUGgy), _buggyParts },
            { typeof(HotRod), _hotRodParts },
            { typeof(Crossroad), _crossroadParts },
            { typeof(Mustang), _mustangParts },
            { typeof(RedNeck), _redNeckParts },
            { typeof(NewsVan), _newsVanParts },
        };

        Container.Bind<IFactory<VehiclePartsExploder>>().To<Factory<VehiclePartsExploder>>().AsSingle().WithArguments(partsDictionary);
    }
}