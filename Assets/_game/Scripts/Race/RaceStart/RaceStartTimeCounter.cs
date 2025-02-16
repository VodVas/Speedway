using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RaceStartTimeCounter : MonoBehaviour
{
    [SerializeField] private int _countdownDuration = 3;
    [SerializeField] private TextMeshProUGUI _countText;

    private float _second = 1f;
    private WaitForSeconds _wait;

    public event Action Started;

    private void Awake()
    {
        _wait = new WaitForSeconds(_second);
    }

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return _wait;

        for (int i = _countdownDuration; i > 0; i--)
        {
            _countText.text = $"{i}";

            yield return _wait;
        }

        _countText.text = "";

        Started?.Invoke();
    }
}