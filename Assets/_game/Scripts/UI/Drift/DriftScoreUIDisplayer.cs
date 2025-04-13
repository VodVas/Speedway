using ArcadeVP;
using System;
using TMPro;
using UnityEngine;

public class DriftScoreUIDisplayer : MonoBehaviour
{
    private const string ERROR_NO_DATA = "[DriftScoreUIDisplayer] Нет данных для UI (carUIDataArray). Отключаю компонент.";
    private const string ERROR_NULL_DATA = "[DriftScoreUIDisplayer] Один из CarUIData равен null. Отключаю компонент.";
    private const string ERROR_NO_SCORE_TEXT = "[DriftScoreUIDisplayer] Не назначен ScoreText в одном из CarUIData. Отключаю компонент.";
    private const string ERROR_NULL_PLAYER = "[DriftScoreUIDisplayer] Передана null-ссылка в SetPlayerCar!";
    private const string WARNING_NO_SLOT = "[DriftScoreUIDisplayer] Не найден слот для игрока (AiCar == null). Машина не была назначена.";

    [SerializeField] private CarUIData[] _carUIDataArray;

    private void Awake()
    {
        ValidateData();
        CacheInitialRacers();
    }

    private void Update()
    {
        if (!enabled) return;

        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            _carUIDataArray[i].UpdateScoreDisplay();
        }
    }

    private void ValidateData()
    {
        if (_carUIDataArray == null || _carUIDataArray.Length == 0)
        {
            Debug.LogError(ERROR_NO_DATA, this);
            enabled = false;
            return;
        }

        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            if (_carUIDataArray[i] == null)
            {
                Debug.LogError(ERROR_NULL_DATA, this);
                enabled = false;
                return;
            }

            if (_carUIDataArray[i].ScoreText == null)
            {
                Debug.LogError(ERROR_NO_SCORE_TEXT, this);
                enabled = false;
                return;
            }
        }
    }

    private void CacheInitialRacers()
    {
        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            _carUIDataArray[i].CacheAiRacer();
        }
    }

    public void SetPlayerCar(ArcadeVehicleController newPlayerCar)
    {
        if (newPlayerCar == null)
        {
            Debug.LogError(ERROR_NULL_PLAYER, this);
            enabled = false;
            return;
        }

        bool assigned = false;
        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            if (_carUIDataArray[i].AiCar == null)
            {
                _carUIDataArray[i].PlayerCar = newPlayerCar;
                assigned = true;
                Debug.Log($"Player car assigned to slot {i}");
                break;
            }
        }

        if (!assigned) Debug.LogWarning(WARNING_NO_SLOT, this);
    }

    public RacerInfo[] CollectAllRacerInfo()
    {
        if (_carUIDataArray == null) return Array.Empty<RacerInfo>();

        var infos = new RacerInfo[_carUIDataArray.Length];

        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            infos[i] = _carUIDataArray[i].GetRacerInfo();
            Debug.Log($"Collected racer info: Name={infos[i].Name}, Score={infos[i].Score}");
        }

        return infos;
    }

    public float[] CollectAllScores()
    {
        if (_carUIDataArray == null) return Array.Empty<float>();

        var scores = new float[_carUIDataArray.Length];
        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            scores[i] = _carUIDataArray[i].GetRacerInfo().Score;
        }
        return scores;
    }
}