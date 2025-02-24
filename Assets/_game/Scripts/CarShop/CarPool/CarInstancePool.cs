using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInstancePool : MonoBehaviour
{
    [SerializeField] private List<GameObject> _carPrefabs;
    [SerializeField] private CarInstanceModifier _physicsOptimizer;

    [field: SerializeField] public Transform SpawnPoint { get; private set; }

    private List<GameObject> _spawnedInstances = new List<GameObject>();

    public List<GameObject> SpawnedInstances => _spawnedInstances;
    public GameObject GetCarInstance(int index) => _spawnedInstances[index];
    public List<GameObject> GetAllCarsList() => _spawnedInstances;
    public int TotalCars => _spawnedInstances.Count;

    public IEnumerator SpawnAllCars()
    {
        _spawnedInstances.Clear();

        foreach (var prefab in _carPrefabs)
        {
            var instance = Instantiate(prefab, SpawnPoint);
            _physicsOptimizer.OptimizeCar(instance);
            instance.SetActive(false);
            _spawnedInstances.Add(instance);
            yield return null;
        }
    }
}