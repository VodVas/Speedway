using UnityEngine;
using System.Collections;

public class FinalScoreDisplayer : MonoBehaviour
{
    private const string ErrorTimeLimit = "[TimeLimitAnnouncer] Неверное время, должно быть больше 0!";
    private const string ErrorNoUIDisplayer = "[TimeLimitAnnouncer] Не назначен DriftScoreUIDisplayer!";
    private const string WarningNoScores = "[TimeLimitAnnouncer] Нет данных для вывода очков. Возможно, массив пуст.";

    [SerializeField] private float MatchDuration = 300f;

    private ObjectsDisabler _objectsDisabler = null;
    private DriftScoreUIDisplayer _driftScoreUIDisplayer = null;
    private bool _isAnnounced = false;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _driftScoreUIDisplayer = GetComponent<DriftScoreUIDisplayer>();
        _objectsDisabler = GetComponent<ObjectsDisabler>();
        _wait = new WaitForSeconds(MatchDuration);

        if (MatchDuration <= 0f)
        {
            Debug.LogError(ErrorTimeLimit, this);
            enabled = false;
            return;
        }

        if (_driftScoreUIDisplayer == null)
        {
            Debug.LogError(ErrorNoUIDisplayer, this);
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(StartCountdown());
    }

    private void OnDisable()
    {
        StopCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        yield return _wait;

        AnnounceFinalScores();
    }

    private void AnnounceFinalScores()
    {
        _objectsDisabler.Execute();

        if (_isAnnounced)
        {
            return;
        }

        float[] scores = _driftScoreUIDisplayer.CollectAllScores();

        if (scores == null || scores.Length == 0)
        {
            Debug.LogWarning(WarningNoScores, this);
            return;
        }

        Debug.Log("----- Итоговые баллы по дрифту -----");

        for (int i = 0; i < scores.Length; i++)
        {
            Debug.Log($"Car index {i} => Score = {scores[i]}");
        }

        _isAnnounced = true;
    }
}