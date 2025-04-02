using UnityEngine;

public class CarModification : MonoBehaviour
{
    public enum ModificationType
    {
        Speed,
        Acceleration,
        Turn,
        Health,
        Color
    }

    [field: SerializeField] public ModificationType Type { get; private set; } = ModificationType.Speed;
    [field: SerializeField] public int ModificationId { get; private set; } = 0;
    [field: SerializeField] public string ModificationName { get; private set; } = "DefaultMod";
    [field: SerializeField] public int Price { get; private set; } = 100;
    [field: SerializeField] public float Value { get; private set; } = 5f;
    [field: SerializeField] public Material[] Materials { get; private set; }
    [field: SerializeField] public Renderer TargetRenderer { get; private set; }

    // Новые поля для системы динамических материалов
    private Material[] _runtimeMaterials;
    private int _runtimeMaterialsCount;

    public void UpdateRuntimeMaterials(PaintIntegrationSystem paintSystem)
    {
        if (Type != ModificationType.Color) return;
        if (paintSystem == null)
        {
            Debug.LogWarning("PaintSystem is null!");
            return;
        }

        try
        {
            Material[] buffer = new Material[128];
            _runtimeMaterialsCount = paintSystem.GetAvailableMaterials(this, ref buffer);

            _runtimeMaterials = new Material[_runtimeMaterialsCount];
            System.Array.Copy(buffer, _runtimeMaterials, _runtimeMaterialsCount);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Material update failed: {e.Message}");
        }
    }

    public Material GetRuntimeMaterial(int index)
    {
        if (index < 0 || index >= _runtimeMaterialsCount)
            return null;

        return _runtimeMaterials[index];
    }

    public int GetRuntimeMaterialsCount() => _runtimeMaterialsCount;

    private void Awake()
    {
        // Валидация остаётся без изменений
        if (ModificationId < 0)
        {
            Debug.LogError($"[CarModification] Неверный ModificationId: {ModificationId}", this);
            enabled = false;
            return;
        }

        if (Price < 0 && Type != ModificationType.Color)
        {
            Debug.LogError($"[CarModification] Отрицательная цена {Price} у {ModificationName}", this);
            enabled = false;
            return;
        }

        if (Type == ModificationType.Color)
        {
            if (TargetRenderer == null)
            {
                Debug.LogError($"[CarModification] TargetRenderer не назначен для модификации цвета!", this);
                enabled = false;
                return;
            }

            if (Materials == null || Materials.Length == 0)
            {
                Debug.LogError($"[CarModification] Не назначены материалы для модификации цвета!", this);
                enabled = false;
                return;
            }
        }
    }
}





//public class CarModification : MonoBehaviour
//{
//    public enum ModificationType
//    {
//        Speed,
//        Acceleration,
//        Turn,
//        Health,
//        Color
//    }

//    [field: SerializeField] public ModificationType Type { get; private set; } = ModificationType.Speed;
//    [field: SerializeField] public int ModificationId { get; private set; } = 0;
//    [field: SerializeField] public string ModificationName { get; private set; } = "DefaultMod";
//    [field: SerializeField] public int Price { get; private set; } = 100;
//    [field: SerializeField] public float Value { get; private set; } = 5f;
//    [field: SerializeField] public Material[] Materials { get; private set; }
//    [field: SerializeField] public Renderer TargetRenderer { get; private set; }

//    private void Awake()
//    {
//        if (ModificationId < 0)
//        {
//            Debug.LogError($"[CarModification] Неверный ModificationId: {ModificationId}", this);
//            enabled = false;
//            return;
//        }
//        if (Price < 0 && Type != ModificationType.Color)
//        {
//            Debug.LogError($"[CarModification] Отрицательная цена {Price} у {ModificationName}", this);
//            enabled = false;
//            return;
//        }
//        if (Type == ModificationType.Color)
//        {
//            if (TargetRenderer == null)
//            { 
//                Debug.LogError($"[CarModification] TargetRenderer не назначен для модификации цвета!", this);
//                enabled = false;
//                return;
//            }
//            if (Materials == null || Materials.Length == 0)
//            { 
//                Debug.LogError($"[CarModification] Не назначены материалы для модификации цвета!", this);
//                enabled = false;
//                return;
//            }
//        }
//    }
//}