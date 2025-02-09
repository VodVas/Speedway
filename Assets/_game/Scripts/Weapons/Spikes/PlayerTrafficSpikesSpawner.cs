using ArcadeVP;
using UnityEngine;

public class PlayerTrafficSpikesSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spikes;
    [SerializeField] private ArcadeVehicleController _carController;
    [SerializeField] private float _offSetY = 0.3f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && _carController.IsGrounded())
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += _offSetY;

            Quaternion carRotation = transform.rotation;
            float yRotation = carRotation.eulerAngles.y;
            Quaternion spikesRotation = Quaternion.Euler(0, yRotation, 0);

            Instantiate(_spikes, spawnPosition, spikesRotation);
        }
    }
}