using ArcadeVP;
using UnityEngine;

public class NearestOnTriggerRespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ArcadeVehicleController carController))
        {
            carController.TeleportToNearestRespawnPoint();
        }
    }
}