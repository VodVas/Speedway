#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraPrioritySwitcher))]
public class CameraPrioritySwitcherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraPrioritySwitcher switcher = (CameraPrioritySwitcher)target;

        if (GUILayout.Button("Find All Virtual Cameras"))
        {
            switcher.FindAllCameras();
            EditorUtility.SetDirty(switcher);
        }

        if (GUILayout.Button("Print Active Cameras"))
        {
            PrintActiveCameras(switcher);
        }
    }

    private void PrintActiveCameras(CameraPrioritySwitcher switcher)
    {
        var cameras = switcher.GetCameras();

        if (cameras == null || cameras.Length == 0)
        {
            Debug.Log("No cameras assigned. Click 'Find All Virtual Cameras' first.");
            return;
        }

        int activeCount = 0;

        foreach (var cam in cameras)
        {
            if (cam != null && cam.gameObject.activeInHierarchy)
            {
                activeCount++;
                Debug.Log($"Active camera: {cam.name} (Priority: {cam.Priority})", cam);
            }
        }

        if (activeCount == 0)
        {
            Debug.Log("No active cameras found (but cameras are assigned)");
        }
        else
        {
            Debug.Log($"Total active cameras: {activeCount}");
        }
    }
}
#endif