using UnityEngine;

public class ParticleWeaponMarker : MonoBehaviour
{
    public IWeapon WeaponRef { get; private set; }

    public void SetWeapon(IWeapon weapon)
    {
        WeaponRef = weapon;
    }
}