using UnityEngine;

public class GarageCarSelectionCycler
{
    private readonly GarageCarInstancePool _pool;

    public int CurrentIndex { get; private set; }

    public GarageCarSelectionCycler(GarageCarInstancePool pool)
    {
        _pool = pool;

        if (_pool.SpawnedCars.Count == 0)
        {
            CurrentIndex = -1;
        }
        else
        {
            CurrentIndex = 0;
        }
    }

    public void SetCurrentIndex(int newIndex)
    {
        if (_pool.SpawnedCars.Count == 0)
        {
            CurrentIndex = -1;
            return;
        }

        if (newIndex < 0 || newIndex >= _pool.SpawnedCars.Count)
        {
            Debug.LogWarning("[GarageCarSelectionCycler] Некорректный индекс, пропускаем.");
            return;
        }

        CurrentIndex = newIndex;
    }

    public void SwitchCar(int direction)
    {
        if (_pool.SpawnedCars.Count == 0)
            return;

        SetCarActive(false);

        CurrentIndex = (CurrentIndex + direction + _pool.SpawnedCars.Count) % _pool.SpawnedCars.Count;

        SetCarActive(true);
    }

    public void SetCarActive(bool state)
    {
        if (CurrentIndex < 0 || CurrentIndex >= _pool.SpawnedCars.Count)
            return;

        var instance = _pool.SpawnedCars[CurrentIndex];
        instance.transform.position = _pool.GarageSpawnPoint.position;
        instance.gameObject.SetActive(state);
    }

    public CarData GetCurrentCarData()
    {
        if (CurrentIndex < 0 || _pool.SpawnedCars.Count == 0)
            return null;

        GameObject carObj = _pool.SpawnedCars[CurrentIndex];

        if (carObj == null)
            return null;

        return carObj.GetComponent<CarData>();
    }
}