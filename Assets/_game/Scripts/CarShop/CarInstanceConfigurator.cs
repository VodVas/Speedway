using UnityEngine;
using System;

[Serializable]
public class CarInstanceConfigurator
{
    [field: SerializeField] public GameObject CarPrefab { get; private set; }

    public string Guid { get; private set; }


#if UNITY_EDITOR
    public void CacheGuid()
    {
        var path = UnityEditor.AssetDatabase.GetAssetPath(CarPrefab);
        Guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);
    }
#endif
}