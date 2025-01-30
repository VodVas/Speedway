using UnityEngine;

public class ObjectCycleRotator : MonoBehaviour
{
    private float _speedX;
    private float _speedY;
    private float _speedZ;

    private void Update()
    {
        StartRotate(transform, _speedX, _speedY, _speedZ);
    }

    public void StartRotate(Transform target, float speedX = 0, float speedY = 0, float speedZ = 0)
    {
        target.Rotate(speedX * Time.deltaTime, speedY * Time.deltaTime, speedZ * Time.deltaTime);
    }

    public void StopRotate(Transform target, float speedX = 0, float speedY = 0, float speedZ = 0)
    {
        target.Rotate(speedX * Time.deltaTime, speedY * Time.deltaTime, speedZ * Time.deltaTime);
    }
}