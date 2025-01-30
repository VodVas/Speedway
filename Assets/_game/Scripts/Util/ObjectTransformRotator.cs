using UnityEngine;
using Zenject;

public class ObjectTransformRotator : MonoBehaviour
{
    [Inject] private Transform _obj;

    private void Update()
    {
        if (transform.rotation != _obj.transform.rotation)
        {
            transform.rotation = _obj.transform.rotation;
        }
    }
}