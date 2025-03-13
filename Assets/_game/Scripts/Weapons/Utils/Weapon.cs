using UnityEngine;

//public abstract class Weapon : MonoBehaviour, IWeapon
//{
//    [field: SerializeField, Range(0, 100)] public float DamageAmount { get; private set; } = 10;

//    protected virtual void Awake() { }

//    protected virtual void Update() { }

//    protected virtual void HandleShooting() { }
//}

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [field: SerializeField, Range(0, 100)] public float DamageAmount { get; private set; } = 10;
    [field: SerializeField] public Vehicle OwnerVehicle { get; private set; }

    protected virtual void Awake() { }
    protected virtual void Update() { }
    protected virtual void HandleShooting() { }
}