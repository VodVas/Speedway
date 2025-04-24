using ArcadeVP;
using UnityEngine;

public class PlayerMine : MineWeapon
{
    [SerializeField] private ArcadeVehicleController _carController;

    protected override bool IsMineReadyToSpawn()
    {
        return Input.GetKeyDown(KeyCode.X) && _carController.IsGrounded();
    }
}