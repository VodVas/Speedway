using UnityEngine;

public class ObjectCycleRotator : MonoBehaviour
{
    [SerializeField] private bool _ignoreTimescale = true;

    [field: SerializeField] public float SpeedX;
    [field: SerializeField] public float SpeedY;
    [field: SerializeField] public float SpeedZ;

    private void Update()
    {
        Rotate(GetDeltaTime());
    }

    public void SetRotationSpeeds(float x, float y, float z)
    {
        SpeedX = x;
        SpeedY = y;
        SpeedZ = z;
    }

    private float GetDeltaTime()
    {
        return _ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
    }

    private void Rotate(float deltaTime)
    {
        transform.Rotate(SpeedX * deltaTime, SpeedY * deltaTime, SpeedZ * deltaTime);
    }
}

//public class ObjectCycleRotator : MonoBehaviour
//{
//    [SerializeField] private float _speedX;
//    [SerializeField] private float _speedY;
//    [SerializeField] private float _speedZ;
//    [SerializeField] private bool _ignoreTimescale;

//    private void Update()
//    {
//        StartRotate(GetDeltaTime());
//    }

//    private float GetDeltaTime()
//    {
//        return _ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
//    }

//    public void StartRotate(float deltaTime)
//    {
//        transform.Rotate(_speedX * deltaTime, _speedY * deltaTime, _speedZ * deltaTime);
//    }
//}





//public class ObjectCycleRotator : MonoBehaviour
//{
//    [SerializeField] private float _speedX;
//    [SerializeField] private float _speedY;
//    [SerializeField] private float _speedZ;

//    private void FixedUpdate()
//    {
//        StartRotate(transform, _speedX, _speedY, _speedZ);
//    }

//    public void StartRotate(Transform target, float speedX = 0, float speedY = 0, float speedZ = 0)
//    {
//        target.Rotate(speedX * Time.deltaTime, speedY * Time.deltaTime, speedZ * Time.fixedDeltaTime);
//    }

//    public void StopRotate(Transform target, float speedX = 0, float speedY = 0, float speedZ = 0)
//    {
//        target.Rotate(speedX * Time.deltaTime, speedY * Time.deltaTime, speedZ * Time.fixedDeltaTime);
//    }
//}