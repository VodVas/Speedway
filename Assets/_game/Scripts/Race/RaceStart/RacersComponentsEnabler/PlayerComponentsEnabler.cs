using ArcadeVP;
using UnityEngine;

public class PlayerComponentsEnabler : BaseComponentsEnabler
{
    [SerializeField] private MonoBehaviour[] _playerWeapon;

    private ArcadeVehicleController _carController;

    protected override void Awake()
    {
        _carController = GetComponent<ArcadeVehicleController>();
    }

    protected override void EnableComponents()
    {
        EnableColliders();

        if (_playerWeapon.Length > 0)
        {
            foreach (var weapon in _playerWeapon)
            {
                if (weapon != null)
                {
                    weapon.enabled = true;
                }
            }
        }

        if (_carController != null)
        {
            _carController.enabled = true;
        }
    }
}