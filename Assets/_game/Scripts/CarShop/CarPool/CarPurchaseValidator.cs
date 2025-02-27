using System.Collections.Generic;
using UnityEngine;

public class CarPurchaseValidator
{
    private readonly List<CarData> _carDataList = new List<CarData>();

    public List<int> AvailableCarIndices { get; private set; } = new List<int>();

    public CarPurchaseValidator(List<GameObject> cars)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            var obj = cars[i];

            if (obj == null) continue;

            CarData data = obj.GetComponent<CarData>();

            if (data == null) continue;

            _carDataList.Add(data);
        }

        RecalculateAvailability();
    }

    public void RecalculateAvailability()
    {
        AvailableCarIndices.Clear();

        for (int i = 0; i < _carDataList.Count; i++)
        {
            CarData data = _carDataList[i];

            if (!YG.YandexGame.savesData.HasCar(data.Id))
            {
                AvailableCarIndices.Add(i);
            }
        }
    }

    public bool TryBuyCar(int localIndex)
    {
        if (localIndex < 0 || localIndex >= AvailableCarIndices.Count)
            return false;

        int realIndex = AvailableCarIndices[localIndex];
        CarData data = _carDataList[realIndex];

        if (!YG.YandexGame.savesData.TrySpendMoney(data.Price))
        {
            return false;
        }

        return true;
    }
}