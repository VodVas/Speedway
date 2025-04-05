using System;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Core;
using Reflex.Injectors;

public class Factory<T> : IFactory<T> where T : MonoBehaviour
{
    private readonly Container _container;
    private readonly Dictionary<Type, T> _prefabs;

    public Factory(Container container, Dictionary<Type, T> prefabs)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _prefabs = prefabs ?? throw new ArgumentNullException(nameof(prefabs));
    }

    public T Create(Type type, Vector3 position)
    {
        if (_prefabs.TryGetValue(type, out T prefab))
        {
            T obj = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
            GameObjectInjector.InjectRecursive(obj.gameObject, _container);

            return obj;
        }
        else
        {
            throw new Exception($"Prefab for type {type} not found.");
        }
    }
}