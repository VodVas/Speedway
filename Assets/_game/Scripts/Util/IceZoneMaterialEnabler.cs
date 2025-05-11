using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class IceZoneMaterialEnabler : MonoBehaviour
{
    private const string SphereRB = "SphereRB";
    private const int MaxDepth = 2;

    [SerializeField] private PhysicMaterial _iceMaterial;
    [SerializeField] private PhysicMaterial _defaultMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Vehicle _)) return;

        ApplyMaterial(other.transform, _iceMaterial);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Vehicle _)) return;

        ApplyMaterial(other.transform, _defaultMaterial);
    }

    private void ApplyMaterial(Transform root, PhysicMaterial material)
    {
        var buffer = new Transform[16];
        int nextIndex = 0;
        buffer[nextIndex++] = root;

        for (int i = 0; i < nextIndex && nextIndex < buffer.Length; i++)
        {
            var current = buffer[i];
            if (current.name == SphereRB && current.TryGetComponent(out Collider col))
            {
                col.material = material;
            }

            for (int j = 0; j < current.childCount; j++)
            {
                if (nextIndex >= buffer.Length) break;
                buffer[nextIndex++] = current.GetChild(j);
            }

            if (i >= MaxDepth) break;
        }
    }
}