using System;
using UnityEngine;

public class LootCar : MonoBehaviour, ITerminatable
{
    private GameObject _currentCar;

    public event Action<ITerminatable> Terminated;

    public void Initialize(GameObject carPrefab)
    {
        foreach (Transform child in transform)
        {
            if (child.name == carPrefab.name)
            {
                _currentCar = child.gameObject;
                _currentCar.SetActive(true);

                return;
            }
        }

        Debug.Log($"[LootCar] Не найдена машина {carPrefab.name} среди дочерних объектов!", this);
        enabled = false;
        return;
    }

    public void Terminate()
    {
        if (_currentCar != null)
        {
            _currentCar.SetActive(false);
            _currentCar = null;
        }

        Terminated?.Invoke(this);
    }
}