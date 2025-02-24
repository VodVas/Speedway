using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GarageCarInstancePool : MonoBehaviour
{
    [Header("Список всех доступных в игре префабов машин")]
    [SerializeField] private List<GameObject> _carPrefabs;

    private readonly List<GameObject> _spawnedCars = new List<GameObject>();

    [field: SerializeField] public Transform GarageSpawnPoint { get; private set; }

    public List<GameObject> SpawnedCars => _spawnedCars;

    public IEnumerator SpawnPurchasedCars()
    {
        _spawnedCars.Clear();

        for (int i = 0; i < _carPrefabs.Count; i++)
        {
            GameObject prefab = _carPrefabs[i];
            if (prefab == null)
            {
                Debug.LogWarning($"[GarageCarInstancePool] Префаб с индексом {i} не задан!");
                continue;
            }

            CarData data = prefab.GetComponent<CarData>();

            if (data == null)
            {
                Debug.LogWarning($"[GarageCarInstancePool] На префабе '{prefab.name}' отсутствует компонент CarData!");
                continue;
            }

            if (YandexGame.savesData.HasCar(data.Id))
            {
                GameObject instance = Instantiate(prefab, GarageSpawnPoint.position, GarageSpawnPoint.rotation);
                instance.SetActive(false);

                _spawnedCars.Add(instance);
            }

            yield return null;
        }
    }
}
