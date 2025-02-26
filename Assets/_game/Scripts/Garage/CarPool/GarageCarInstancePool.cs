using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GarageCarInstancePool : MonoBehaviour
{
    [SerializeField] private List<GameObject> _carPrefabs = null;
    [SerializeField] private ComponentsCleaner _physicsCleaner = null;

    [field: SerializeField] public Transform GarageSpawnPoint { get; private set; }

    private readonly List<GameObject> _spawnedCars = new List<GameObject>();

    public List<GameObject> SpawnedCars => _spawnedCars;

    private void Awake()
    {
        if (_carPrefabs == null)
        {
            Debug.LogError("[GarageCarInstancePool] Car prefabs list is null!", this);
            enabled = false;
            return;
        }
        if (GarageSpawnPoint == null)
        {
            Debug.LogError("[GarageCarInstancePool] GarageSpawnPoint is not assigned!", this);
            enabled = false;
            return;
        }
        if (_physicsCleaner == null)
        {
            Debug.LogError("[GarageCarInstancePool] CarPhysicsCleaner is not assigned!", this);
            enabled = false;
            return;
        }
    }

    public IEnumerator SpawnPurchasedCars()
    {
        _spawnedCars.Clear();

        for (int i = 0; i < _carPrefabs.Count; i++)
        {
            GameObject prefab = _carPrefabs[i];

            if (prefab == null)
            {
                Debug.LogWarning($"[GarageCarInstancePool] Prefab at index {i} is null!", this);
                continue;
            }

            CarData data = prefab.GetComponent<CarData>();
            if (data == null)
            {
                Debug.LogWarning($"[GarageCarInstancePool] Missing CarData on prefab '{prefab.name}'!", this);
                continue;
            }

            if (YandexGame.savesData.HasCar(data.Id))
            {
                GameObject instance = Instantiate(prefab, GarageSpawnPoint.position, GarageSpawnPoint.rotation, GarageSpawnPoint);

                _physicsCleaner.RemoveAllPhysicsComponents(instance);
                instance.SetActive(false);
                _spawnedCars.Add(instance);
            }

            yield return null;
        }
    }
}