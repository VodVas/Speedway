using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInstancePool : MonoBehaviour
{
    [SerializeField] private List<GameObject> _carPrefabs = null;
    [SerializeField] private ComponentsCleaner _physicsCleaner = null;

    [field: SerializeField] public Transform SpawnPoint { get; private set; }

    private readonly List<GameObject> _spawnedInstances = new List<GameObject>();

    public List<GameObject> SpawnedInstances => _spawnedInstances;
    public GameObject GetCarInstance(int index) => _spawnedInstances[index];

    private void Awake()
    {
        if (_carPrefabs == null)
        {
            Debug.LogError("[CarInstancePool] _carPrefabs is null!", this);
            enabled = false;
            return;
        }
        if (SpawnPoint == null)
        {
            Debug.LogError("[CarInstancePool] SpawnPoint is not assigned!", this);
            enabled = false;
            return;
        }
        if (_physicsCleaner == null)
        {
            Debug.LogError("[CarInstancePool] CarPhysicsCleaner is not assigned!", this);
            enabled = false;
            return;
        }
    }

    public IEnumerator SpawnAllCars()
    {
        _spawnedInstances.Clear();

        for (int i = 0; i < _carPrefabs.Count; i++)
        {
            GameObject prefab = _carPrefabs[i];
            if (prefab == null)
            {
                Debug.LogWarning($"[CarInstancePool] Prefab at index {i} is null!", this);
                continue;
            }

            GameObject instance = Instantiate(prefab, SpawnPoint);

            _physicsCleaner.RemoveAllPhysicsComponents(instance);
            instance.SetActive(false);
            _spawnedInstances.Add(instance);

            yield return null;
        }
    }
}