#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace ApocalypseTools
{
    public class ShadowWheelEnabler : EditorWindow
    {
        private Transform parent;
        private Vector2 scroll;
        private const string WHEEL_KEYWORD = "Wheel";

        [MenuItem("Tools/Apocalypse/Configure Wheel Shadows")]
        private static void Init()
        {
            var window = GetWindow<ShadowWheelEnabler>("Wheel Shadows");
            window.minSize = new Vector2(350, 100);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            parent = (Transform)EditorGUILayout.ObjectField("Parent Object", parent, typeof(Transform), true);

            GUILayout.Space(20);
            if (GUILayout.Button("Apply Shadows to Wheels", GUILayout.Height(30)))
            {
                if (!parent)
                {
                    Debug.LogError("Parent object not assigned!");
                    return;
                }

                ProcessWheels();
            }
        }

        private void ProcessWheels()
        {
            var renderers = parent.GetComponentsInChildren<MeshRenderer>(true);
            if (renderers.Length == 0) return;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Enable Wheel Shadows");
            int changed = 0;

            foreach (var r in renderers)
            {
                if (!r.name.Contains(WHEEL_KEYWORD)) continue;

                if (r.shadowCastingMode != ShadowCastingMode.On)
                {
                    Undo.RecordObject(r, "Wheel Shadow Modification");
                    r.shadowCastingMode = ShadowCastingMode.On;
                    EditorUtility.SetDirty(r);
                    changed++;
                }
            }

            EditorSceneManager.MarkSceneDirty(parent.gameObject.scene);
            Debug.Log($"Processed {renderers.Length} objects. Enabled shadows on {changed} wheels.");
        }
    }
}
#endif