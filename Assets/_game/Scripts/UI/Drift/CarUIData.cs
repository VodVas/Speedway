using ArcadeVP;
using System;
using TMPro;
using UnityEngine;

[Serializable]
public class CarUIData
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private ArcadeAiVehicleController _aiCar;

    private ArcadeVehicleController _playerCar;
    private Racer _cachedRacer;
    private bool _isPlayer;

    public TextMeshProUGUI ScoreText => _scoreText;
    public ArcadeAiVehicleController AiCar => _aiCar;

    public ArcadeVehicleController PlayerCar
    {
        set
        {
            _playerCar = value;
            _isPlayer = _playerCar != null;
            if (_isPlayer)
            {
                _cachedRacer = _playerCar.GetComponent<Racer>();
            }
        }
    }

    public void CacheAiRacer()
    {
        if (_aiCar != null) _cachedRacer = _aiCar.GetComponent<Racer>();
    }

    public void UpdateScoreDisplay()
    {
        if (_scoreText == null) return;

        float score = GetCurrentScore();
        _scoreText.text = $"{score:F0}";
    }

    public RacerInfo GetRacerInfo()
    {
        ValidateRacer();

        return new RacerInfo("Игрок", 0f)
        {
            Name = _cachedRacer?.Name ?? "Игрок",
            Score = GetCurrentScore()
        };
    }

    public void ValidateRacer()
    {
        if (_cachedRacer != null) return;

        if (_isPlayer && _playerCar != null)
        {
            _cachedRacer = _playerCar.GetComponent<Racer>();
            Debug.Log($"Validated player racer: {(_cachedRacer != null ? _cachedRacer.Name : "null")}");
        }
        else if (!_isPlayer && _aiCar != null)
        {
            _cachedRacer = _aiCar.GetComponent<Racer>();
            Debug.Log($"Validated AI racer: {(_cachedRacer != null ? _cachedRacer.Name : "null")}");
        }
    }

    private float GetCurrentScore()
    {
        if (_isPlayer && _playerCar != null)
            return _playerCar.PlayerDriftScore;

        if (!_isPlayer && _aiCar != null)
            return _aiCar.EnemyDriftScore;

        return 0f;
    }
}