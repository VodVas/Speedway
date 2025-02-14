using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class RaceCarSelector : MonoBehaviour
{
    [SerializeField] private List<RaceCarItem> _allCarsInRace;

    [Inject] private SaveService _saveManager;
    private Racer _playerRacer;

    private void Start()
    {
        if (_allCarsInRace == null || _allCarsInRace.Count == 0)
        {
            Debug.LogWarning("[RaceCarSelector] ������ ����� ����!");
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
            Debug.LogWarning("[RaceCarSelector] LastUsedCarId �� �����, �������� ������ ������ �� ���������.");
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
            Debug.LogWarning($"[RaceCarSelector] ������ � id={lastCarId} �� ������� � ������!");
        }
    }

    private void ActivateCar(int index)
    {
        RaceCarItem item = _allCarsInRace[index];
        item.carObject.SetActive(true);

        if (item.carUpgrades != null)
        {
            // ������������� � ���������� ��������� ���������
            item.carUpgrades.InitializePurchasedUpgrades(_saveManager.HasCarUpgrade);
            item.carUpgrades.ApplyPurchasedStats(
                _saveManager.HasCarUpgrade,
                item.carObject.GetComponent<ArcadeVP.ArcadeVehicleController>(), // ���������, ��� � ��� ���������� ����������
                item.carObject.GetComponent<Health>()
            );
        }

        if (item.carModifications != null)
        {
            // ������������� � ���������� ��������� �����������
            item.carModifications.InitializePurchasedMods(_saveManager.GetCarModificationCount);
            item.carModifications.ApplyPurchasedMods(
                _saveManager.GetCarModificationCount,
                item.carObject.GetComponent<ArcadeVP.ArcadeVehicleController>(), // ���������, ��� � ��� ���������� ����������
                item.carObject.GetComponent<Health>()
            );
        }

        // ��������� ������-�������
        _playerRacer = item.carObject.GetComponent<Racer>();
    }
}






//    private void ActivateCar(int index)
//    {
//        RaceCarItem item = _allCarsInRace[index];
//        item.carObject.SetActive(true);

//        if (item.carUpgrades != null)
//        {
//            item.carUpgrades.InitializePurchasedUpgrades(_saveManager.HasCarUpgrade);
//        }

//        _playerRacer = item.carObject.GetComponent<Racer>();
//    }
//}