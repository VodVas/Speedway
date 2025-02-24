using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrefabId))]
public class PrefabIdEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PrefabId prefabId = (PrefabId)target;
        if (GUILayout.Button("Update GUID"))
        {
            prefabId.CacheGuid();
        }
    }
}