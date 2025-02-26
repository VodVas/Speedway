using Reflex.Attributes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceStartTimeCounter : MonoBehaviour
{
    [SerializeField] private int _countdownDuration = 3;
    [SerializeField] private float _lightDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image[] _lights;

    [Inject] private GameObject _UICounter;
    [Inject] private AiStuckHelper _aiStuckHelper;

    public event Action Started;

    public void Awake()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f);

        for (int i = _countdownDuration; i > 0; i--)
        {
            _countText.text = $"{i}";
            SetLightsEnabled(true);
            yield return new WaitForSeconds(_lightDuration);
            SetLightsEnabled(false);
            yield return new WaitForSeconds(1f);
        }

        FinalizeCountdown();
    }

    private void FinalizeCountdown()
    {
        _UICounter.SetActive(false);
        _aiStuckHelper.enabled = true;
        _countText.text = string.Empty;
        Started?.Invoke();
        Debug.Log("Started?.Invoke();");
    }

    private void SetLightsEnabled(bool enabled)
    {
        foreach (var image in _lights)
            image.enabled = enabled;
    }
}