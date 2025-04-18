using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshRenderer))]
public class MaterialRandomizer : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private int _materialCount;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialCount = _meshRenderer.sharedMaterials.Length;
    }

    void Start()
    {
        if (!ValidateComponents()) return;

        for (int i = 0; i < _materialCount; i++)
        {
            ApplyRandomizedProperties(i);
        }
    }

    private bool ValidateComponents()
    {
        if (_meshRenderer != null && _materialCount > 0) return true;

        Debug.LogError(_meshRenderer == null ?
            "MeshRenderer not found" : "No materials assigned");
        enabled = false;
        return false;
    }

    private void ApplyRandomizedProperties(int materialIndex)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        GenerateMaterialProperties(out Color color, out float metallic, out float smoothness);

        block.SetColor("_Color", color);
        block.SetFloat("_Metallic", metallic);
        block.SetFloat("_Smoothness", smoothness);

        _meshRenderer.SetPropertyBlock(block, materialIndex);
    }

    private void GenerateMaterialProperties(out Color color, out float metallic, out float smoothness)
    {
        color = new Color(
            Random.value,
            Random.value,
            Random.value,
            1f
        );
        metallic = Random.value;
        smoothness = Random.value;
    }
}