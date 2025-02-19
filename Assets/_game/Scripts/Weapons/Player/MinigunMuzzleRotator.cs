using UnityEngine;

public class MinigunMuzzleRotator : MonoBehaviour
{
    [SerializeField] private ParticleSystem _shot;
    [SerializeField] private float _speedX;
    [SerializeField] private float _speedY;
    [SerializeField] private float _speedZ;

    private void Update()
    {
        if (_shot.isPlaying)
        {
            StartRotate();
        }
        else
        {
            StopRotate();
        }
    }

    public void StartRotate()
    {
        transform.Rotate(_speedX * Time.deltaTime, _speedY * Time.deltaTime, _speedZ * Time.deltaTime);
    }

    public void StopRotate()
    {
        transform.Rotate(_speedX * Time.deltaTime, _speedY * Time.deltaTime, _speedZ * Time.deltaTime);
    }
}