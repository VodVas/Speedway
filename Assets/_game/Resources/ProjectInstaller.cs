using Zenject;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
public class ProjectInstaller : ScriptableObjectInstaller<ProjectInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<SaveService>().AsSingle().NonLazy();
    }
}