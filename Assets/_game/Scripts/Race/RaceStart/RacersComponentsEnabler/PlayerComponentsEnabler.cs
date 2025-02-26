using ArcadeVP;
using UnityEngine;

[RequireComponent(typeof(ArcadeVehicleController))]
public class PlayerComponentsEnabler : BaseComponentsEnabler
{
    [SerializeField] private Weapon[] _playerWeapon;

    private ArcadeVehicleController _carController;

    private void Awake()
    {
        _carController = GetComponent<ArcadeVehicleController>();
    }

    protected override void EnableComponents()
    {
        if (_carController != null)
            _carController.enabled = true;

        EnableColliders();

        foreach (var weapon in _playerWeapon)
        {
            if (weapon != null)
                weapon.enabled = true;
        }
    }
}