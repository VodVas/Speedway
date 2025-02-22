using UnityEngine;
using System.Collections.Generic;
using System;
using ArcadeVP;

public class CarUpgrades : MonoBehaviour
{
    [SerializeField] private List<CarUpgrade> _upgrades;

    [field: SerializeField] public int CarId { get; private set; } = 0;

    private CarData _carData;

    public IReadOnlyList<CarUpgrade> Upgrades => _upgrades;

    private void Awake()
    {
        _carData = GetComponent<CarData>();

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

    public void ApplyPurchasedStats(Func<int, int, bool> hasCarUpgrade,
        ArcadeVehicleController controller,
        Health health)
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







//public class CarUpgrades : MonoBehaviour
//{
//    [SerializeField] private List<CarUpgrade> _upgrades;
//    [field: SerializeField] public int CarId { get; private set; } = 0;

//    private CarData _carData;

//    public IReadOnlyList<CarUpgrade> Upgrades => _upgrades;

//    private void Awake()
//    {
//        _carData = GetComponent<CarData>();

//        if (CarId < 0)
//        {
//            Debug.LogError($"[CarUpgrades] Неверный CarId: {CarId}", this);
//            enabled = false;
//            return;
//        }

//        if (_carData == null)
//        {
//            Debug.LogError($"[CarUpgrades] Не назначен CarData для CarId={CarId}!", this);
//            enabled = false;
//            return;
//        }

//        if (_upgrades == null)
//        {
//            Debug.LogError($"[CarUpgrades] Список _upgrades не назначен для CarId={CarId}!", this);
//            enabled = false;
//            return;
//        }
//    }

//    public void InitializePurchasedUpgrades(Func<int, int, bool> hasCarUpgrade)
//    {
//        if (!enabled) return;
//        if (_upgrades.Count == 0) return; // Нечего инициализировать

//        for (int i = 0; i < _upgrades.Count; i++)
//        {
//            CarUpgrade upgrade = _upgrades[i];
//            if (upgrade == null)
//                continue;

//            bool purchased = hasCarUpgrade(CarId, upgrade.UpgradeId);
//            upgrade.SetActive(purchased);
//        }
//    }

//    public void ApplyPurchasedStats(Func<int, int, bool> hasCarUpgrade, ArcadeVehicleController controller, Health health)
//    {
//        if (!enabled) return; // Если скрипт отключён, нет смысла что-то применять

//        if (controller != null)
//        {
//            controller.SetMaxSpeed(_carData.Speed);
//            controller.SetAcceleration(_carData.Acceleration);
//            controller.SetTurn(_carData.Turn);
//            // Если нужно оружие, можно тоже сбрасывать
//        }
//        if (health != null)
//        {
//            health.Init(_carData.Armor); // armor = "здоровье" машины
//        }

//        // 2) Прибавляем апгрейды на основе сохранений
//        if (_upgrades.Count == 0 || hasCarUpgrade == null)
//            return;

//        for (int i = 0; i < _upgrades.Count; i++)
//        {
//            CarUpgrade upgrade = _upgrades[i];
//            if (upgrade == null)
//                continue;

//            bool purchased = hasCarUpgrade(CarId, upgrade.UpgradeId);
//            if (!purchased)
//                continue;

//            switch (upgrade.UpgradeType)
//            {
//                case CarUpgrade.CarUpgradeType.Speed:
//                    if (controller != null)
//                        controller.SetMaxSpeed(controller.GetMaxSpeed() + upgrade.UpgradeValue);
//                    break;
//                case CarUpgrade.CarUpgradeType.Acceleration:
//                    if (controller != null)
//                        controller.SetAcceleration(controller.GetAcceleration() + upgrade.UpgradeValue);
//                    break;
//                case CarUpgrade.CarUpgradeType.Turn:
//                    if (controller != null)
//                        controller.SetTurn(controller.GetTurn() + upgrade.UpgradeValue);
//                    break;
//                case CarUpgrade.CarUpgradeType.Health:
//                    if (health != null)
//                    {
//                        float newMax = health.Max + upgrade.UpgradeValue;
//                        health.Init(newMax);
//                    }
//                    break;
//            }
//        }
//    }
//}

















//public class CarUpgrades : MonoBehaviour
//{
//    [SerializeField] private List<CarUpgrade> _upgrades;

//    [field: SerializeField] public int CarId { get; private set; } = 0;

//    public IReadOnlyList<CarUpgrade> Upgrades => _upgrades;

//    private void Awake()
//    {
//        if (_upgrades == null)
//        {
//            Debug.LogError($"CarUpgrades: список _upgrades не назначен на {name}", this);
//            enabled = false;
//        }
//    }

//    public void InitializePurchasedUpgrades(Func<int, int, bool> hasCarUpgrade)
//    {
//        if (_upgrades == null || _upgrades.Count == 0) return;

//        for (int i = 0; i < _upgrades.Count; i++)
//        {
//            CarUpgrade upgrade = _upgrades[i];
//            bool purchased = hasCarUpgrade(CarId, upgrade.UpgradeId);

//            upgrade.SetActive(purchased);
//        }
//    }

//    public void ApplyPurchasedStats( Func<int, int, bool> hasCarUpgrade, ArcadeVehicleController controller, Health health)
//    {
//        if (controller == null)
//        {
//            Debug.LogWarning($"CarUpgrades({CarId}): контроллер не задан, не к чему применять!");
//            return;
//        }

//        if (_upgrades == null || _upgrades.Count == 0) return;

//        for (int i = 0; i < _upgrades.Count; i++)
//        {
//            CarUpgrade upgrade = _upgrades[i];
//            bool purchased = hasCarUpgrade(CarId, upgrade.UpgradeId);

//            if (purchased)
//            {
//                //upgrade.SetActive(true); // Под вопросом - итак включено уже в гонке

//                switch (upgrade.UpgradeType)
//                {
//                    case CarUpgradeType.Speed:
//                        controller.SetMaxSpeed(controller.GetMaxSpeed() + upgrade.UpgradeValue);
//                        Debug.Log("SetMaxSpeed");
//                        break;

//                    case CarUpgradeType.Acceleration:
//                        controller.SetAcceleration(controller.GetAcceleration() + upgrade.UpgradeValue);
//                        Debug.Log("SetAcceleration");
//                        break;

//                    case CarUpgradeType.Turn:
//                        controller.SetTurn(controller.GetTurn() + upgrade.UpgradeValue);
//                        Debug.Log("SetTurn");
//                        break;

//                    case CarUpgradeType.Health:
//                        if (health != null)
//                        {
//                            float newMax = health.Max + upgrade.UpgradeValue;
//                            health.Init(newMax);
//                        }
//                        break;

//                    case CarUpgradeType.Weapon:

//                        break;
//                }
//            }
//        }
//    }
//}