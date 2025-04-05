using UnityEngine;
using System.Collections.Generic;

public class CarSelectionCycler
{
    private readonly List<GameObject> _cars;
    private readonly CarPurchaseValidator _validator;
    private readonly CarCollection _carCollection;

    public int CurrentIndex { get; private set; } = 0;

    public CarSelectionCycler(List<GameObject> cars, CarPurchaseValidator validator, CarCollection carCollection)
    {
        _cars = cars;
        _validator = validator;
        _carCollection = carCollection;

        if (_validator.AvailableCarIndices.Count == 0)
        {
            CurrentIndex = -1;
        }
    }

    public void SwitchCar(int direction)
    {
        if (_validator.AvailableCarIndices.Count == 0 || Mathf.Abs(direction) != 1)
            return;

        SetCarActive(false);

        CurrentIndex = (_validator.AvailableCarIndices.Count + CurrentIndex + direction)
                       % _validator.AvailableCarIndices.Count;

        SetCarActive(true);
    }

    //public void SwitchCar(int direction)
    //{
    //    if (_validator.AvailableCarIndices.Count == 0)
    //        return;

    //    SetCarActive(false);

    //    CurrentIndex = (CurrentIndex + direction + _validator.AvailableCarIndices.Count)
    //                   % _validator.AvailableCarIndices.Count;

    //    SetCarActive(true);
    //}

    public void SetCarActive(bool state)
    {
        foreach (var car in _cars)
        {
            car.SetActive(false);
        }

        if (state && CurrentIndex >= 0 && CurrentIndex < _validator.AvailableCarIndices.Count)
        {
            int realIndex = _validator.AvailableCarIndices[CurrentIndex];
            _cars[realIndex].SetActive(true);
        }
    }

    public CarData GetCurrentCarData()
    {
        if (CurrentIndex < 0 ||
            _validator.AvailableCarIndices.Count == 0 ||
            CurrentIndex >= _validator.AvailableCarIndices.Count)
            return null;

        int realIndex = _validator.AvailableCarIndices[CurrentIndex];
        return _cars[realIndex].GetComponent<CarData>();
    }

    //public CarData GetCurrentCarData()
    //{
    //    if (CurrentIndex < 0 || _validator.AvailableCarIndices.Count == 0)
    //        return null;

    //    int realIndex = _validator.AvailableCarIndices[CurrentIndex];
    //    GameObject car = _cars[realIndex];
    //    return car != null ? car.GetComponent<CarData>() : null;
    //}

    public void RevalidateCurrentIndex()
    {
        if (_validator.AvailableCarIndices.Count == 0)
        {
            CurrentIndex = -1;
            return;
        }

        CurrentIndex = Mathf.Clamp(
            CurrentIndex,
            0,
            Mathf.Max(0, _validator.AvailableCarIndices.Count - 1)
        );
    }

    //public void RevalidateCurrentIndex()
    //{
    //    if (_validator.AvailableCarIndices.Count == 0)
    //    {
    //        CurrentIndex = -1;
    //        return;
    //    }

    //    if (CurrentIndex >= _validator.AvailableCarIndices.Count)
    //    {
    //        CurrentIndex = _validator.AvailableCarIndices.Count - 1;
    //    }
    //}

    public bool IsCurrentCarEpic()
    {
        if (_carCollection == null || _validator == null || _validator.AvailableCarIndices.Count == 0)
            return false;

        CarData currentCar = GetCurrentCarData();
        if (currentCar == null)
            return false;

        return _carCollection.IsCarEpic(currentCar.Id);
    }
}