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
    [SerializeField, Range(0, 3)] private int _bossStartTime = 2;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image[] _lights;

    [Inject] private GameObject _UICounter;
    [Inject] private AiStuckHelper _aiStuckHelper;
    private WaitForSeconds _wait1Sec;
    private WaitForSeconds _waitLightDuration;

    public event Action Started;
    public event Action BossStarted;

    public void Awake()
    {
        _wait1Sec = new WaitForSeconds(1f);
        _waitLightDuration = new WaitForSeconds(_lightDuration);

        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return _wait1Sec;

        for (int i = _countdownDuration; i > 0; i--)
        {
            _countText.text = $"{i}";
            SetLightsToggle(true);

            if (i == _bossStartTime)
            {
                BossStarted?.Invoke();
            }

            yield return _waitLightDuration;
            SetLightsToggle(false);
            yield return _wait1Sec;
        }

        FinalizeCountdown();
    }

    private void FinalizeCountdown()
    {
        _UICounter.SetActive(false);
        _aiStuckHelper.enabled = true;
        _countText.text = string.Empty;
        Started?.Invoke();
    }

    private void SetLightsToggle(bool enable)
    {
        foreach (var image in _lights)
            image.enabled = enable;
    }
}