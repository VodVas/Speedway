using UnityEngine;
using System.Collections;
using TMPro;
using System;
using YG;

public class FinalScoreDisplayer : MonoBehaviour
{
    [Serializable]
    private class RacerDisplayData
    {
        [field: SerializeField] public TextMeshProUGUI TextField { get; private set; }
        [field: SerializeField] public TextMeshProUGUI RankField { get; private set; }
        [field: SerializeField] public string DefaultName { get; private set; }
    }

    [Header("Respect Settings")]
    [SerializeField] private int[] _respectRewards = new int[3] { 50, 40, 30 };
    [SerializeField] private int[] _positionPenalties = new int[2] { 10, 20 };
    [SerializeField] private float _matchDuration = 300f;
    [SerializeField] private TextMeshProUGUI _timerText;
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

        if (_respectRewards.Length != 3)
            throw new ArgumentException("Должно быть 3 значения наград");

        if (_positionPenalties.Length != 2)
            throw new ArgumentException("Должно быть 2 значения штрафов");

        ValidateDependencies();
        InitializeTimer();
    }

    private int GetRespectChange(int position)
    {
        if (position <= 3) return _respectRewards[position - 1];
        if (position <= 5) return -_positionPenalties[position - 4];
        return 0;
    }

    private void OnDisable()
    {
        if (_timerActive) StopCoroutine(TimerRoutine());
    }

    private void ValidateDependencies()
    {
        if (_matchDuration <= 0f) throw new ArgumentException("Match duration must be positive");
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
        
        Array.Sort(racerInfos, (a, b) => b.Score.CompareTo(a.Score));

        for (int i = 0; i < _racerDisplays.Length; i++)
        {
            if (i >= racerInfos.Length) break;

            var display = _racerDisplays[i];
            RacerInfo info = racerInfos[i];
            int position = i + 1;

            string displayName = string.IsNullOrEmpty(info.Name) ? display.DefaultName : info.Name;
            display.TextField.text = $"{position} {displayName}: {info.Score:F0}";
            display.TextField.ForceMeshUpdate();

            if (info.IsPlayer && display.RankField != null)
            {
                int respect = GetRespectChange(position);

                if (respect != 0)
                {
                    display.RankField.gameObject.SetActive(true);
                    string sign = respect > 0 ? "+" : "-";
                    display.RankField.text = $"Ранг {sign}{Mathf.Abs(respect)}";

                    display.RankField.ForceMeshUpdate();
                }
                else
                {
                    display.RankField.gameObject.SetActive(false);
                }
            }
            else if (display.RankField != null)
            {
                display.RankField.gameObject.SetActive(false);
            }
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
        _driftScoreUIDisplayer.ValidatePlayerAssignment();

        var racerInfo = _driftScoreUIDisplayer.CollectAllRacerInfo();
        Time.timeScale = 0f;

        ApplyRespectChanges(racerInfo);
        DisplayResults(racerInfo);
    }

    private void ApplyRespectChanges(RacerInfo[] racers)
    {
        if (racers == null || racers.Length == 0) return;

        Array.Sort(racers, (a, b) => b.Score.CompareTo(a.Score));

        for (int i = 0; i < racers.Length; i++)
        {
            int position = i + 1;
            if (racers[i].IsPlayer)
            {
                ApplyRespectForPosition(position);
                break;
            }
        }
    }

    private void ApplyRespectForPosition(int position)
    {
        try
        {
            if (position >= 1 && position <= 3)
            {
                int rewardIndex = position - 1;
                if (rewardIndex < _respectRewards.Length)
                    YandexGame.savesData.AddRespect(_respectRewards[rewardIndex]);
            }
            else if (position >= 4 && position <= 6)
            {
                int penaltyIndex = position - 4;
                if (penaltyIndex < _positionPenalties.Length)
                    YandexGame.savesData.AddRespect(-_positionPenalties[penaltyIndex]);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error applying respect changes: {ex.Message}");
        }
    }
}