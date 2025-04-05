using UnityEngine;
using System.Collections.Generic;
using YG;

public class CarPurchaseValidator
{
    private readonly List<GameObject> _cars;
    private readonly List<int> _availableCarIndices = new();
    private CarCollection _carCollection;

    public IReadOnlyList<int> AvailableCarIndices => _availableCarIndices;

    public CarPurchaseValidator(List<GameObject> cars, CarCollection carCollection)
    {
        _cars = cars;
        _carCollection = carCollection;

        RecalculateAvailability();
    }

    public void RecalculateAvailability()
    {
        _availableCarIndices.Clear();

        for (int i = 0; i < _cars.Count; i++)
        {
            if (_cars[i].TryGetComponent(out CarData carData))
            {
                bool isEpic = _carCollection.IsCarEpic(carData.Id);

                if (!YandexGame.savesData.HasCar(carData.Id))
                {
                    _availableCarIndices.Add(i);
                }
            }
        }
    }

    public bool TryBuyCar(int index)
    {
        if (index < 0 || index >= _cars.Count)
            return false;

        if (!_availableCarIndices.Contains(index))
            return false;

        if (_cars[index].TryGetComponent(out CarData carData))
        {
            Debug.Log($"Покупка: {carData.CarName}, цена: {carData.Price}, денег: {YandexGame.savesData.Money}");
            if (YandexGame.savesData.Money >= carData.Price)
            {
                YandexGame.savesData.TrySpendMoney(carData.Price);
                YandexGame.savesData.AddCar(carData.Id);
                YandexGame.SaveProgress();
                RecalculateAvailability();
                return true;
            }
        }

        return false;
    }
}