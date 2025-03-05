using ArcadeVP;
using Reflex.Attributes;
using System;
using UnityEngine;

public class AutoLevelingManager : MonoBehaviour
{
    [Serializable]
    private class PlayerCars
    {
        [field: SerializeField] public GameObject PlayerCar { get; private set; }
    }

    [Serializable]
    private class EnemyCars
    {
        [field: SerializeField] public GameObject EnemyCar { get; private set; }
        [field: SerializeField] public float MinHealthStatOffset { get; private set; } = -5f;
        [field: SerializeField] public float MaxHealthStatOffset { get; private set; } = 5f;
        [field: SerializeField] public float MinSpeedStatOffset { get; private set; } = 5f;
        [field: SerializeField] public float MaxSpeedStatOffset { get; private set; } = 5f;
        [field: SerializeField] public float minAccStatOffset { get; private set; } = 5f;
        [field: SerializeField] public float MaxAccStatOffset { get; private set; } = 5f;
        [field: SerializeField] public float MinTurnStatOffset { get; private set; } = 5f;
        [field: SerializeField] public float MaxTurnStatOffset { get; private set; } = 5f;
    }

    [SerializeField] private PlayerCars[] _playerCars;
    [SerializeField] private EnemyCars[] _enemies;

    [Inject] private PlayerCarSelector _playerCarSelector;

    private void OnEnable()
    {
        _playerCarSelector.CarActivated += ApplyAutoLeveling;
    }

    private void OnDisable()
    {
        _playerCarSelector.CarActivated -= ApplyAutoLeveling;
    }

    public void ApplyAutoLeveling()
    {
        GameObject activePlayerCar = FindActivePlayerCar();

        if (activePlayerCar == null)
        {
            Debug.LogWarning("[AutoLevelingManager] Не найдена активная машина игрока!");
            return;
        }

        Health playerHealth = activePlayerCar.GetComponent<Health>();
        ArcadeVehicleController playerController = activePlayerCar.GetComponent<ArcadeVehicleController>();

        if (playerHealth == null || playerController == null)
        {
            Debug.LogError("[AutoLevelingManager] У активной машины игрока отсутствуют Health или ArcadeVehicleController!", activePlayerCar);
            return;
        }

        float playerMaxHealth = playerHealth.Max;
        float playerMaxSpeed = playerController.GetMaxSpeed();
        float playerAcceleration = playerController.GetAcceleration();
        float playerTurn = playerController.GetTurn();

        int length = _enemies.Length;

        for (int i = 0; i < length; i++)
        {
            EnemyCars enemySlot = _enemies[i];

            if (enemySlot == null || enemySlot.EnemyCar == null)
            {
                continue;
            }

            Health enemyHealth = enemySlot.EnemyCar.GetComponent<Health>();
            ArcadeAiVehicleController enemyController = enemySlot.EnemyCar.GetComponent<ArcadeAiVehicleController>();

            if (enemyHealth == null || enemyController == null)
            {
                Debug.LogWarning("[AutoLevelingManager] У одного из врагов отсутствуют нужные компоненты!", enemySlot.EnemyCar);
                enabled = false;
                continue;
            }

            float offsetHealth = RandomRange(enemySlot.MinHealthStatOffset, enemySlot.MaxHealthStatOffset);
            float offsetSpeed = RandomRange(enemySlot.MinSpeedStatOffset, enemySlot.MaxSpeedStatOffset);
            float offsetAccel = RandomRange(enemySlot.minAccStatOffset, enemySlot.MaxAccStatOffset);
            float offsetTurn = RandomRange(enemySlot.MinTurnStatOffset, enemySlot.MaxTurnStatOffset);
            float newEnemyHealth = playerMaxHealth + offsetHealth;

            if (newEnemyHealth < 1) newEnemyHealth = 1;
            enemyHealth.Init(newEnemyHealth);

            float newEnemyMaxSpeed = playerMaxSpeed + offsetSpeed;

            if (newEnemyMaxSpeed < 0.1f) newEnemyMaxSpeed = 0.1f;
            enemyController.MaxSpeed = newEnemyMaxSpeed;

            float newEnemyAccel = playerAcceleration + offsetAccel;

            if (newEnemyAccel < 0f) newEnemyAccel = 0f;
            enemyController.accelaration = newEnemyAccel;

            float newEnemyTurn = playerTurn + offsetTurn;

            if (newEnemyTurn < 0f) newEnemyTurn = 0f;
            enemyController.turn = newEnemyTurn;
        }
    }

    private GameObject FindActivePlayerCar()
    {
        int length = _playerCars.Length;

        for (int i = 0; i < length; i++)
        {
            PlayerCars playerCars = _playerCars[i];

            if (playerCars != null && playerCars.PlayerCar != null && playerCars.PlayerCar.activeSelf)
            {
                return playerCars.PlayerCar;
            }
        }

        return null;
    }

    private float RandomRange(float minValue, float maxValue)
    {
        return UnityEngine.Random.Range(minValue, maxValue);
    }
}