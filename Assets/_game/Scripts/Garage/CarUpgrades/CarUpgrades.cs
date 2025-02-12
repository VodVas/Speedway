using UnityEngine;
using System.Collections.Generic;
using System;
using static CarUpgrade;
using ArcadeVP;

public class CarUpgrades : MonoBehaviour
{
    [SerializeField] private List<CarUpgrade> _upgrades;

    [field: SerializeField] public int CarId { get; private set; } = 0;

    public IReadOnlyList<CarUpgrade> Upgrades => _upgrades;

    private void Awake()
    {
        if (_upgrades == null)
        {
            Debug.LogError($"CarUpgrades: список _upgrades не назначен на {name}", this);
            enabled = false;
        }
    }

    public void InitializePurchasedUpgrades(Func<int, int, bool> hasCarUpgrade)
    {
        if (_upgrades == null || _upgrades.Count == 0) return;

        for (int i = 0; i < _upgrades.Count; i++)
        {
            CarUpgrade upgrade = _upgrades[i];
            bool purchased = hasCarUpgrade(CarId, upgrade.UpgradeId);

            upgrade.SetActive(purchased);
        }
    }

    public void ApplyPurchasedStats( Func<int, int, bool> hasCarUpgrade, ArcadeVehicleController controller, Health health)
    {
        if (controller == null)
        {
            Debug.LogWarning($"CarUpgrades({CarId}): контроллер не задан, не к чему примен€ть!");
            return;
        }

        if (_upgrades == null || _upgrades.Count == 0) return;

        for (int i = 0; i < _upgrades.Count; i++)
        {
            CarUpgrade upgrade = _upgrades[i];
            bool purchased = hasCarUpgrade(CarId, upgrade.UpgradeId);

            if (purchased)
            {
                //upgrade.SetActive(true); // ѕод вопросом - итак включено уже в гонке

                switch (upgrade.UpgradeType)
                {
                    case CarUpgradeType.Speed:
                        controller.SetMaxSpeed(controller.GetMaxSpeed() + upgrade.UpgradeValue);
                        Debug.Log("SetMaxSpeed");
                        break;

                    case CarUpgradeType.Acceleration:
                        controller.SetAcceleration(controller.GetAcceleration() + upgrade.UpgradeValue);
                        Debug.Log("SetAcceleration");
                        break;

                    case CarUpgradeType.Turn:
                        controller.SetTurn(controller.GetTurn() + upgrade.UpgradeValue);
                        Debug.Log("SetTurn");
                        break;

                    case CarUpgradeType.Health:
                        if (health != null)
                        {
                            float newMax = health.Max + upgrade.UpgradeValue;
                            health.Init(newMax);
                        }
                        break;

                    case CarUpgradeType.Weapon:

                        break;
                }
            }
        }
    }
}