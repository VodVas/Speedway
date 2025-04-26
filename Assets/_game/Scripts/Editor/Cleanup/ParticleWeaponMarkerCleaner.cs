#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ParticleWeaponMarkerCleaner : EditorWindow
{
    [MenuItem("Tools/Apocalypse/Particle damage/Remove Duplicate Markers")]
    static void Init()
    {
        EditorApplication.delayCall += ExecuteCleanup;
    }

    static void ExecuteCleanup()
    {
        try
        {
            EditorUtility.DisplayProgressBar("Cleaning Markers", "Starting cleanup...", 0);

            int processed = 0;
            int removed = 0;
            HashSet<GameObject> visited = new HashSet<GameObject>();
            Scene activeScene = EditorSceneManager.GetActiveScene();

            foreach (GameObject root in activeScene.GetRootGameObjects())
            {
                ProcessHierarchy(root, ref processed, ref removed, visited);
            }

            EditorUtility.ClearProgressBar();
            Debug.Log($"Processed {processed} objects. Removed {removed} duplicate markers");

            if (removed > 0)
            {
                EditorSceneManager.MarkSceneDirty(activeScene);
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    static void ProcessHierarchy(GameObject obj, ref int processed, ref int removed, HashSet<GameObject> visited)
    {
        if (!visited.Add(obj)) return;

        ParticleWeaponMarker[] markers = obj.GetComponents<ParticleWeaponMarker>();
        processed++;

        if (markers.Length > 1)
        {
            for (int i = markers.Length - 1; i >= 1; i--)
            {
                Undo.DestroyObjectImmediate(markers[i]);
                removed++;
            }
        }

        EditorUtility.DisplayProgressBar("Cleaning Markers",
            $"Processing {obj.name}",
            (float)processed / visited.Count);

        foreach (Transform child in obj.transform)
        {
            ProcessHierarchy(child.gameObject, ref processed, ref removed, visited);
        }
    }
}
#endif