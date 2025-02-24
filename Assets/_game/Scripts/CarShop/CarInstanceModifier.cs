using UnityEngine;
using System.Collections.Generic;

public class CarInstanceModifier : MonoBehaviour
{
    [field: SerializeField] public List<CarInstanceConfigurator> CarsPrefabs { get; private set; }

    private void Start()
    {
#if UNITY_EDITOR
        for (int i = 0; i < CarsPrefabs.Count; i++)
        {
            CarInstanceConfigurator setting = CarsPrefabs[i];
            setting.CacheGuid();
        }
#endif
    }

    public void OptimizeCar(GameObject carInstance)
    {
        if (carInstance == null) return;

        PrefabId prefabId = carInstance.GetComponent<PrefabId>();

        if (prefabId == null) return;

        string instanceGuid = prefabId.Guid;

        bool found = false;

        for (int i = 0; i < CarsPrefabs.Count; i++)
        {
            CarInstanceConfigurator setting = CarsPrefabs[i];

            if (setting.Guid == instanceGuid)
            {
                MakeRigidbodiesKinematic(carInstance);
                DisableWeaponsOnInstance(carInstance);

                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogWarning($"[CarShopOptimizer] Не найден Setting с GUID {instanceGuid}", carInstance);
        }
    }

    private void MakeRigidbodiesKinematic(GameObject instance)
    {
        Rigidbody[] allRBs = instance.GetComponentsInChildren<Rigidbody>(true);

        for (int i = 0; i < allRBs.Length; i++)
        {
            if (allRBs[i] != null)
            {
               allRBs[i].isKinematic = true;
            }
        }
    }

    private void DisableWeaponsOnInstance(GameObject instance)
    {
        Transform[] allChildren = instance.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < allChildren.Length; i++)
        {
            Transform child = allChildren[i];

            if (child != null)
            {
                IWeapon weaponInterface = child.GetComponent<IWeapon>();

                if (weaponInterface != null)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}