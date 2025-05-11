#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BossAccessHandler))]
public class RespectAddTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BossAccessHandler handler = (BossAccessHandler)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Test Respect", GUILayout.Height(30)))
        {
            handler.RespectTest();
        }
    }
}
#endif