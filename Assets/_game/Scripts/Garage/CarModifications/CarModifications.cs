using UnityEngine;
using System;
using System.Collections.Generic;
using YG;

[RequireComponent(typeof(CarData))]
public class CarModifications : MonoBehaviour
{
    [SerializeField] private List<CarModification> _modifications;
    private CarData _carData;
    public int CarId { get; private set; }

    private void Awake()
    {
        _carData = GetComponent<CarData>();
        CarId = _carData.Id;

        if (CarId < 0) Debug.LogError($"[CarModifications] Неверный CarId: {CarId}", this);
        if (_carData == null) Debug.LogError($"[CarModifications] Не назначен CarData!", this);
        if (_modifications == null) Debug.LogError($"[CarModifications] Список модификаций пуст!", this);
    }

    public void ApplyPurchasedMods(Func<int, int, int> getCarModCount, ArcadeVP.ArcadeVehicleController controller, Health health)
    {
        if (controller == null && health == null)
        {
            Debug.LogError("[CarModifications] Не заданы controller и health!");
            return;
        }

        ApplyStandardMods(controller, health);
        ApplyPurchasedCountMods(getCarModCount, controller, health);
        ApplyColorMods();
    }

    public void InitializePurchasedMods(Func<int, int, int> getCarModCount)
    {
        if (!enabled) return;
        if (_modifications.Count == 0) return;

        for (int i = 0; i < _modifications.Count; i++)
        {
            CarModification carModification = _modifications[i];

            if (carModification == null)
                continue;
        }
    }

    private void ApplyStandardMods(ArcadeVP.ArcadeVehicleController controller, Health health)
    {
        if (controller != null)
        {
            controller.SetMaxSpeed(_carData.Speed);
            controller.SetAcceleration(_carData.Acceleration);
            controller.SetTurn(_carData.Turn);
        }
        if (health != null) health.Init(_carData.Armor);
    }

    private void ApplyPurchasedCountMods(Func<int, int, int> getCarModCount, ArcadeVP.ArcadeVehicleController controller, Health health)
    {
        foreach (var mod in _modifications)
        {
            if (mod == null || mod.Type == CarModification.ModificationType.Color) continue;

            int count = getCarModCount(CarId, mod.ModificationId);
            if (count < 1) continue;

            ApplyModEffect(mod, count, controller, health);
        }
    }

    private void ApplyModEffect(CarModification mod, int count, ArcadeVP.ArcadeVehicleController controller, Health health)
    {
        float total = mod.Value * count;
        switch (mod.Type)
        {
            case CarModification.ModificationType.Speed:
                controller.SetMaxSpeed(controller.GetMaxSpeed() + total);
                break;
            case CarModification.ModificationType.Acceleration:
                controller.SetAcceleration(controller.GetAcceleration() + total);
                break;
            case CarModification.ModificationType.Turn:
                controller.SetTurn(controller.GetTurn() + total);
                break;
            case CarModification.ModificationType.Health:
                health.Init(health.Max + total);
                break;
        }
    }

    private void ApplyColorMods()
    {
        foreach (var mod in _modifications)
        {
            if (mod == null || mod.Type != CarModification.ModificationType.Color) continue;

            int selectedIndex = YandexGame.savesData.GetSelectedMaterialIndex(CarId, mod.ModificationId);
            if (mod.TargetRenderer != null && mod.Materials != null && selectedIndex < mod.Materials.Length)
            {
                mod.TargetRenderer.material = mod.Materials[selectedIndex];
            }
        }
    }

    public IReadOnlyList<CarModification> GetAll() => _modifications;
}








//public sealed class CarModifications : MonoBehaviour
//{
//    [SerializeField] private List<CarModification> _modifications;

//    //[field: SerializeField] public int CarId { get; private set; } = 0;
//    private CarData _carData;

//    public int CarId { get; private set; } = 0;

//    private void Awake()
//    {
//        _carData = GetComponent<CarData>();

//        CarId = _carData.Id;

//        if (CarId < 0)
//        {
//            Debug.LogError($"[CarModifications] Неверный CarId: {CarId}", this);
//            enabled = false;
//            return;
//        }
//        if (_carData == null)
//        {
//            Debug.LogError($"[CarModifications] Не назначен CarData (CarId={CarId})!", this);
//            enabled = false;
//            return;
//        }
//        if (_modifications == null)
//        {
//            Debug.LogError($"[CarModifications] Список _modifications не назначен на {name}", this);
//            enabled = false;
//            return;
//        }
//    }

//    public void InitializePurchasedMods(Func<int, int, int> getCarModCount)
//    {
//        if (!enabled) return;
//        if (_modifications.Count == 0) return;

//        for (int i = 0; i < _modifications.Count; i++)
//        {
//            CarModification carModification = _modifications[i];

//            if (carModification == null)
//                continue;
//        }
//    }

//    public IReadOnlyList<CarModification> GetAll()
//    {
//        return _modifications;
//    }

//    public void ApplyPurchasedMods(Func<int, int, int> getCarModCount, ArcadeVP.ArcadeVehicleController controller, Health health)
//    {
//        if (controller == null && health == null)
//        {
//            Debug.Log("[CarUpgrades] controller && health не задан!");
//            enabled = false;
//            return;
//        }

//        if (controller != null)
//        {
//            controller.SetMaxSpeed(_carData.Speed);
//            controller.SetAcceleration(_carData.Acceleration);
//            controller.SetTurn(_carData.Turn);
//        }

//        if (health != null)
//        {
//            health.Init(_carData.Armor);
//        }

//        if (_modifications.Count == 0 || getCarModCount == null)
//            return;

//        for (int i = 0; i < _modifications.Count; i++)
//        {
//            CarModification carModification = _modifications[i];

//            if (carModification == null)
//                continue;

//            int timesBought = getCarModCount(CarId, carModification.ModificationId);

//            if (timesBought < 1)
//                continue;

//            float totalBonus = carModification.Value * timesBought;

//            switch (carModification.Type)
//            {
//                case CarModification.ModificationType.Speed:
//                    controller.SetMaxSpeed(controller.GetMaxSpeed() + totalBonus);
//                    break;

//                case CarModification.ModificationType.Acceleration:
//                    controller.SetAcceleration(controller.GetAcceleration() + totalBonus);
//                    break;

//                case CarModification.ModificationType.Turn:
//                    controller.SetTurn(controller.GetTurn() + totalBonus);
//                    break;

//                case CarModification.ModificationType.Health:
//                    float newMax = health.Max + totalBonus;
//                    health.Init(newMax);
//                    break;
//            }
//        }
//    }
//}