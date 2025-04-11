#if UNITY_EDITOR
using UnityEngine;

public class ObjectScriptCleaner : MonoBehaviour
{
    public void CleanupHierarchy()
    {
        CleanObjectAndChildren(transform);
        DeleteGameObjectByName("CM vcam1");
        DeleteGameObjectByName("Skid marks FL");

        UnityEditor.EditorUtility.SetDirty(gameObject);
    }

    void CleanObjectAndChildren(Transform target)
    {
        for (int i = 0; i < 2; i++)
        {
            CleanComponents(target.gameObject);

            foreach (Transform child in target)
            {
                CleanObjectAndChildren(child);
            }
        }
    }

    void CleanComponents(GameObject target)
    {
        Component[] components = target.GetComponents<Component>();

        foreach (Component component in components)
        {
            if (ShouldKeepComponent(component)) continue;

            if (Application.isPlaying)
                Destroy(component);
            else
                DestroyImmediate(component);
        }

        Joint[] joints = target.GetComponents<Joint>();
        foreach (var joint in joints)
        {
            if (Application.isPlaying)
                Destroy(joint);
            else
                DestroyImmediate(joint);
        }

        AudioSource[] audioSources = target.GetComponents<AudioSource>();

        foreach (var audioSource in audioSources)
        {
            if (Application.isPlaying)
                Destroy(audioSource);
            else
                DestroyImmediate(audioSource);
        }

        Rigidbody[] rigidbodies = target.GetComponents<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            if (Application.isPlaying)
                Destroy(rb);
            else
                DestroyImmediate(rb);
        }

        Collider[] colliders = target.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            if (Application.isPlaying)
                Destroy(collider);
            else
                DestroyImmediate(collider);
        }
    }

    bool ShouldKeepComponent(Component component)
    {
        if (component is ObjectScriptCleaner)
            return true;

        if (component is Transform || component is RectTransform)
            return true;

        if (component.GetType() == typeof(CarData))
            return true;

        if (!(component is MonoBehaviour))
            return true;

        var type = component.GetType();
        return type.Assembly.FullName.Contains("UnityEngine") ||
               type.Assembly.FullName.Contains("UnityEditor");
    }

    void DeleteGameObjectByName(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
        {
            DestroyImmediate(obj);
        }
    }
}
#endif