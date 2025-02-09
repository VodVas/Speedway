using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToSwitch = null;

    private int _currentIndex = 0;

    private void Awake()
    {
        if (_objectsToSwitch == null || _objectsToSwitch.Count == 0)
        {
            Debug.LogWarning("ObjectSwitcher: список объектов пуст или не назначен.", this);
            enabled = false;
            return;
        }

        Init();
    }

    internal void SwitchToNextObject()
    {
        if (_currentIndex >= 0 && _currentIndex < _objectsToSwitch.Count)
        {
            _objectsToSwitch[_currentIndex].SetActive(false);
        }

        _currentIndex = (_currentIndex + 1) % _objectsToSwitch.Count;

        _objectsToSwitch[_currentIndex].SetActive(true);
    }

    internal void SwitchToPreviousObject()
    {
        if (_currentIndex >= 0 && _currentIndex < _objectsToSwitch.Count)
        {
            _objectsToSwitch[_currentIndex].SetActive(false);
        }

        _currentIndex = (_currentIndex - 1 + _objectsToSwitch.Count) % _objectsToSwitch.Count;

        _objectsToSwitch[_currentIndex].SetActive(true);
    }

    internal void Init()
    {
        _currentIndex = 0;
        SwitchToNextObject();
    }

    internal int GetCurrentIndex()
    {
        return _currentIndex;
    }

    internal void RemoveCarAtIndex(int index)
    {
        if (index < 0 || index >= _objectsToSwitch.Count)
        {
            Debug.LogError($"ObjectSwitcher: некорректный индекс для удаления {index}", this);
            enabled = false;
            return;
        }

        _objectsToSwitch[index].SetActive(false);
        _objectsToSwitch.RemoveAt(index);

        if (index <= _currentIndex)
        {
            _currentIndex--;
        }

        if (_objectsToSwitch.Count == 0)
        {
            Debug.Log("ObjectSwitcher: все объекты удалены.");
            enabled = false;
            return;
        }

        if (_currentIndex < 0)
        {
            _currentIndex = 0;
        }
        if (_currentIndex >= _objectsToSwitch.Count)
        {
            _currentIndex = _objectsToSwitch.Count - 1;
        }

        _objectsToSwitch[_currentIndex].SetActive(true);
    }
}