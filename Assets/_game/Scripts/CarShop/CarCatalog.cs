using System.Collections.Generic;
using UnityEngine;

internal sealed class CarCatalog : MonoBehaviour
{
    [SerializeField] private List<CarData> _cars = null;

    private void Awake()
    {
        if (_cars == null)
        {
            Debug.LogError("CarCatalog: ������ ����� (cars) �� �����!", this);
            enabled = false;
            return;
        }

        if (_cars.Count == 0)
        {
            Debug.LogWarning("CarCatalog: ������ ����� ����!", this);
        }
    }

    public CarData GetCarData(int index)
    {
        if (index < 0 || index >= _cars.Count)
        {
            Debug.LogError($"CarCatalog: �������� ������ {index}", this);
            return null;
        }
        return _cars[index];
    }

    public void RemoveCarAtIndex(int index)
    {
        if (index < 0 || index >= _cars.Count)
        {
            Debug.LogError($"CarCatalog: ������ ������� ������ �� ������� {index}", this);
            enabled = false;
            return;
        }

        _cars.RemoveAt(index);
    }

    public int GetCount()
    {
        return _cars.Count;
    }
}