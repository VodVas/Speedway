using ArcadeVP;
using System;
using TMPro;
using UnityEngine;

[Serializable]
public class CarUIData
{
    private const string UnknownRacerName = "Unknown Racer";
    private static readonly string ScoreFormat = "{0} - {1:F0}";

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private ArcadeAiVehicleController _aiCar;

    private ArcadeVehicleController _playerCar;
    private Racer _cachedRacer;
    private bool _isPlayer;
    private string _lastName = string.Empty;
    private float _lastScore = -1f;

    public TextMeshProUGUI ScoreText => _scoreText;
    public ArcadeAiVehicleController AiCar => _aiCar;

    public ArcadeVehicleController PlayerCar
    {
        get { return _playerCar; }
        set
        {
            _playerCar = value;
            _isPlayer = _playerCar != null;
            _cachedRacer = _isPlayer ? _playerCar.GetComponent<Racer>() : null;
            _lastName = string.Empty;
        }
    }

    public void CacheComponents()
    {
        if (_aiCar != null)
        {
            _cachedRacer = _aiCar.GetComponent<Racer>();
        }
    }

    public void UpdateScoreDisplay()
    {
        if (_scoreText == null) return;

        var currentName = GetRacerName();
        var currentScore = GetCurrentScore();

        if (NeedsUpdate(currentName, currentScore))
        {
            UpdateText(currentName, currentScore);
        }
    }

    public RacerInfo GetRacerInfo()
    {
        return new RacerInfo(
            GetRacerName(),
            GetCurrentScore(),
            _isPlayer
        );
    }

    private bool NeedsUpdate(string name, float score)
    {
        return !string.Equals(_lastName, name)
            || !Mathf.Approximately(_lastScore, score);
    }

    private void UpdateText(string name, float score)
    {
        _scoreText.SetText(string.Format(ScoreFormat, name, score));
        _lastName = name;
        _lastScore = score;
    }

    private string GetRacerName()
    {
        if (_cachedRacer == null)
        {
            _cachedRacer = _isPlayer
                ? _playerCar?.GetComponent<Racer>()
                : _aiCar?.GetComponent<Racer>();
        }

        return _cachedRacer?.Name ?? UnknownRacerName;
    }

    private float GetCurrentScore()
    {
        if (_isPlayer)
        {
            return _playerCar != null
                ? _playerCar.PlayerDriftScore
                : 0f;
        }

        return _aiCar != null
            ? _aiCar.EnemyDriftScore
            : 0f;
    }

    public void CacheAiRacer()
    {
        if (_aiCar != null)
        {
            _cachedRacer = _aiCar.GetComponent<Racer>();
        }
    }
}