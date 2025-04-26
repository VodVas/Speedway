using Reflex.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePartsExploderInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private VehiclePartsExploder _buggyParts;
    [SerializeField] private VehiclePartsExploder _hotRodParts;
    [SerializeField] private VehiclePartsExploder _crossroadParts;
    [SerializeField] private VehiclePartsExploder _mustangParts;
    [SerializeField] private VehiclePartsExploder _redNeckParts;
    [SerializeField] private VehiclePartsExploder _newsVanParts;
    [SerializeField] private VehiclePartsExploder _elvisParts;
    [SerializeField] private VehiclePartsExploder _tubParts;
    [SerializeField] private VehiclePartsExploder _novaParts;
    [SerializeField] private VehiclePartsExploder _phantomParts;
    [SerializeField] private VehiclePartsExploder _fireflyParts;
    [SerializeField] private VehiclePartsExploder _roadkillParts;
    [SerializeField] private VehiclePartsExploder _ice—reamerParts;
    [SerializeField] private VehiclePartsExploder _rollerParts;
    [SerializeField] private VehiclePartsExploder _vortexParts;
    [SerializeField] private VehiclePartsExploder _harleyParts;
    [SerializeField] private VehiclePartsExploder _madMaxParts;
    [SerializeField] private VehiclePartsExploder _undertakerParts;
    [SerializeField] private VehiclePartsExploder _truckerParts;
    [SerializeField] private VehiclePartsExploder _trainParts;

    private void Awake()
    {
        if (_buggyParts == null || _hotRodParts == null || _crossroadParts == null || _mustangParts == null || _redNeckParts == null || _newsVanParts == null ||
            _elvisParts == null || _tubParts == null || _ice—reamerParts == null || _rollerParts == null || _novaParts == null || _phantomParts == null || _fireflyParts == null
            || _roadkillParts == null || _vortexParts == null || _harleyParts == null || _madMaxParts == null || _undertakerParts == null || _truckerParts == null || _trainParts == null)
        {
            Debug.LogError("One or more VehiclePartsExploder components are not assigned.");
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
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
            { typeof(Nova), _novaParts },
            { typeof(Phantom), _phantomParts },
            { typeof(Firefly), _fireflyParts },
            { typeof(Roadkill), _roadkillParts },
            { typeof(IceCreamer), _ice—reamerParts },
            { typeof(Roller), _rollerParts },
            { typeof(Vortex), _vortexParts },
            { typeof(Harley), _harleyParts },
            { typeof(MadMax), _madMaxParts },
            { typeof(Undertaker), _undertakerParts },
            { typeof(Trucker), _truckerParts },
            { typeof(Train), _trainParts },
        };

        builder.AddSingleton(partsDictionary);

        builder.AddSingleton<IFactory<VehiclePartsExploder>>(resolver =>
        {
            var container = resolver.Resolve<Container>();
            return new Factory<VehiclePartsExploder>(container, partsDictionary);
        });
    }
}