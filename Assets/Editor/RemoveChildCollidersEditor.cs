using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RemoveChildColliders))]
public class RemoveChildCollidersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RemoveChildColliders script = (RemoveChildColliders)target;

        if (GUILayout.Button("Remove All Child Colliders"))
        {
            script.RemoveAllChildColliders();

            EditorUtility.SetDirty(script.gameObject);
            SceneView.RepaintAll();
        }
    }
}
