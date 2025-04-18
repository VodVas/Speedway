using System.Collections;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private float _blinkDuration = 0.5f;
    [SerializeField] private float _blinkInterval = 1f;

    private WaitForSecondsRealtime _cachedDurationWait;
    private WaitForSecondsRealtime _cachedIntervalWait;
    private int _arrayLength;
    private bool _hasValidObjects;

    private void Awake()
    {
        ValidateObjects();
        CacheWaitObjects();
    }

    private void OnEnable()
    {
        StartCoroutine(BlinkingProcess());
    }

    private void OnValidate()
    {
        ValidateObjects();
        CacheWaitObjects();
    }

    private void ValidateObjects()
    {
        _hasValidObjects = _objects != null && _objects.Length > 0;
        if (!_hasValidObjects) return;

        _arrayLength = _objects.Length;
    }

    private void CacheWaitObjects()
    {
        _cachedDurationWait = new WaitForSecondsRealtime(_blinkDuration);
        _cachedIntervalWait = new WaitForSecondsRealtime(_blinkInterval);
    }

    private IEnumerator BlinkingProcess()
    {
        if (!_hasValidObjects) yield break;

        while (true)
        {
            for (int i = 0; i < _arrayLength; i++)
            {
                GameObject current = _objects[i];
                if (current != null)
                {
                    current.SetActive(true);
                    yield return _cachedDurationWait;
                    current.SetActive(false);
                }

                if (i < _arrayLength - 1)
                {
                    yield return _cachedIntervalWait;
                }
            }

            yield return _cachedIntervalWait;
        }
    }
}