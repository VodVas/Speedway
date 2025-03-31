using UnityEngine;
using System.Collections.Generic;
using System;
using ArcadeVP;

[RequireComponent(typeof(CarData))]
public class CarUpgrades : MonoBehaviour
{
    [SerializeField] private List<CarUpgrade> _upgrades;

    //[field: SerializeField] public int CarId { get; private set; } = 0;
    private CarData _carData;

    public int CarId { get; private set; } = 0;

    public IReadOnlyList<CarUpgrade> Upgrades => _upgrades;

    private void Awake()
    {
        _carData = GetComponent<CarData>();

        CarId = _carData.Id;

        if (CarId < 0)
        {
            Debug.LogError($"[CarUpgrades] Неверный CarId: {CarId}", this);
            enabled = false;
            return;
        }

        if (_carData == null)
        {
            Debug.LogError($"[CarUpgrades] Не назначен CarData (на том же объекте) для CarId={CarId}!", this);
            enabled = false;
            return;
        }

        if (_upgrades == null)
        {
            Debug.LogError($"[CarUpgrades] Список _upgrades не назначен для CarId={CarId}!", this);
            enabled = false;
            return;
        }
    }

    public void InitializePurchasedUpgrades(Func<int, int, bool> hasCarUpgrade)
    {
        if (!enabled) return;

        if (_upgrades.Count == 0) return;

        if (hasCarUpgrade == null)
        {
            Debug.LogError("[CarUpgrades] hasCarUpgrade не задан!");
            return;
        }

        for (int i = 0; i < _upgrades.Count; i++)
        {
            CarUpgrade upgrade = _upgrades[i];
            if (upgrade == null)
                continue;

            bool purchased = hasCarUpgrade(CarId, upgrade.UpgradeId);
            upgrade.SetActive(purchased);
        }
    }

    public void ApplyPurchasedStats(Func<int, int, bool> hasCarUpgrade, ArcadeVehicleController controller, Health health)
    {
        if (controller == null && health == null)
        {
            Debug.Log("[CarUpgrades] controller && health не задан!");
            enabled = false;
            return;
        }

        if (controller != null)
        {
            controller.SetMaxSpeed(_carData.Speed);
            controller.SetAcceleration(_carData.Acceleration);
            controller.SetTurn(_carData.Turn);
        }

        if (health != null)
        {
            health.Init(_carData.Armor);
        }

        if (_upgrades.Count == 0 || hasCarUpgrade == null)
            return;

        for (int i = 0; i < _upgrades.Count; i++)
        {
            CarUpgrade upgrade = _upgrades[i];

            if (upgrade == null)
                continue;

            bool purchased = hasCarUpgrade(CarId, upgrade.UpgradeId);

            if (!purchased)
                continue;

            switch (upgrade.UpgradeType)
            {
                case CarUpgrade.CarUpgradeType.Speed:
                    controller.SetMaxSpeed(controller.GetMaxSpeed() + upgrade.UpgradeValue);
                    break;

                case CarUpgrade.CarUpgradeType.Acceleration:
                    controller.SetAcceleration(controller.GetAcceleration() + upgrade.UpgradeValue);
                    break;

                case CarUpgrade.CarUpgradeType.Turn:
                    controller.SetTurn(controller.GetTurn() + upgrade.UpgradeValue);
                    break;

                case CarUpgrade.CarUpgradeType.Health:
                    float newMax = health.Max + upgrade.UpgradeValue;
                    health.Init(newMax);
                    break;
            }
        }
    }
}