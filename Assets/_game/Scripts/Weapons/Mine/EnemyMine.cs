using ArcadeVP;
using UnityEngine;

public class EnemyMine : Weapon
{
    [SerializeField] private ArcadeAiVehicleController _carController;

    private MineSpawner _mineSpawner;

    private void Awake()
    {
        _mineSpawner = GetComponent<MineSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Vehicle _) && _carController.IsGrounded())
        {
            _mineSpawner.StartSpawn(transform.position);
        }
    }
}