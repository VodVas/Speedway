using UnityEngine;
using System;
using System.Collections.Generic;

public sealed class CarModifications : MonoBehaviour
{
    [field: SerializeField] public int CarId { get; private set; } = 0;

    [SerializeField]
    private List<CarModification> _modifications = new List<CarModification>();

    private void Awake()
    {
        if (CarId < 0)
        {
            Debug.LogError($"[CarModifications] Неверный CarId: {CarId}", this);
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

    public IReadOnlyList<CarModification> GetAll()
    {
        return _modifications;
    }

    // 3. Методы логики
    public void InitializePurchasedMods(Func<int, int, int> getCarModCount)
    {
        if (_modifications == null || _modifications.Count == 0)
        {
            return;
        }
        for (int i = 0; i < _modifications.Count; i++)
        {
            CarModification cm = _modifications[i];
            if (cm == null)
            {
                continue;
            }
        }
    }

    public void ApplyPurchasedMods(
        Func<int, int, int> getCarModCount,
        ArcadeVP.ArcadeVehicleController mover,
        Health health
    )
    {
        if (mover == null)
        {
            Debug.LogWarning($"[CarModifications({CarId})] ArcadeVehicleMover не указан — не к чему применять!", this);
            return;
        }

        if (_modifications == null || _modifications.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _modifications.Count; i++)
        {
            CarModification mod = _modifications[i];
            if (mod == null)
            {
                continue;
            }

            int timesBought = getCarModCount(CarId, mod.ModificationId);
            if (timesBought == 0)
            {
                continue;
            }

            // Суммарный эффект = Value * количество покупок
            float totalBonus = mod.Value * timesBought;

            switch (mod.Type)
            {
                case CarModification.ModificationType.Speed:
                    // Добавляем к максимальной скорости
                    mover.SetMaxSpeed(mover.GetMaxSpeed() + totalBonus);
                    break;

                case CarModification.ModificationType.Acceleration:
                    mover.SetAcceleration(mover.GetAcceleration() + totalBonus);
                    break;

                case CarModification.ModificationType.Turn:
                    mover.SetTurn(mover.GetTurn() + totalBonus);
                    break;

                case CarModification.ModificationType.Health:
                    // Прибавляем к максимальному здоровью
                    if (health != null)
                    {
                        float newMax = health.Max + totalBonus;
                        health.Init(newMax);
                    }
                    break;
            }
        }
    }
}