//using Reflex.Core;
//using UnityEngine;

//public class GarageInstaller : MonoBehaviour, IInstaller
//{
//    [SerializeField] private SpheresMaterialsRegistry _registry;

//    private void Awake()
//    {
//        if (_registry == null)
//        {
//            Debug.LogError("one or more components are not assigned.", this);
//            enabled = false;
//            return;
//        }
//    }

//    public void InstallBindings(ContainerBuilder builder)
//    {
//        builder.AddSingleton(_registry, typeof(SpheresMaterialsRegistry));
//    }
//}