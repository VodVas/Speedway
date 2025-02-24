using UnityEngine;

public class CarSelectionCycler
{
    private readonly CarInstancePool _pool;
    private readonly CarPurchaseValidator _validator;

    public int CurrentIndex { get; private set; } = 0;


    public CarSelectionCycler(CarInstancePool pool, CarPurchaseValidator validator)
    {
        _pool = pool;
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
        CurrentIndex = (CurrentIndex + direction + _validator.AvailableCarIndices.Count) % _validator.AvailableCarIndices.Count;
        SetCarActive(true);
    }

    public void SetCarActive(bool state)
    {
        if (CurrentIndex < 0 || CurrentIndex >= _validator.AvailableCarIndices.Count)
            return;

        int realIndex = _validator.AvailableCarIndices[CurrentIndex];
        GameObject instance = _pool.GetCarInstance(realIndex);
        instance.transform.position = _pool.SpawnPoint.position;
        instance.SetActive(state);
    }

    public CarData GetCurrentCarData()
    {
        if (CurrentIndex < 0 || _validator.AvailableCarIndices.Count == 0)
            return null;

        int realIndex = _validator.AvailableCarIndices[CurrentIndex];
        return _pool.GetCarInstance(realIndex).GetComponent<CarData>();
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