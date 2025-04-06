using UnityEngine;
using YG;
using System.Collections.Generic;

public class PaintIntegrationSystem : MonoBehaviour
{
    [SerializeField] private SpheresMaterialsRegistry _registry;

    private Dictionary<int, Material> _unlockedMaterials;
    private bool _needsRefresh = true;

    public bool IsInitialized => _unlockedMaterials != null;

    private void OnEnable()
    {
        YandexGame.GetDataEvent += HandleDataLoaded;

        if (YandexGame.SDKEnabled)
            RefreshUnlockedMaterials();
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= HandleDataLoaded;
    }

    private void HandleDataLoaded()
    {
        RefreshUnlockedMaterials();
    }

    public void ForceRefresh()
    {
        _needsRefresh = true;
        RefreshUnlockedMaterials();
    }

    public Material GetDefaultMaterial(CarModification mod)
    {
        if (mod.Materials != null && mod.Materials.Length > 0)
            return mod.Materials[0];
        return null;
    }

    public int GetAvailableMaterials(CarModification mod, ref Material[] output)
    {
        if (_needsRefresh) RefreshUnlockedMaterials();

        if (_registry == null)
        {
            Debug.LogError("[PaintIntegrationSystem] Registry is not assigned!");
            return 0;
        }

        int defaultCount = mod.Materials?.Length ?? 0;
        int totalCount = defaultCount + _unlockedMaterials.Count;

        Debug.Log($"[PaintIntegrationSystem] Getting materials for {mod.ModificationName}. Default: {defaultCount}, Unlocked: {_unlockedMaterials.Count}");

        if (output == null || output.Length < totalCount)
            output = new Material[totalCount];

        for (int i = 0; i < defaultCount; i++)
        {
            output[i] = mod.Materials[i];
        }

        int index = defaultCount;

        foreach (var kvp in _unlockedMaterials)
        {
            output[index++] = kvp.Value;
        }

        return totalCount;
    }

    private void RefreshUnlockedMaterials()
    {
        if (!_needsRefresh) return;

        if (_registry == null)
        {
            Debug.LogError("[PaintIntegrationSystem] Registry is not assigned!");
            return;
        }

        _unlockedMaterials = new Dictionary<int, Material>(32);

        SavesYG saves = YandexGame.savesData;

        if (saves == null)
        {
            Debug.LogError("[PaintIntegrationSystem] Saves data is null!");
            return;
        }

        int count = saves.GetUnlockedPaintsCount();
        Debug.Log($"[PaintIntegrationSystem] Refreshing materials. Unlocked paints count: {count}");

        for (int i = 0; i < count; i++)
        {
            int paintId = saves.GetUnlockedPaintId(i);

            if (_registry.TryGetMaterial(paintId, out Material mat))
            {
                _unlockedMaterials[paintId] = mat;
                Debug.Log($"[PaintIntegrationSystem] Added material for paint ID: {paintId}");
            }
            else
            {
                Debug.LogWarning($"[PaintIntegrationSystem] Failed to get material for paint ID: {paintId}");
            }
        }

        _needsRefresh = false;
    }
}