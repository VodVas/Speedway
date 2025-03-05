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

    protected override bool IsBoss
    {
        get { return false; }
    }

    protected override void EnableComponents()
    {
        if (_carController != null)
            _carController.enabled = true;

        for (int i = 0; i < _playerWeapon.Length; i++)
        {
            if (_playerWeapon[i] != null)
            {
                _playerWeapon[i].enabled = true;
            }
        }
    }
}