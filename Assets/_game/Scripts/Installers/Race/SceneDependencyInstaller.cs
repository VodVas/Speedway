using UnityEngine;
using Reflex.Core;

public class SceneDependencyInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private AiStuckHelper _aiStuckHelper = null;
    [SerializeField] private RaceStartTimeCounter _timeCounter = null;
    [SerializeField] private DeadCarRespawner _carRespawner = null;
    [SerializeField] private PlayerCarSelector _playerCarSelector = null;
    [SerializeField] private RaceRewardHandler _rewardHandler = null;
    [SerializeField] private ParticleWeaponLinker _weaponLinker = null;

    private const string ErrorMissingField = "[UIInstaller] Не все поля назначены в инспекторе!";

    private void Awake()
    {
        if (_aiStuckHelper == null || _timeCounter == null || _carRespawner == null || _playerCarSelector == null || _rewardHandler == null || _weaponLinker == null)
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
        builder.AddSingleton(_playerCarSelector, typeof(PlayerCarSelector));
        builder.AddSingleton(_rewardHandler, typeof(RaceRewardHandler));
        builder.AddSingleton(_weaponLinker, typeof(ParticleWeaponLinker));
    }
}