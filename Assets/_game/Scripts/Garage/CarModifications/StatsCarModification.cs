using UnityEngine;

public sealed class StatsCarModification : BaseCarModification
{
    private enum StatType { Speed, Acceleration, Turn, Health }

    [SerializeField] private StatType _targetStat;
    [SerializeField] private float _valuePerUnit = 5f;

    public bool TryApplyEffect(int count, ArcadeVP.ArcadeVehicleController controller, Health health)
    {
        if (count < 1 || controller == null) return false;

        float totalValue = _valuePerUnit * count;
        switch (_targetStat)
        {
            case StatType.Speed:
                controller.SetMaxSpeed(controller.GetMaxSpeed() + totalValue);
                return true;
            case StatType.Acceleration:
                controller.SetAcceleration(controller.GetAcceleration() + totalValue);
                return true;
            case StatType.Turn:
                controller.SetTurn(controller.GetTurn() + totalValue);
                return true;
            case StatType.Health when health != null:
                health.Init(health.Max + totalValue);
                return true;
            default:
                return false;
        }
    }

    public override string GetEffectDescription() => _targetStat switch
    {
        StatType.Speed => $"Скорость +{_valuePerUnit}",
        StatType.Acceleration => $"Ускорение +{_valuePerUnit}",
        StatType.Turn => $"Поворот +{_valuePerUnit}",
        StatType.Health => $"Жизни +{_valuePerUnit}",
        _ => "Неизвестный эффект"
    };
}