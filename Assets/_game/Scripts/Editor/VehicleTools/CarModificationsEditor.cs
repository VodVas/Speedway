#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(CarModifications))]
public class CarModificationsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUIStyle colorfulButtonStyle = new GUIStyle(GUI.skin.button)
        {
            richText = true,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 12,
            fontStyle = FontStyle.Bold
        };

        string buttonText = "Go <color=#FF6B6B>easy</color> <color=#4ECDC4>add</color> <color=#45B7D1>mode</color>";

        if (GUILayout.Button(buttonText, colorfulButtonStyle, GUILayout.Height(30)))
        {
            FindAndAssignModifications((CarModifications)target);
        }
    }

    private void FindAndAssignModifications(CarModifications carModifications)
    {
        if (carModifications == null)
        {
            Debug.LogError("Target object is null!");
            return;
        }

        Undo.RecordObject(carModifications, "Auto-fill Modifications");

        Transform targetTransform = carModifications.transform;
        Transform modificationsParent = FindChildByName(targetTransform, "Modifications NewSystem");

        if (modificationsParent == null)
        {
            Debug.LogError($"Parent 'Modifications NewSystem' not found in {targetTransform.name}", carModifications);
            return;
        }

        List<BaseCarModification> modifications = new List<BaseCarModification>();
        ColorCarModification colorModification = null;

        foreach (Transform child in modificationsParent)
        {
            if (child == null) continue;

            BaseCarModification modification = child.GetComponent<BaseCarModification>();

            if (modification == null)
            {
                Debug.LogWarning($"Missing component on {child.name}", child.gameObject);
                continue;
            }

            modifications.Add(modification);

            if (child.name.Equals("Color"))
            {
                colorModification = modification as ColorCarModification;
                if (colorModification == null)
                {
                    Debug.LogError($"Color object has wrong component", child.gameObject);
                }
            }
        }

        if (modifications.Count == 0 || carModifications == null)
        {
            Debug.LogWarning("No valid modifications found", carModifications);
            return;
        }

        SerializedObject serializedObject = new SerializedObject(carModifications);
        if (serializedObject == null)
        {
            Debug.LogError("Failed to create SerializedObject");
            return;
        }

        SerializedProperty modsProperty = serializedObject.FindProperty("_modifications");
        SerializedProperty colorProperty = serializedObject.FindProperty("_colorModification");

        if (modsProperty == null || colorProperty == null)
        {
            Debug.LogError("Failed to find serialized properties");
            return;
        }

        try
        {
            serializedObject.Update();
            modsProperty.ClearArray();
            modsProperty.arraySize = modifications.Count;

            for (int i = 0; i < modifications.Count; i++)
            {
                if (modifications[i] == null) continue;
                SerializedProperty element = modsProperty.GetArrayElementAtIndex(i);
                element.objectReferenceValue = modifications[i];
            }

            colorProperty.objectReferenceValue = colorModification;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(carModifications);

            Debug.Log($"Success! Added {modifications.Count} modifications", carModifications);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error during serialization: {e.Message}", carModifications);
        }
    }

    private Transform FindChildByName(Transform parent, string targetName)
    {
        if (parent == null) return null;

        foreach (Transform child in parent)
        {
            if (child.name == targetName) return child;
            Transform result = FindChildByName(child, targetName);
            if (result != null) return result;
        }
        return null;
    }
}
#endif