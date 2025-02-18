using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RaceStartTimeCounter : MonoBehaviour
{
    [SerializeField] private int _countdownDuration = 3;
    [SerializeField] private float _lightDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image[] _lights;
    [SerializeField] private GameObject _UICounter;

    [Inject] private AiStuckHelper _aiStuckHelper;
    private float _second = 1f;
    private WaitForSeconds _wait;
    private WaitForSeconds _waitLightDuration;

    public event Action Started;

    private void Awake()
    {
        _wait = new WaitForSeconds(_second);
        _waitLightDuration = new WaitForSeconds(_lightDuration);
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

            SetLightsEnabled(true);

            yield return _waitLightDuration;

            SetLightsEnabled(false);

            yield return _wait;
        }

        _UICounter.SetActive(false);
        _aiStuckHelper.enabled = true;
        _countText.text = "";

        Started?.Invoke();
    }

    private void SetLightsEnabled(bool enabled)
    {
        foreach (var image in _lights)
        {
            image.enabled = enabled;
        }
    }
}