using ArcadeVP;
using UnityEngine;

public class EnemyMine : MineWeapon
{
    [SerializeField] private ArcadeAiVehicleController _carController;

    private bool _shouldSpawn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Vehicle>(out _) && _carController.IsGrounded())
        {
            _shouldSpawn = true;
        }
    }

    protected override bool IsMineReadyToSpawn()
    {
        bool result = _shouldSpawn;
        _shouldSpawn = false;
        return result;
    }
}

//public class EnemyMine : Weapon
//{
//    [SerializeField] private ArcadeAiVehicleController _carController;

//    private MineSpawner _mineSpawner;

//    private void Awake()
//    {
//        _mineSpawner = GetComponent<MineSpawner>();
//    }

//    protected override void Update()
//    {
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.TryGetComponent(out Vehicle _) && _carController.IsGrounded())
//        {
//            _mineSpawner.StartSpawn(transform.position);
//        }
//    }
//}