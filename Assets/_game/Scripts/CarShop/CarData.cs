using UnityEngine;

public class CarData : MonoBehaviour
{
    [field: SerializeField] public string CarName { get; private set; } = "Noname";
    [field: SerializeField] public int Price { get; private set; } = 100;
    [field: SerializeField] public int Id { get; private set; } = 0;
    [field: SerializeField] public int Speed { get; private set; } = 0;
    [field: SerializeField] public int Acceleration { get; private set; } = 0;
    [field: SerializeField] public int Turn { get; private set; } = 0;
    [field: SerializeField] public int Armor { get; private set; } = 0;

    private void Awake()
    {
        if (Id < 0)
        {
            Debug.LogError($"[CarData] Invalid Id: {Id}", this);
            enabled = false;
            return;
        }
        if (Price < 0)
        {
            Debug.LogError($"[CarData] Price cannot be negative: {Price}", this);
            enabled = false;
            return;
        }
    }
}