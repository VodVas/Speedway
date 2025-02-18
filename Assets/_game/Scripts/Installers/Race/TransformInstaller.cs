using UnityEngine;
using Zenject;

public class TransformInstaller : MonoInstaller
{
    [SerializeField] private Transform _obj;
    [SerializeField] private AiStuckHelper _aiStuckHelper;

    public override void InstallBindings()
    {
        if (_obj == null || _aiStuckHelper == null)
        {
            Debug.LogError("One or more components are not assigned.");
            return;
        }

        Container.Bind<Transform>().FromInstance(_obj).AsSingle();
        Container.Bind<AiStuckHelper>().FromInstance(_aiStuckHelper).AsSingle();
    }
}