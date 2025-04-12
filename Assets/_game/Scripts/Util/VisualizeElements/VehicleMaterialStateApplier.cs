using UnityEngine;
using System;

sealed class VehicleMaterialStateApplier : MonoBehaviour
{
    [SerializeField] private LayerMask _affectedLayers = -1;
    [SerializeField] private Shader _targetShader;

    [Serializable]
    private struct GlassProfile
    {
        public Color Color;
        [Range(0.3f, 1f)] public float Transparency;
    }

    [SerializeField] private GlassProfile _primaryProfile = new GlassProfile { Color = new Color(0.9f, 0.95f, 1f, 1f), Transparency = 0.85f };
    [SerializeField] private GlassProfile _secondaryProfile = new GlassProfile { Color = Color.gray, Transparency = 0.5f };

    private Material[] _materialInstances = Array.Empty<Material>();
    private int _colorID;
    private int _transparencyID;

    private void Start()
    {
        _colorID = Shader.PropertyToID("_Color");
        _transparencyID = Shader.PropertyToID("_Transparency");
        PrepareLayerSpecificMaterialInstances();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _materialInstances.Length; i++)
            if (_materialInstances[i] != null) Destroy(_materialInstances[i]);
    }

    private void PrepareLayerSpecificMaterialInstances()
    {
        Renderer[] allRenderers = FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int count = CalculateEligibleRendererCount(allRenderers);

        Renderer[] eligibleRenderers = new Renderer[count];
        _materialInstances = new Material[count];
        PopulateEligibleRenderers(allRenderers, eligibleRenderers);
        ReplaceSharedMaterialsWithCopies(eligibleRenderers);
    }

    private int CalculateEligibleRendererCount(Renderer[] renderers)
    {
        int count = 0;
        for (int i = 0; i < renderers.Length; i++)
            if (IsEligibleRenderer(renderers[i])) count++;
        return count;
    }

    private void PopulateEligibleRenderers(Renderer[] source, Renderer[] destination)
    {
        int index = 0;
        for (int i = 0; i < source.Length; i++)
            if (IsEligibleRenderer(source[i])) destination[index++] = source[i];
    }

    private bool IsEligibleRenderer(Renderer r) =>
        (1 << r.gameObject.layer & _affectedLayers) != 0 &&
        r.sharedMaterial != null &&
        r.sharedMaterial.shader == _targetShader;

    private void ReplaceSharedMaterialsWithCopies(Renderer[] renderers)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            var mat = new Material(renderers[i].sharedMaterial);
            renderers[i].material = mat;
            _materialInstances[i] = mat;
        }
    }

    public void ApplyPreset(bool usePrimary)
    {
        GlassProfile profile = usePrimary ? _primaryProfile : _secondaryProfile;
        for (int i = 0; i < _materialInstances.Length; i++)
        {
            if (_materialInstances[i] == null) continue;

            _materialInstances[i].SetColor(_colorID, profile.Color);
            _materialInstances[i].SetFloat(_transparencyID, profile.Transparency);
        }
    }
}