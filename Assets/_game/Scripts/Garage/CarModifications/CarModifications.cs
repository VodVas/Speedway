using UnityEngine;
using System;
using System.Collections.Generic;

public sealed class CarModifications : MonoBehaviour
{
    [SerializeField] private List<CarModification> _modifications;

    [field: SerializeField] public int CarId { get; private set; } = 0;

    private CarData _carData;

    private void Awake()
    {
        _carData = GetComponent<CarData>();

        if (CarId < 0)
        {
            Debug.LogError($"[CarModifications] Неверный CarId: {CarId}", this);
            enabled = false;
            return;
        }
        if (_carData == null)
        {
            Debug.LogError($"[CarModifications] Не назначен CarData (CarId={CarId})!", this);
            enabled = false;
            return;
        }
        if (_modifications == null)
        {
            Debug.LogError($"[CarModifications] Список _modifications не назначен на {name}", this);
            enabled = false;
            return;
        }
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

    public IReadOnlyList<CarModification> GetAll()
    {
        return _modifications;
    }

    public void ApplyPurchasedMods(Func<int, int, int> getCarModCount, ArcadeVP.ArcadeVehicleController controller, Health health)
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

        if (_modifications.Count == 0 || getCarModCount == null)
            return;

        for (int i = 0; i < _modifications.Count; i++)
        {
            CarModification carModification = _modifications[i];

            if (carModification == null)
                continue;

            int timesBought = getCarModCount(CarId, carModification.ModificationId);

            if (timesBought < 1)
                continue;

            float totalBonus = carModification.Value * timesBought;

            switch (carModification.Type)
            {
                case CarModification.ModificationType.Speed:
                    controller.SetMaxSpeed(controller.GetMaxSpeed() + totalBonus);
                    break;

                case CarModification.ModificationType.Acceleration:
                    controller.SetAcceleration(controller.GetAcceleration() + totalBonus);
                    break;

                case CarModification.ModificationType.Turn:
                    controller.SetTurn(controller.GetTurn() + totalBonus);
                    break;

                case CarModification.ModificationType.Health:
                    float newMax = health.Max + totalBonus;
                    health.Init(newMax);
                    break;
            }
        }
    }
}