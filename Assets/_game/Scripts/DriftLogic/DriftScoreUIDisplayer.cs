using ArcadeVP;
using System;
using TMPro;
using UnityEngine;

public class DriftScoreUIDisplayer : MonoBehaviour
{
    [Serializable]
    private class CarUIData
    {
        [field: SerializeField] public TextMeshProUGUI ScoreText { get; private set; } = null;
        [field: SerializeField] public ArcadeVehicleController PlayerCar { get; private set; } = null;
        [field: SerializeField] public ArcadeAiVehicleController AiCar { get; private set; } = null;
    }

    private const string ScoreFormat = "{0}";
    private const float DefaultScore = 0f;

    [SerializeField] private CarUIData[] _carUIDataArray;

    private void Awake()
    {
        if (_carUIDataArray == null || _carUIDataArray.Length == 0)
        {
            Debug.LogError("DriftScoreUIDisplayer: Нет данных для UI автотранспорта. Отключаю компонент.");
            enabled = false;
            return;
        }

        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            if (_carUIDataArray[i] == null)
            {
                Debug.LogError("DriftScoreUIDisplayer: Один из элементов данных UI равен null. Отключаю компонент.");
                enabled = false;
                return;
            }
            if (_carUIDataArray[i].ScoreText == null)
            {
                Debug.LogWarning("DriftScoreUIDisplayer: ScoreText не назначен в одном из элементов данных UI. Отключаю компонент.");
                enabled = false;
                return;
            }
            if (_carUIDataArray[i].PlayerCar == null && _carUIDataArray[i].AiCar == null)
            {
                Debug.LogWarning("DriftScoreUIDisplayer: Ни один из автотранспортных контроллеров не назначен в элементе данных UI.");
                enabled = false;
                return;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            ProcessScoreUpdate(_carUIDataArray[i]);
        }
    }

    private void ProcessScoreUpdate(CarUIData carData)
    {
        float score = DefaultScore;

        if (carData.PlayerCar != null)
        {
            score = carData.PlayerCar.PlayerDriftScore;
        }
        else if (carData.AiCar != null)
        {
            score = carData.AiCar.EnemyDriftScore;
        }
        else
        {
            Debug.LogWarning("DriftScoreUIDisplayer: Не назначен ни один контроллер автотранспорта.");
            return;
        }

        carData.ScoreText.text = string.Format(ScoreFormat, score);
    }
}