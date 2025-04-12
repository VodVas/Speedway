using UnityEngine;
using System;
using YG;
using ArcadeVP;

[RequireComponent(typeof(CarData))]
public class CarModifications : MonoBehaviour
{
    [SerializeField] private BaseCarModification[] _modifications;
    [SerializeField] private PaintIntegrationSystem _paintSystem;

   // [field: SerializeField] public ColorCarModification _colorModification { get; private set; }

    [SerializeField] private ColorCarModification _colorModification;
    public ColorCarModification ColorModification => _colorModification;

    private CarData _carData;
    private int _carId;
    private bool _isInitialized;

    public int CarId => _carId;
    public BaseCarModification[] Modifications => _modifications;

    private void Awake()
    {
        _carData = GetComponent<CarData>();
        _carId = _carData.Id;

        if (_carId < 0 || _carData == null || _modifications == null)
        {
            Debug.LogError($"[{GetType().Name}] Initialization failed!");
            enabled = false;
            return;
        }

        InitializeMaterials();
        _isInitialized = true;
    }

    public void ForceInitialize()
    {
        foreach (var mod in Modifications)
        {
            if (mod is ColorCarModification colorMod)
                colorMod.RefreshMaterials(FindObjectOfType<PaintIntegrationSystem>());
        }
    }

    private void InitializeMaterials()
    {
        if (_paintSystem == null) _paintSystem = FindObjectOfType<PaintIntegrationSystem>();

        for (int i = 0; i < _modifications.Length; i++)
        {
            if (_modifications[i] is ColorCarModification colorMod)
            {
                colorMod.RefreshMaterials(_paintSystem);
            }
        }
    }

    public void InitializePurchasedMods(Func<int, int, int> getModCount)
    {
        ApplyModifications(getModCount, null, null);
    }

    public void ApplyModifications(Func<int, int, int> getModCount, ArcadeVehicleController controller, Health health)
    {
        if (!_isInitialized) return;

        ApplyBaseStats(controller, health);
        ApplyPurchasedMods(getModCount, controller, health);
        ApplyColors();
    }

    private void ApplyBaseStats(ArcadeVehicleController carController, Health health)
    {
        if (carController != null)
        {
            carController.SetMaxSpeed(_carData.Speed);
            carController.SetAcceleration(_carData.Acceleration);
            carController.SetTurn(_carData.Turn);
        }

        health?.Init(_carData.Armor);
    }

    public void ApplyPurchasedMods(Func<int, int, int> getModCount,
        ArcadeVehicleController c, Health h)
    {
        for (int i = 0; i < _modifications.Length; i++)
        {
            if (_modifications[i] is StatsCarModification statsMod)
            {
                int count = getModCount(_carId, statsMod.ModificationId);
                if (count > 0) statsMod.TryApplyEffect(count, c, h);
            }
        }
    }

    public void ApplyColors()
    {
        for (int i = 0; i < _modifications.Length; i++)
        {
            if (_modifications[i] is ColorCarModification colorMod)
            {
                colorMod.RefreshMaterials(_paintSystem);
                
                int index = YandexGame.savesData.GetSelectedMaterialIndex(_carId, colorMod.ModificationId);
                Debug.Log($"[{GetType().Name}] Applying color for mod {colorMod.ModificationName}, selected index: {index}");
                
                ApplyColor(colorMod, index);
            }
        }
    }

    private void ApplyColor(ColorCarModification mod, int index)
    {
        Material mat = mod.GetMaterial(index);

        if (mat == null)
        {
            Debug.LogWarning($"[{GetType().Name}] Material is null for index {index} in {mod.ModificationName}", this);
            return;
        }
        
        if (mod.TargetRenderer == null)
        {
            Debug.LogError($"[{GetType().Name}] Target renderer is null for {mod.ModificationName}", this);
            return;
        }

        Debug.Log($"[{GetType().Name}] Applied material for {mod.ModificationName}, index: {index}");
        
        Material[] materials = mod.TargetRenderer.materials;
        for (int i = 0; i < materials.Length; i++) materials[i] = mat;
        mod.TargetRenderer.materials = materials;
    }
}