using UnityEditor;
using UnityEngine;

public class PrefabId : MonoBehaviour
{
    [field: SerializeField] public string Guid { get; private set; }

#if UNITY_EDITOR
    [ContextMenu("Update GUID")]
    public void CacheGuid()
    {
        if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
        {
            string path = AssetDatabase.GetAssetPath(gameObject);
            Guid = AssetDatabase.AssetPathToGUID(path);
            EditorUtility.SetDirty(this);
            Debug.Log($"GUID updated: {Guid}");
        }
        else
        {
            Debug.LogWarning("Not original prefab!");
        }
    }
#endif
}