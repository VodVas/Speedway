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
        get => _playerCar;
        set
        {
            _playerCar = value;
            _isPlayer = _playerCar != null;
            if (_isPlayer) CacheRacerComponent();
        }
    }

    public void CacheComponents()
    {
        if (_aiCar != null)
        {
            _cachedRacer = _aiCar.GetComponent<Racer>();
            Debug.Log($"Cached AI racer: {_cachedRacer?.Name ?? "null"}");
        }
    }

    public void UpdateScoreDisplay()
    {
        if (_scoreText == null) return;

        float score = GetCurrentScore();
        _scoreText.text = $"{score:F0}";
    }

    public RacerInfo GetRacerInfo()
    {
        bool isCurrentlyPlayer = _playerCar != null;

        return new RacerInfo(
            GetRacerName(),
            GetCurrentScore(),
            isCurrentlyPlayer
        );
    }

    private string GetRacerName()
    {
        if (_cachedRacer == null) CacheRacerComponent();
        return _cachedRacer?.Name ?? "Unknown Racer";
    }

    private float GetCurrentScore()
    {
        if (_isPlayer && _playerCar != null)
        {
            float score = _playerCar.PlayerDriftScore;
            Debug.Log($"GetCurrentScore дл€ игрока: {score} (контроллер: {_playerCar.name})");
            return score;
        }
        else if (!_isPlayer && _aiCar != null)
        {
            float score = _aiCar.EnemyDriftScore;
            //Debug.Log($"GetCurrentScore дл€ AI {_aiCar.name}: {score}");
            return score;
        }

        Debug.LogWarning($"GetCurrentScore: Ќе удалось получить счет ({(_isPlayer ? "player" : "AI")}, playerCar={_playerCar != null}, aiCar={_aiCar != null})");
        return 0f;
    }

    private void CacheRacerComponent()
    {
        if (!_isPlayer || _playerCar == null) return;

        _cachedRacer = _playerCar.GetComponent<Racer>();
        //Debug.Log($"Cached player racer: {_cachedRacer?.Name ?? "null"}");
    }

    public void CacheAiRacer()
    {
        if (_aiCar != null) _cachedRacer = _aiCar.GetComponent<Racer>();
    }

    public void ValidateRacer()
    {
        if (_cachedRacer != null) return;

        if (_isPlayer && _playerCar != null)
        {
            _cachedRacer = _playerCar.GetComponent<Racer>();
        }
        else if (!_isPlayer && _aiCar != null)
        {
            _cachedRacer = _aiCar.GetComponent<Racer>();
        }
        Debug.Log($"Validated {(_isPlayer ? "player" : "AI")} racer: {_cachedRacer?.Name ?? "null"}");
    }
}










//public class CarUIData
//{
//    [SerializeField] private TextMeshProUGUI _scoreText;
//    [SerializeField] private ArcadeAiVehicleController _aiCar;

//    private ArcadeVehicleController _playerCar;
//    private Racer _cachedRacer;
//    private bool _isPlayer;

//    public TextMeshProUGUI ScoreText => _scoreText;
//    public ArcadeAiVehicleController AiCar => _aiCar;

//    public ArcadeVehicleController PlayerCar
//    {
//        set
//        {
//            _playerCar = value;
//            _isPlayer = _playerCar != null;
//            if (_isPlayer)
//            {
//                _cachedRacer = _playerCar.GetComponent<Racer>();
//            }
//        }
//    }

//    public void CacheComponents()
//    {
//        if (_aiCar != null)
//        {
//            _cachedRacer = _aiCar.GetComponent<Racer>();
//            Debug.Log($"Cached AI racer: {_cachedRacer?.Name ?? "null"}");
//        }
//    }

//    public void CacheAiRacer()
//    {
//        if (_aiCar != null) _cachedRacer = _aiCar.GetComponent<Racer>();
//    }

//    public void UpdateScoreDisplay()
//    {
//        if (_scoreText == null) return;

//        float score = GetCurrentScore();
//        _scoreText.text = $"{score:F0}";
//    }

//    public RacerInfo GetRacerInfo()
//    {
//        return new RacerInfo(
//            GetRacerName(),
//            GetCurrentScore(),
//            _isPlayerCached
//        );
//    }

//    private string GetRacerName()
//    {
//        if (_cachedRacer == null) CacheRacerComponent();
//        return _cachedRacer?.Name ?? "Unknown Racer";
//    }

//    private float GetCurrentScore()
//    {
//        if (_isPlayerCached)
//            return _playerCar != null ? _playerCar.PlayerDriftScore : 0f;

//        return _aiCar != null ? _aiCar.EnemyDriftScore : 0f;
//    }

//    private void CacheRacerComponent()
//    {
//        if (_isPlayerCached && _playerCar != null)
//        {
//            _cachedRacer = _playerCar.GetComponent<Racer>();
//            Debug.Log($"Cached player racer: {_cachedRacer?.Name ?? "null"}");
//        }
//    }

//    public bool IsPlayerCar()
//    {
//        return PlayerCar != null;
//    }

//    public void ValidateRacer()
//    {
//        if (_cachedRacer != null) return;

//        if (_isPlayer && _playerCar != null)
//        {
//            _cachedRacer = _playerCar.GetComponent<Racer>();
//            Debug.Log($"Validated player racer: {(_cachedRacer != null ? _cachedRacer.Name : "null")}");
//        }
//        else if (!_isPlayer && _aiCar != null)
//        {
//            _cachedRacer = _aiCar.GetComponent<Racer>();
//            Debug.Log($"Validated AI racer: {(_cachedRacer != null ? _cachedRacer.Name : "null")}");
//        }
//    }

//    //private float GetCurrentScore()
//    //{
//    //    if (_isPlayer && _playerCar != null)
//    //        return _playerCar.PlayerDriftScore;

//    //    if (!_isPlayer && _aiCar != null)
//    //        return _aiCar.EnemyDriftScore;

//    //    return 0f;
//    //}
//}