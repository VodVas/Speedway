using UnityEngine;

public class ObjectTransformRotator : MonoBehaviour
{
    [SerializeField] private Transform _obj;

    private void Update()
    {
        if (transform.rotation != _obj.transform.rotation)
        {
            transform.rotation = _obj.transform.rotation;
        }
    }
}