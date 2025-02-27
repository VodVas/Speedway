using UnityEngine;
using System.Collections.Generic;

public class CarSelectionCycler
{
    private readonly List<GameObject> _cars;
    private readonly CarPurchaseValidator _validator;

    public int CurrentIndex { get; private set; } = 0;

    public CarSelectionCycler(List<GameObject> cars, CarPurchaseValidator validator)
    {
        _cars = cars;
        _validator = validator;

        if (_validator.AvailableCarIndices.Count == 0)
        {
            CurrentIndex = -1;
        }
    }

    public void SwitchCar(int direction)
    {
        if (_validator.AvailableCarIndices.Count == 0)
            return;

        SetCarActive(false);

        CurrentIndex = (CurrentIndex + direction + _validator.AvailableCarIndices.Count)
                       % _validator.AvailableCarIndices.Count;

        SetCarActive(true);
    }

    public void SetCarActive(bool state)
    {
        if (CurrentIndex < 0 || CurrentIndex >= _validator.AvailableCarIndices.Count)
            return;

        int realIndex = _validator.AvailableCarIndices[CurrentIndex];

        GameObject car = _cars[realIndex];

        if (car != null)
        {
            car.SetActive(state);
        }
    }

    public CarData GetCurrentCarData()
    {
        if (CurrentIndex < 0 || _validator.AvailableCarIndices.Count == 0)
            return null;

        int realIndex = _validator.AvailableCarIndices[CurrentIndex];
        GameObject car = _cars[realIndex];
        return car != null ? car.GetComponent<CarData>() : null;
    }

    public void RevalidateCurrentIndex()
    {
        if (_validator.AvailableCarIndices.Count == 0)
        {
            CurrentIndex = -1;
            return;
        }

        if (CurrentIndex >= _validator.AvailableCarIndices.Count)
        {
            CurrentIndex = _validator.AvailableCarIndices.Count - 1;
        }
    }
}