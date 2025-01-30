using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Factory<T> : IFactory<T> where T : MonoBehaviour
{
    private readonly DiContainer _container;
    private readonly Dictionary<Type, T> _prefabs;

    public Factory(DiContainer container, Dictionary<Type, T> prefabs)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _prefabs = prefabs ?? throw new ArgumentNullException(nameof(prefabs));
    }

    public T Create(Type type, Vector3 position)
    {
        if (_prefabs.TryGetValue(type, out T prefab))
        {
            T obj = _container.InstantiatePrefabForComponent<T>(prefab, position, Quaternion.identity, null);

            return obj;
        }
        else
        {
            throw new Exception($"Prefab for type {type} not found.");
        }
    }
}