using System.Collections;
using UnityEngine;

public class LampLightToggler : MonoBehaviour
{
    public enum BlinkMode { Single, Continuous }

    [SerializeField] private Transform[] _lampArray;
    [SerializeField] private float _blinkDuration = 0.5f;
    [SerializeField] private BlinkMode _mode = BlinkMode.Single;
    [SerializeField] private bool _useUnscaledTime = true;

    private float[] _timers;
    private int _currentIndex;
    private bool _isBlinking;

    private void OnEnable()
    {
        if (_mode == BlinkMode.Continuous)
        {
            StartBlinking();
        }
    }

    private void OnDisable()
    {
        _isBlinking = false;
        ResetAllLamps();
    }

    public void StartBlinking()
    {
        if (_isBlinking) return;

        _isBlinking = true;
        _currentIndex = 0;

        InitializeTimers();
        StartCoroutine(BlinkRoutine());
    }

    private void InitializeTimers()
    {
        _timers = new float[_lampArray.Length];
        for (int i = 0; i < _timers.Length; i++)
        {
            _timers[i] = _blinkDuration;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (_isBlinking)
        {
            ToggleLamp(_currentIndex, true);

            float timer = 0;
            while (timer < _blinkDuration)
            {
                if (_useUnscaledTime)
                {
                    timer += Time.unscaledDeltaTime;
                }
                else
                {
                    timer += Time.deltaTime;
                }
                yield return null;
            }

            ToggleLamp(_currentIndex, false);

            _currentIndex++;
            if (_currentIndex >= _lampArray.Length)
            {
                if (_mode == BlinkMode.Single)
                {
                    _isBlinking = false;
                    yield break;
                }
                _currentIndex = 0;
            }
        }
    }

    private void ToggleLamp(int index, bool state)
    {
        if (index < 0 || index >= _lampArray.Length) return;
        if (_lampArray[index] != null)
        {
            _lampArray[index].gameObject.SetActive(state);
        }
    }

    private void ResetAllLamps()
    {
        foreach (var lamp in _lampArray)
        {
            if (lamp != null)
            {
                lamp.gameObject.SetActive(false);
            }
        }
    }
}