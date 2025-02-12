using UnityEngine;
using Zenject;

public class TransformInstaller : MonoInstaller
{
    [SerializeField] private Transform _obj;

    public override void InstallBindings()
    {
        if (_obj == null)
        {
            Debug.LogError("One or more components are not assigned.");
            return;
        }

        Container.Bind<Transform>().FromInstance(_obj).AsSingle();
    }
}