using UnityEngine;

public class CarModification : MonoBehaviour
{
    public enum ModificationType
    {
        Speed,
        Acceleration,
        Turn,
        Health
    }

    [field: SerializeField] public int ModificationId { get; private set; } = 0;
    [field: SerializeField] public string ModificationName { get; private set; } = "DefaultMod";
    [field: SerializeField] public int Price { get; private set; } = 100;
    [field: SerializeField] public ModificationType Type { get; private set; } = ModificationType.Speed;
    [field: SerializeField] public float Value { get; private set; } = 5f;


    private void Awake()
    {
        if (ModificationId < 0)
        {
            Debug.LogError($"[CarModification] Неверный ModificationId: {ModificationId}", this);
            enabled = false;
            return;
        }
        if (Price < 0)
        {
            Debug.LogError($"[CarModification] Отрицательная цена {Price} у {ModificationName}", this);
            enabled = false;
            return;
        }
    }
}