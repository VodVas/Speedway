using Reflex.Core;
using UnityEngine;

public class DriftScoreInstaller : MonoBehaviour, IInstaller
{
    private const string ErrorMissingField = "[UIInstaller] Не все поля назначены в инспекторе!";

    [SerializeField] private DriftScoreUIDisplayer _driftScoreUIDisplayer = null;

    private void Awake()
    {
        if ( _driftScoreUIDisplayer == null )
        {
            Debug.LogError(ErrorMissingField, this);
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(_driftScoreUIDisplayer, typeof(DriftScoreUIDisplayer));
    }
}