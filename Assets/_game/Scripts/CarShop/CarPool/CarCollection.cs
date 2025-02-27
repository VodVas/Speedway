using System.Collections.Generic;
using UnityEngine;

public class CarCollection : MonoBehaviour
{
    [SerializeField] private List<GameObject> _sceneCars = null;

    public List<GameObject> SceneCars => _sceneCars;

    private void Awake()
    {
        if (_sceneCars == null || _sceneCars.Count == 0)
        {
            Debug.LogError("[CarCollection] Список машин пуст или не назначен!", this);
            enabled = false;
            return;
        }

        for (int i = 0; i < _sceneCars.Count; i++)
        {
            if (_sceneCars[i] == null)
            {
                Debug.LogWarning($"[CarCollection] Пустой элемент списка _sceneCars на индексе {i}!", this);
                continue;
            }

            CarData carData = _sceneCars[i].GetComponent<CarData>();

            if (carData == null)
            {
                Debug.LogError($"[CarCollection] На объекте '{_sceneCars[i].name}' нет CarData!", this);
            }

            _sceneCars[i].SetActive(false);
        }
    }
}