using ArcadeVP;
using UnityEngine;

public class PlayerMine : Weapon
{
    [SerializeField] private ArcadeVehicleController _carController;

    private MineSpawner _mineSpawner;

    private void Awake()
    {
        _mineSpawner = GetComponent<MineSpawner>();
    }

    protected override void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.X) && _carController.IsGrounded())
        {
            _mineSpawner.StartSpawn(transform.position);
        }
    }
}