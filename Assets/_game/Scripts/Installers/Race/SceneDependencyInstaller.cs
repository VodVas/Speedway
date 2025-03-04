using UnityEngine;
using Reflex.Core;

public class SceneDependencyInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private AiStuckHelper _aiStuckHelper = null;
    [SerializeField] private RaceStartTimeCounter _timeCounter = null;
    [SerializeField] private DeadCarRespawner _carRespawner = null;

    private const string ErrorMissingField = "[UIInstaller] Не все поля назначены в инспекторе!";

    private void Awake()
    {
        if (_aiStuckHelper == null || _timeCounter == null)
        {
            Debug.LogError(ErrorMissingField, this);
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(_aiStuckHelper, typeof(AiStuckHelper));
        builder.AddSingleton(_timeCounter, typeof(RaceStartTimeCounter));
        builder.AddSingleton(_carRespawner, typeof(DeadCarRespawner));
    }
}