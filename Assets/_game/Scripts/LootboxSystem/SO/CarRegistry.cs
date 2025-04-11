using UnityEngine;

[CreateAssetMenu(fileName = "CarRegistry", menuName = "Apocalypse/Car Registry")]
public class CarRegistry : ScriptableObject
{
    [field: SerializeField] public CarModifications[] Cars;
}