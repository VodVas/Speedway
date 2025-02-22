using UnityEngine;
using System;
using System.Collections.Generic;

public sealed class CarModifications : MonoBehaviour
{
    [SerializeField] private List<CarModification> _modifications;
    [field: SerializeField] public int CarId { get; private set; } = 0;

    // Ссылка на базовые данные той же машины
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

    public void InitializePurchasedMods(System.Func<int, int, int> getCarModCount)
    {
        if (!enabled) return;
        if (_modifications.Count == 0) return;

        // В данном случае мы просто проверяем, сколько раз куплено, 
        // но включать/выключать объект модификации — на ваше усмотрение
        for (int i = 0; i < _modifications.Count; i++)
        {
            CarModification cm = _modifications[i];
            if (cm == null)
                continue;
            // При желании включать визуальную часть, если count > 0
        }
    }

    public IReadOnlyList<CarModification> GetAll()
    {
        return _modifications;
    }

    /// <summary>
    /// Сбрасываем статы машины на дефолт, потом прибавляем все купленные модификации по списку savesData.
    /// </summary>
    public void ApplyPurchasedMods(
        System.Func<int, int, int> getCarModCount,
        ArcadeVP.ArcadeVehicleController mover,
        Health health
    )
    {
        if (!enabled) return;
        // Ставим базовые статы из CarData:
        if (mover != null)
        {
            mover.SetMaxSpeed(_carData.Speed);
            mover.SetAcceleration(_carData.Acceleration);
            mover.SetTurn(_carData.Turn);
        }
        if (health != null)
        {
            health.Init(_carData.Armor);
        }

        // Применяем каждую купленную модификацию
        if (_modifications.Count == 0 || getCarModCount == null)
            return;

        for (int i = 0; i < _modifications.Count; i++)
        {
            CarModification mod = _modifications[i];
            if (mod == null)
                continue;

            int timesBought = getCarModCount(CarId, mod.ModificationId);
            if (timesBought < 1)
                continue;

            float totalBonus = mod.Value * timesBought;

            switch (mod.Type)
            {
                case CarModification.ModificationType.Speed:
                    if (mover != null)
                        mover.SetMaxSpeed(mover.GetMaxSpeed() + totalBonus);
                    break;

                case CarModification.ModificationType.Acceleration:
                    if (mover != null)
                        mover.SetAcceleration(mover.GetAcceleration() + totalBonus);
                    break;

                case CarModification.ModificationType.Turn:
                    if (mover != null)
                        mover.SetTurn(mover.GetTurn() + totalBonus);
                    break;

                case CarModification.ModificationType.Health:
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





//public sealed class CarModifications : MonoBehaviour
//{
//    [field: SerializeField] public int CarId { get; private set; } = 0;

//    [SerializeField] private List<CarModification> _modifications = new List<CarModification>();
//    private void Awake()
//    {
//        if (CarId < 0)
//        {
//            Debug.LogError($"[CarModifications] Неверный CarId: {CarId}", this);
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

//    public IReadOnlyList<CarModification> GetAll()
//    {
//        return _modifications;
//    }
//    public void InitializePurchasedMods(Func<int, int, int> getCarModCount)
//    {
//        if (_modifications == null || _modifications.Count == 0)
//        {
//            return;
//        }
//        for (int i = 0; i < _modifications.Count; i++)
//        {
//            CarModification cm = _modifications[i];
//            if (cm == null)
//            {
//                continue;
//            }
//        }
//    }
//    public void ApplyPurchasedMods(
//        Func<int, int, int> getCarModCount,
//        ArcadeVP.ArcadeVehicleController mover,
//        Health health
//    )
//    {
//        if (mover == null)
//        {
//            Debug.LogWarning($"[CarModifications({CarId})] ArcadeVehicleMover не указан — не к чему применять!", this);
//            return;
//        }

//        if (_modifications == null || _modifications.Count == 0)
//        {
//            return;
//        }

//        for (int i = 0; i < _modifications.Count; i++)
//        {
//            CarModification mod = _modifications[i];
//            if (mod == null)
//            {
//                continue;
//            }

//            int timesBought = getCarModCount(CarId, mod.ModificationId);
//            if (timesBought == 0)
//            {
//                continue;
//            }

//            float totalBonus = mod.Value * timesBought;

//            switch (mod.Type)
//            {
//                case CarModification.ModificationType.Speed:
//                    mover.SetMaxSpeed(mover.GetMaxSpeed() + totalBonus);
//                    break;

//                case CarModification.ModificationType.Acceleration:
//                    mover.SetAcceleration(mover.GetAcceleration() + totalBonus);
//                    break;

//                case CarModification.ModificationType.Turn:
//                    mover.SetTurn(mover.GetTurn() + totalBonus);
//                    break;

//                case CarModification.ModificationType.Health:
//                    if (health != null)
//                    {
//                        float newMax = health.Max + totalBonus;
//                        health.Init(newMax);
//                    }
//                    break;
//            }
//        }
//    }
//}