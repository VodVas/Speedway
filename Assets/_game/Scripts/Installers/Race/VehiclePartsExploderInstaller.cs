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
    [SerializeField] private VehiclePartsExploder _elvisParts;
    [SerializeField] private VehiclePartsExploder _tubParts;


    public override void InstallBindings()
    {
        if (_buggyParts == null || _hotRodParts == null || _crossroadParts == null|| _mustangParts == null || _redNeckParts == null || _newsVanParts == null ||
            _elvisParts == null || _tubParts == null) 
        {
            Debug.LogError("_buggyPartsOne or more components are not assigned.");
            enabled = false;
            return;
        }

        Dictionary<Type, VehiclePartsExploder> partsDictionary = new Dictionary<Type, VehiclePartsExploder>
        {
            { typeof(BUGgy), _buggyParts },
            { typeof(HotRod), _hotRodParts },
            { typeof(Outlander), _crossroadParts },
            { typeof(Mustang), _mustangParts },
            { typeof(RedNeck), _redNeckParts },
            { typeof(NewsVan), _newsVanParts },
            { typeof(Elvis), _elvisParts },
            { typeof(Tub), _tubParts },
        };

        Container.Bind<IFactory<VehiclePartsExploder>>().To<Factory<VehiclePartsExploder>>().AsSingle().WithArguments(partsDictionary);
    }
}