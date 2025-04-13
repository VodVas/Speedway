using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class FinalScoreDisplayer : MonoBehaviour
{
    [Serializable]
    private class RacerDisplayData
    {
        [field: SerializeField] public TMP_Text TextField { get; private set; }
        [field: SerializeField] public string DefaultName { get; private set; }
    }

    [SerializeField] private float _matchDuration = 300f;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private RacerDisplayData[] _racerDisplays;
    [SerializeField] private Transform _resultMenu;

    private DriftScoreUIDisplayer _driftScoreUIDisplayer;
    private ObjectsDisabler _objectsDisabler;
    private float _remainingTime;
    private bool _timerActive;
    private const float UPDATE_INTERVAL = 1f;
    private WaitForSeconds _cachedWait;

    private void Awake()
    {
        _driftScoreUIDisplayer = GetComponent<DriftScoreUIDisplayer>();
        _objectsDisabler = GetComponent<ObjectsDisabler>();
        _cachedWait = new WaitForSeconds(UPDATE_INTERVAL);

        ValidateDependencies();
        InitializeTimer();
    }

    private void OnDisable()
    {
        if (_timerActive) StopCoroutine(TimerRoutine());
    }

    private void ValidateDependencies()
    {
        if (_matchDuration <= 0f) throw new System.ArgumentException("Match duration must be positive");
        if (_driftScoreUIDisplayer == null) throw new MissingComponentException("DriftScoreUIDisplayer required");
        if (_racerDisplays == null || _racerDisplays.Length == 0) throw new MissingReferenceException("Racer displays required");
    }

    private void InitializeTimer()
    {
        _remainingTime = _matchDuration;
        UpdateTimerDisplay();
        StartCoroutine(TimerRoutine());
    }

    private void DisplayResults(RacerInfo[] racerInfos)
    {
        Debug.Log($"DisplayResults called with {racerInfos.Length} entries");
        StartCoroutine(DisplayResultsCoroutine(racerInfos));
    }

    private IEnumerator DisplayResultsCoroutine(RacerInfo[] racerInfos)
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < _racerDisplays.Length; i++)
        {
            if (i >= racerInfos.Length) break;

            var display = _racerDisplays[i];
            var info = racerInfos[i];

            string displayName = (string.IsNullOrEmpty(info.Name))
                ? display.DefaultName
                : info.Name;

            display.TextField.text = $"{displayName}: {info.Score:F0}";
            Debug.Log($"Setting text for display {i}: {display.TextField.text}");

            display.TextField.ForceMeshUpdate();
        }
    }

    private IEnumerator TimerRoutine()
    {
        _timerActive = true;

        while (_remainingTime > 0)
        {
            yield return _cachedWait;
            _remainingTime -= UPDATE_INTERVAL;
            UpdateTimerDisplay();
        }

        _timerActive = false;
        HandleMatchEnd();
    }

    private void UpdateTimerDisplay()
    {
        _timerText.text = FormatTime(_remainingTime);
    }

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int sec = Mathf.FloorToInt(seconds % 60);
        return $"{minutes:00}:{sec:00}";
    }

    private void HandleMatchEnd()
    {
        _objectsDisabler.Execute();
        _resultMenu.gameObject.SetActive(true);

        Time.timeScale = 0f;
        var racerInfo = _driftScoreUIDisplayer.CollectAllRacerInfo();
        Debug.Log($"HandleMatchEnd called, collecting {racerInfo.Length} racers info");
        DisplayResults(racerInfo);
    }
}