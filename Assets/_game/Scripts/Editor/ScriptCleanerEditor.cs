#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectScriptCleaner))]
public class ScriptCleanerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        if (GUILayout.Button("Clean Up Scripts", GUILayout.Height(30)))
        {
            ((ObjectScriptCleaner)target).CleanupHierarchy();
        }
    }
}
#endif