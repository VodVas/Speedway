using UnityEngine;

public class RemoveChildColliders : MonoBehaviour
{
    public void RemoveAllChildColliders()
    {
        Collider[] childColliders = GetComponentsInChildren<Collider>();

        foreach (Collider childCollider in childColliders)
        {
            if (childCollider.transform != transform)
            {
                DestroyImmediate(childCollider.gameObject.GetComponent<Collider>());
            }
        }

        Debug.Log("Remove All Child Colliders");
    }
}
