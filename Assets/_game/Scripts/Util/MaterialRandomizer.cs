using UnityEngine;

public class MaterialRandomizer : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        if (_meshRenderer == null)
        {
            Debug.LogError("MeshRenderer not found on the object.");
            enabled = false;
            return;
        }

        Material material = _meshRenderer.material;

        Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        material.color = randomColor;

        float randomMetallic = Random.Range(0f, 1f);
        float randomSmoothness = Random.Range(0f, 1f);

        material.SetFloat("_Metallic", randomMetallic);
        material.SetFloat("_Smoothness", randomSmoothness);
    }
}