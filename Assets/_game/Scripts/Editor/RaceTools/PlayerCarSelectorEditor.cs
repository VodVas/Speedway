#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerCarSelector))]
public class PlayerCarSelectorEditor : Editor
{
    private SerializedProperty _sceneCars;
    private SerializedProperty _carsContainer;
    private const string ExcludedProperties = "m_Script,_carsContainer,_sceneCars";
    private GameObject[] _childBuffer = new GameObject[100];

    private void OnEnable()
    {
        _sceneCars = serializedObject.FindProperty("_sceneCars");
        _carsContainer = serializedObject.FindProperty("_carsContainer");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_carsContainer);

        if (GUILayout.Button("Add cars") && ValidateContainer())
            PopulateCars();

        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_sceneCars, true);

        DrawPropertiesExcluding(serializedObject, ExcludedProperties.Split(','));
        serializedObject.ApplyModifiedProperties();
    }

    private bool ValidateContainer()
    {
        if (_carsContainer.objectReferenceValue != null) return true;
        Debug.LogError("Cars container not assigned!", target);
        return false;
    }

    private void PopulateCars()
    {
        try
        {
            serializedObject.Update();
            _sceneCars.ClearArray();

            Transform container = (Transform)_carsContainer.objectReferenceValue;
            int count = CacheChildrenNonAlloc(container);

            for (int i = 0; i < count; i++)
            {
                GameObject child = _childBuffer[i];
                if (!child.TryGetComponent(out CarData data)) continue;

                AddCarEntry(child, data);
            }
        }
        finally
        {
            serializedObject.ApplyModifiedProperties();
            ClearChildBuffer();
        }
    }

    private int CacheChildrenNonAlloc(Transform container)
    {
        int count = container.childCount;
        if (_childBuffer.Length < count)
            System.Array.Resize(ref _childBuffer, count);

        for (int i = 0; i < count; i++)
            _childBuffer[i] = container.GetChild(i).gameObject;

        return count;
    }

    private void AddCarEntry(GameObject obj, CarData data)
    {
        int index = _sceneCars.arraySize;
        _sceneCars.InsertArrayElementAtIndex(index);

        SerializedProperty element = _sceneCars.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("_carData").objectReferenceValue = data;
        element.FindPropertyRelative("_carObject").objectReferenceValue = obj;
    }

    private void ClearChildBuffer()
    {
        for (int i = 0; i < _childBuffer.Length; i++)
            _childBuffer[i] = null;
    }
}
#endif