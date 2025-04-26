#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AssignArmorLayerEditor : EditorWindow
{
    private static readonly int ArmorLayer = LayerMask.NameToLayer("Armor");
    [SerializeField] private Material _targetMaterial;

    [MenuItem("Tools/Apocalypse/Assign Armor Layer")]
    public static void ShowWindow()
    {
        GetWindow<AssignArmorLayerEditor>("Assign Armor Layer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Assign Armor Layer to Objects", EditorStyles.boldLabel);

        _targetMaterial = (Material)EditorGUILayout.ObjectField(
            "Target Material",
            _targetMaterial,
            typeof(Material),
            false);

        if (GUILayout.Button("Find and Assign Layer (Current Scene Only)"))
        {
            if (_targetMaterial == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a target material first!", "OK");
                return;
            }
            FindAndAssignLayer();
        }
    }

    private void FindAndAssignLayer()
    {
        if (ArmorLayer == -1)
        {
            Debug.LogError("Layer 'Armor' does not exist! Please create it first.");
            return;
        }

        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        List<GameObject> affectedObjects = new List<GameObject>();

        foreach (GameObject root in rootObjects)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer renderer in renderers)
            {
                foreach (Material mat in renderer.sharedMaterials)
                {
                    if (mat == _targetMaterial)
                    {
                        renderer.gameObject.layer = ArmorLayer;
                        affectedObjects.Add(renderer.gameObject);
                        EditorUtility.SetDirty(renderer.gameObject);
                        break;
                    }
                }
            }
        }

        Debug.Log($"Assigned 'Armor' layer to {affectedObjects.Count} objects with material '{_targetMaterial.name}'");

        if (affectedObjects.Count > 0)
        {
            Selection.objects = affectedObjects.ToArray();
        }
        else
        {
            Debug.Log("No objects with the specified material found in current scene.");
        }
    }
}
#endif