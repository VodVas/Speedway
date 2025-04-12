using UnityEngine;
using System.Collections.Generic;

public class GarageCarSelectionCycler
{
    private readonly List<GameObject> _carsInScene;

    public int CurrentIndex { get; private set; }

    public GarageCarSelectionCycler(List<GameObject> carsInScene)
    {
        _carsInScene = carsInScene;

        if (_carsInScene == null || _carsInScene.Count == 0)
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
        if (_carsInScene == null || _carsInScene.Count == 0)
        {
            CurrentIndex = -1;
            return;
        }

        if (newIndex < 0 || newIndex >= _carsInScene.Count)
        {
            Debug.LogWarning("[GarageCarSelectionCycler] Некорректный индекс, пропускаем.");
            return;
        }

        CurrentIndex = newIndex;
    }

    public void SwitchCar(int direction)
    {
        if (_carsInScene == null || _carsInScene.Count == 0)
            return;

        SetCarActive(false);

        CurrentIndex = (CurrentIndex + direction + _carsInScene.Count) % _carsInScene.Count;

        SetCarActive(true);
    }

    public void SetCarActive(bool state)
    {
        if (CurrentIndex < 0 || _carsInScene == null || CurrentIndex >= _carsInScene.Count)
            return;

        GameObject instance = _carsInScene[CurrentIndex];
        if (instance == null) return;

        instance.SetActive(state);

        if (state)
        {
            var carMods = instance.GetComponent<CarModifications>();
            carMods?.ForceInitialize();
        }
    }

    public CarData GetCurrentCarData()
    {
        if (_carsInScene == null || _carsInScene.Count == 0 || CurrentIndex < 0)
            return null;

        GameObject carObj = _carsInScene[CurrentIndex];
        if (carObj == null)
            return null;

        return carObj.GetComponent<CarData>();
    }
}