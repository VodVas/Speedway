using System.Collections;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private float _blinkDuration = 0.5f;
    [SerializeField] private float _blinkInterval = 1f;

    private WaitForSeconds _cachedDurationWait;
    private WaitForSeconds _cachedIntervalWait;
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
        _cachedDurationWait = new WaitForSeconds(_blinkDuration);
        _cachedIntervalWait = new WaitForSeconds(_blinkInterval);
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




//public class Blinker : MonoBehaviour
//{
//    [SerializeField] private GameObject _object;
//    [SerializeField] private float _blinkDuration = 0.5f;
//    [SerializeField] private float _blinkInterval = 1f;

//    private void OnEnable()
//    {
//        StartCoroutine(BlinkLight());
//    }

//    private IEnumerator BlinkLight()
//    {
//        var waitDuration = new WaitForSeconds(_blinkDuration);
//        var waitInterval = new WaitForSeconds(_blinkInterval);

//        while (true)
//        {
//            _object.SetActive(true);

//            yield return waitDuration;

//            _object.SetActive(false);

//            yield return waitInterval;
//        }
//    }
//}