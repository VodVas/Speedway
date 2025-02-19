using ArcadeVP;
using System;
using TMPro;
using UnityEngine;

public class DriftScoreUIDisplayer : MonoBehaviour
{
    [Serializable]
    private class CarUIData
    {
        [SerializeField] private TextMeshProUGUI _scoreText = null;
        [SerializeField] private ArcadeAiVehicleController _aiCar = null;

        private ArcadeVehicleController _playerCar = null;

        public TextMeshProUGUI ScoreText => _scoreText;
        public ArcadeAiVehicleController AiCar => _aiCar;

        public ArcadeVehicleController PlayerCar
        {
            get => _playerCar;
            set => _playerCar = value;
        }
    }

    private const string ScoreFormat = "{0}";
    private const float DefaultScore = 0f;
    private const string ErrorNoData = "[DriftScoreUIDisplayer] Нет данных для UI (carUIDataArray). Отключаю компонент.";
    private const string ErrorNullData = "[DriftScoreUIDisplayer] Один из CarUIData равен null. Отключаю компонент.";
    private const string ErrorNoScoreText = "[DriftScoreUIDisplayer] Не назначен ScoreText в одном из CarUIData. Отключаю компонент.";
    private const string ErrorNullPlayer = "[DriftScoreUIDisplayer] Передана null-ссылка в SetPlayerCar!";
    private const string WarningNoSlot = "[DriftScoreUIDisplayer] Не найден слот для игрока (AiCar == null). Машина не была назначена.";

    [SerializeField] private CarUIData[] _carUIDataArray = null;

    private void Awake()
    {
        if (_carUIDataArray == null || _carUIDataArray.Length == 0)
        {
            Debug.LogError(ErrorNoData, this);
            enabled = false;
            return;
        }

        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            CarUIData carData = _carUIDataArray[i];
            if (carData == null)
            {
                Debug.LogError(ErrorNullData, this);
                enabled = false;
                return;
            }
            if (carData.ScoreText == null)
            {
                Debug.LogError(ErrorNoScoreText, this);
                enabled = false;
                return;
            }
        }
    }

    private void Update()
    {
        if (!enabled)
        {
            return;
        }
        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            UpdateCarScore(_carUIDataArray[i]);
        }
    }

    public void SetPlayerCar(ArcadeVehicleController newPlayerCar)
    {
        if (newPlayerCar == null)
        {
            Debug.LogError(ErrorNullPlayer, this);
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
                break;
            }
        }

        if (!assigned)
        {
            Debug.LogWarning(WarningNoSlot, this);
        }
    }

    public float[] CollectAllScores()
    {
        if (_carUIDataArray == null || _carUIDataArray.Length == 0)
        {
            return null;
        }

        float[] scores = new float[_carUIDataArray.Length];

        for (int i = 0; i < _carUIDataArray.Length; i++)
        {
            CarUIData carData = _carUIDataArray[i];
            float score = DefaultScore;

            if (carData.PlayerCar != null)
            {
                score = carData.PlayerCar.PlayerDriftScore;
            }

            else if (carData.AiCar != null)
            {
                score = carData.AiCar.EnemyDriftScore;
            }

            scores[i] = score;
        }

        return scores;
    }

    private void UpdateCarScore(CarUIData carData)
    {
        if (carData == null || carData.ScoreText == null)
        {
            return;
        }

        float score = DefaultScore;

        if (carData.PlayerCar != null)
        {
            score = carData.PlayerCar.PlayerDriftScore;
        }
        else if (carData.AiCar != null)
        {
            score = carData.AiCar.EnemyDriftScore;
        }

        carData.ScoreText.text = string.Format(ScoreFormat, score);
    }
}