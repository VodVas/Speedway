using System.Collections.Generic;
using UnityEngine;

public class CarCollection : MonoBehaviour
{
    [SerializeField] private List<GameObject> _sceneCars = new List<GameObject>();
    [SerializeField] private CarLootDatabase _epicCarsDatabase;

    public List<GameObject> SceneCars => _sceneCars;

    private void Awake()
    {
        if (_sceneCars == null || _sceneCars.Count == 0)
        {
            Debug.LogError("[CarCollection] Список машин пуст или не инициализирован!", this);
            enabled = false;
            return;
        }

        for (int i = 0; i < _sceneCars.Count; i++)
        {
            if (_sceneCars[i] == null)
            {
                Debug.LogWarning($"[CarCollection] Обнаружен null элемент в _sceneCars по индексу {i}!", this);
                continue;
            }

            CarData carData = _sceneCars[i].GetComponent<CarData>();

            if (carData == null)
            {
                Debug.LogError($"[CarCollection] На объекте '{_sceneCars[i].name}' отсутствует компонент CarData!", this);
            }

            _sceneCars[i].SetActive(false);
        }
    }

    public bool IsCarEpic(int carId)
    {
        if (_epicCarsDatabase == null)
        {
            Debug.LogWarning("[CarCollection] База данных эпических машин не назначена!", this);
            return false;
        }

        return _epicCarsDatabase.IsCarEpic(carId);
    }
}