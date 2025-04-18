using UnityEngine;
using YG;
using System;

public sealed class PaintIntegrationSystem : MonoBehaviour
{
    [SerializeField] private PaintLootDatabase _paintDatabase;
    [SerializeField] private CarRegistry _carRegistry;

    private Material[] _unlockedMaterials;
    private int _unlockedCount;
    private bool _needsRefresh = true;

    public bool IsInitialized => _paintDatabase != null;

    private void Awake()
    {
        if (_paintDatabase)
            _paintDatabase.Initialize();
        
        _needsRefresh = true;

        RefreshMaterials();
        Refresh();
    }

    private void Refresh()
    {
        if (_unlockedCount == 0 && YandexGame.savesData != null)
        {
            _needsRefresh = true;

            RefreshMaterials();

            if (_carRegistry != null)
            {
                CarModifications[] cars = _carRegistry.Cars;

                for (int i = 0; i < cars.Length; i++)
                {
                    if (cars[i] != null && cars[i].isActiveAndEnabled)
                    {
                        cars[i].ApplyColors();
                    }
                }
            }
        }
    }

    private void OnEnable() => YandexGame.GetDataEvent += HandleDataLoaded;
    private void OnDisable() => YandexGame.GetDataEvent -= HandleDataLoaded;

    private void HandleDataLoaded()
    {
        _needsRefresh = true;
        RefreshMaterials();
    }

    public void RefreshMaterials()
    {
        if (!IsInitialized)
        {
            Debug.LogError("[PaintIntegrationSystem] Нельзя обновить материалы - система не инициализирована!");
            enabled = false;
            return;
        }
        
        if (!_needsRefresh && _unlockedCount > 0)
        {
            Debug.Log($"[PaintIntegrationSystem] Пропуск обновления - уже загружено {_unlockedCount} материалов");
            return;
        }

        SavesYG saves = YandexGame.savesData;

        if (saves == null) 
        {
            Debug.LogError("[PaintIntegrationSystem] SavesYG is null! Данные не могут быть загружены.");
            enabled = false;
            return;
        }

        saves.RefreshPaintCache();
        
        int capacity = saves.GetUnlockedPaintsCount();
        
        _unlockedMaterials = new Material[capacity];
        _unlockedCount = 0;

        for (int i = 0; i < capacity; i++)
        {
            int paintId = saves.GetUnlockedPaintId(i);

            
            if (_paintDatabase.TryGetMaterial(paintId, out Material mat))
            {
                _unlockedMaterials[_unlockedCount++] = mat;
            }
            else
            {
                Debug.Log($"[PaintIntegrationSystem] Failed to get material for paint ID: {paintId}", this);
            }
        }

        _needsRefresh = false;

        if (_unlockedCount > 0)
        {
            if (_carRegistry != null)
            {
                CarModifications[] cars = _carRegistry.Cars;

                for (int i = 0; i < cars.Length; i++)
                {
                    if (cars[i] != null && cars[i].isActiveAndEnabled && cars[i].ColorModification != null)
                    {
                        cars[i].ColorModification.RefreshMaterials(this);
                    }
                }
            }
        }
        //else
        //{
        //    Debug.LogWarning("[PaintIntegrationSystem] Не найдено ни одной открытой краски!");
        //}
    }

    public int GetAvailableMaterials(ColorCarModification mod, ref Material[] buffer)
    {
        if (!IsInitialized) return 0;

        int defaultCount = mod.DefaultMaterials.Length;
        int total = defaultCount + _unlockedCount;

        if (buffer == null || buffer.Length < total)
            buffer = new Material[total];


        Array.Copy(mod.DefaultMaterials, 0, buffer, 0, defaultCount);

        if (_unlockedMaterials != null)
            Array.Copy(_unlockedMaterials, 0, buffer, defaultCount, _unlockedCount);

        return total;
    }
}






//public class PaintIntegrationSystem : MonoBehaviour
//{
//    [SerializeField] private PaintLootDatabase _paintDatabase;

//    private Dictionary<int, Material> _unlockedMaterials;
//    private bool _needsRefresh = true;
//    private readonly int _initialCapacity = 32;

//    public bool IsInitialized => _unlockedMaterials != null;

//    private void Awake()
//    {
//        _unlockedMaterials = new Dictionary<int, Material>(_initialCapacity);
//        if (_paintDatabase)
//            _paintDatabase.Initialize();
//    }

//    private void OnEnable()
//    {
//        YandexGame.GetDataEvent += HandleDataLoaded;

//        if (YandexGame.SDKEnabled)
//            Invoke(nameof(RefreshUnlockedMaterials), 0.5f);
//    }

//    private void OnDisable()
//    {
//        YandexGame.GetDataEvent -= HandleDataLoaded;
//    }

//    private void HandleDataLoaded()
//    {
//        RefreshUnlockedMaterials();
//    }

//    public void ForceRefresh()
//    {
//        _needsRefresh = true;
//        RefreshUnlockedMaterials();
//    }

//    public Material GetDefaultMaterial(CarModification mod)
//    {
//        if (mod.Materials != null && mod.Materials.Length > 0)
//            return mod.Materials[0];
//        return null;
//    }

//    public int GetAvailableMaterials(CarModification mod, ref Material[] output)
//    {
//        if (_needsRefresh) RefreshUnlockedMaterials();

//        if (_paintDatabase == null)
//        {
//            Debug.LogError("[PaintIntegrationSystem] Paint database is not assigned!", this);
//            enabled = false;
//            return 0;
//        }

//        int defaultCount = mod.Materials?.Length ?? 0;
//        int totalCount = defaultCount + _unlockedMaterials.Count;

//        Debug.Log($"[PaintIntegrationSystem] Getting materials for {mod.ModificationName}. Default: {defaultCount}, Unlocked: {_unlockedMaterials.Count}");

//        if (output == null || output.Length < totalCount)
//            output = new Material[totalCount];

//        for (int i = 0; i < defaultCount; i++)
//        {
//            output[i] = mod.Materials[i];
//        }

//        int index = defaultCount;

//        foreach (var kvp in _unlockedMaterials)
//        {
//            output[index++] = kvp.Value;
//        }

//        return totalCount;
//    }

//    private void RefreshUnlockedMaterials()
//    {
//        if (!_needsRefresh) return;

//        if (_paintDatabase == null)
//        {
//            Debug.LogError("[PaintIntegrationSystem] Paint database is not assigned!");
//            enabled = false;
//            return;
//        }

//        _unlockedMaterials.Clear();

//        SavesYG saves = YandexGame.savesData;
//        if (saves == null)
//        {
//            Debug.LogError("[PaintIntegrationSystem] Saves data is null!");
//            enabled = false;
//            return;
//        }

//        int count = saves.GetUnlockedPaintsCount();

//        Debug.Log($"[PaintIntegrationSystem] Refreshing materials. Unlocked paints count: {count}");

//        for (int i = 0; i < count; i++)
//        {
//            int paintId = saves.GetUnlockedPaintId(i);
//            if (_paintDatabase.TryGetMaterial(paintId, out Material material) && material != null)
//            {
//                _unlockedMaterials[paintId] = material;

//                Debug.Log($"[PaintIntegrationSystem] Added material for paint ID: {paintId}");
//            }
//            else
//            {
//                Debug.LogWarning($"[PaintIntegrationSystem] Failed to get material for paint ID: {paintId}");
//            }
//        }

//        _needsRefresh = false;
//    }
//}