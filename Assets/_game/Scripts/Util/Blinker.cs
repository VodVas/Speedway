using System.Collections;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private float _blinkDuration = 0.5f;
    [SerializeField] private float _blinkInterval = 1f;

    private void OnEnable()
    {
        StartCoroutine(BlinkLight());
    }

    private IEnumerator BlinkLight()
    {
        var waitDuration = new WaitForSeconds(_blinkDuration);
        var waitInterval = new WaitForSeconds(_blinkInterval);

        while (true)
        {
            _object.SetActive(true);

            yield return waitDuration;

            _object.SetActive(false);

            yield return waitInterval;
        }
    }
}