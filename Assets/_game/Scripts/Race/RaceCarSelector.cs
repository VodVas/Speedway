using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class RaceCarSelector : MonoBehaviour
{
    [SerializeField] private List<RaceCarItem> _allCarsInRace;

    [Inject] private SaveManager _saveManager;
    private Racer _playerRacer;

    private void Start()
    {
        if (_allCarsInRace == null || _allCarsInRace.Count == 0)
        {
            Debug.LogWarning("[RaceCarSelector] Список машин пуст!");
            return;
        }

        DeactivateAllCars();
        ActivateLastUsedCar();
    }

    public Racer GetPlayerRacer()
    {
        return _playerRacer;
    }

    private void DeactivateAllCars()
    {
        for (int i = 0; i < _allCarsInRace.Count; i++)
        {
            if (_allCarsInRace[i] != null && _allCarsInRace[i].carObject != null)
            {
                _allCarsInRace[i].carObject.SetActive(false);
            }
        }
    }

    private void ActivateLastUsedCar()
    {
        int lastCarId = _saveManager.LastUsedCarId;

        if (lastCarId < 0)
        {
            Debug.LogWarning("[RaceCarSelector] LastUsedCarId не задан, включаем первую машину по умолчанию.");
            ActivateCar(0);
            return;
        }

        bool foundCar = false;

        for (int i = 0; i < _allCarsInRace.Count; i++)
        {
            RaceCarItem item = _allCarsInRace[i];

            if (item == null || item.carObject == null)
                continue;

            if (item.carId == lastCarId)
            {
                ActivateCar(i);
                foundCar = true;
                break;
            }
        }

        if (foundCar == false)
        {
            Debug.LogWarning($"[RaceCarSelector] Машина с id={lastCarId} не найдена в списке!");
        }
    }

    private void ActivateCar(int index)
    {
        RaceCarItem item = _allCarsInRace[index];
        item.carObject.SetActive(true);

        if (item.carUpgrades != null)
        {
            item.carUpgrades.InitializePurchasedUpgrades(_saveManager.HasCarUpgrade);
        }

        _playerRacer = item.carObject.GetComponent<Racer>();
    }
}