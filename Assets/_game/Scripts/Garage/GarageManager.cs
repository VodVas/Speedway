using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System;

public class GarageManager : MonoBehaviour
{
    [Serializable]
    public class GarageCarItem
    {
        public int carId;
        public GameObject carObject;
    }

    [SerializeField] private List<GarageCarItem> _garageCars;

    [Inject] private SaveManager _saveManager;

    private void Start()
    {
        foreach (var car in _garageCars)
        {
            if (car.carObject != null)
                car.carObject.SetActive(false);
        }

        foreach (var car in _garageCars)
        {
            if (_saveManager.HasCar(car.carId))
            {
                car.carObject.SetActive(true);
            }
        }
    }
}