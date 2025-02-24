using UnityEngine;

public class GarageCarSelectionCycler
{
    private readonly GarageCarInstancePool _pool;

    public int CurrentIndex { get; private set; } = 0;

    public GarageCarSelectionCycler(GarageCarInstancePool pool)
    {
        _pool = pool;

        if (_pool.SpawnedCars.Count == 0)
        {
            CurrentIndex = -1;
        }
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
        instance.transform.position = _pool.GarageSpawnPoint.position; // При желании можно менять позицию
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
