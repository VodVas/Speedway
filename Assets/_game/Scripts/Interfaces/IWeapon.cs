//public interface IWeapon
//{
//    public float DamageAmount { get; }
//}

public interface IWeapon
{
    public float DamageAmount { get; }
    public Vehicle OwnerVehicle { get; }
}