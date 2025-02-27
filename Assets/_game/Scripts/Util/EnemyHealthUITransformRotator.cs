using UnityEngine;

public class EnemyHealthUITransformRotator : MonoBehaviour
{
    private Transform _obj;

    private void Start()
    {
        _obj = Camera.main.transform;
    }

    private void Update()
    {
        if (transform.rotation != _obj.transform.rotation)
        {
            transform.rotation = _obj.transform.rotation;
        }
    }
}