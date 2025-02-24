using System.Collections.Generic;
using UnityEngine;
using YG;

public class CarPurchaseValidator
{
    private readonly List<int> _availableCarIndices = new List<int>();
    private readonly List<GameObject> _allCars;

    public IReadOnlyList<int> AvailableCarIndices => _availableCarIndices;

    public CarPurchaseValidator(List<GameObject> allCars)
    {
        _allCars = allCars ?? new List<GameObject>();
        RecalculateAvailability();
    }

    public void RecalculateAvailability()
    {
        _availableCarIndices.Clear();

        for (int i = 0; i < _allCars.Count; i++)
        {
            CarData data = _allCars[i].GetComponent<CarData>();
            if (data == null)
            {
                Debug.LogError("[CarPurchaseValidator] CarData не найден на заспавненном объекте!", _allCars[i]);
                continue;
            }

            if (!YandexGame.savesData.HasCar(data.Id))
            {
                _availableCarIndices.Add(i);
            }
        }
    }

    public bool TryBuyCar(int currentSelectionIndex)
    {
        if (currentSelectionIndex < 0 || currentSelectionIndex >= _availableCarIndices.Count)
        {
            Debug.LogError("[CarPurchaseValidator] Некорректный индекс для покупки.");
            return false;
        }

        int realIndex = _availableCarIndices[currentSelectionIndex];
        CarData carData = _allCars[realIndex].GetComponent<CarData>();

        if (carData == null)
        {
            Debug.LogError("[CarPurchaseValidator] Данные машины не найдены!");
            return false;
        }

        bool success = YandexGame.savesData.TrySpendMoney(carData.Price);

        if (!success)
        {
            Debug.Log("[CarPurchaseValidator] Недостаточно средств для покупки!");
        }
        return success;
    }
}