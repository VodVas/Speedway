using Reflex.Core;
using UnityEngine;

public class PaintDatabaseInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private PaintLootDatabase _paintDatabase;

    public void InstallBindings(ContainerBuilder builder)
    {
        if (_paintDatabase == null)
        {
            Debug.LogError("[PaintDatabaseInstaller] Paint database is not assigned!");
            return;
        }

        _paintDatabase.Initialize();
        builder.AddSingleton(_paintDatabase, typeof(PaintLootDatabase));

        Debug.Log("[PaintDatabaseInstaller] Paint database installed");
    }
}