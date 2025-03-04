using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetChildRigidbodiesKinematic))]
public class SetChildRigidbodiesKinematicEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SetChildRigidbodiesKinematic script = (SetChildRigidbodiesKinematic)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Enable Kinematic (All Children)"))
        {
            ModifyKinematic(script, true, "Enable Kinematic");
        }

        if (GUILayout.Button("Disable Kinematic (All Children)"))
        {
            ModifyKinematic(script, false, "Disable Kinematic");
        }
    }

    private void ModifyKinematic(SetChildRigidbodiesKinematic script, bool state, string actionName)
    {
        Rigidbody[] rigidbodies = script.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            Undo.RecordObject(rb, actionName);
            rb.isKinematic = state;
            EditorUtility.SetDirty(rb);
        }

        Debug.Log($"{actionName} performed on {rigidbodies.Length} Rigidbodies");
        SceneView.RepaintAll();
    }
}