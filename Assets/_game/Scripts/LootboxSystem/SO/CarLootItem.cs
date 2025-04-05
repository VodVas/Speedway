using UnityEngine;

[CreateAssetMenu(menuName = "Loot System/Car Loot Item")]
public class CarLootItem : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public GameObject CarPrefab { get; private set; }

    [SerializeField] public bool IsUnlocked { get; private set; } = false;
    [SerializeField] public int CarId { get; private set; }

    private void Awake()
    {
        if (CarPrefab == null)
        {
            Debug.LogError($"[CarLootItem] CarPrefab not assign!", this);
            return;
        }

        if (CarPrefab.TryGetComponent(out CarData carData))
        {
            CarId = carData.Id;
        }
        else
        {
            Debug.LogError($"[CarLootItem] CarData have not ID {CarPrefab.name}!", this);
            return;
        }

    }

    public void Unlock()
    {
        IsUnlocked = true;
    }
} 