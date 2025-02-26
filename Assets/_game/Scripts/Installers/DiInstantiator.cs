using Reflex.Core;
using Reflex.Injectors;
using System;
using UnityEngine;

public class DiInstantiator
{
    private readonly Container _container;

    public DiInstantiator(Container container)
    {
        _container = container;
    }

    public T Instantiate<T>(T prefab, Vector3 position, Quaternion identity, Transform parent = null) where T : MonoBehaviour
    {
        var instance = Instantiate(prefab, position, Quaternion.identity, parent);
        InjectHierarchy(instance.gameObject);
        return instance;
    }
    

    private void InjectHierarchy(GameObject root)
    {
        foreach (var component in root.GetComponentsInChildren<MonoBehaviour>(true))
        {
            try
            {
                AttributeInjector.Inject(component, _container);
            }
            catch (Exception e)
            {
                Debug.LogError($"Injection failed for {component.GetType().Name}: {e.Message}");
            }
        }
    }
}