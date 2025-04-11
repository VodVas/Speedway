using UnityEngine;
using System.Collections.Generic;
using YG;

[CreateAssetMenu(menuName = "Apocalypse//Loot System/Car Loot Database")]
public class CarLootDatabase : ScriptableObject
{
    [SerializeField] private List<CarLootItem> _epicItems = new();

    private Dictionary<Rarity, List<CarLootItem>> _rarityMap;

    public void Initialize()
    {
        _rarityMap = new Dictionary<Rarity, List<CarLootItem>>
        {
            { Rarity.Epic, _epicItems }
        };
    }

    public CarLootItem GetRandomItem(Rarity rarity)
    {
        if (_rarityMap.TryGetValue(rarity, out List<CarLootItem> items) && items.Count > 0)
        {
            return items[Random.Range(0, items.Count)];
        }
        return null;
    }

    public bool IsCarUnlocked(int carId)
    {
        return YandexGame.savesData.GetUnlockedEpicCars().Contains(carId);
    }

    public bool IsCarEpic(int carId)
    {
        //if (carId < 12)
        //{
        //    return false;
        //}

        return _epicItems.Exists(item => item.CarId == carId);
    }
} 