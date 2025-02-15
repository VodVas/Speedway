using UnityEngine;

public class RaceProgressInitializer
{
    private const string WarningNoPlayerCar = "[RaceFlowInitializer] Нет активной машины игрока (Racer)!";

    private readonly Racer[] _racers;
    private readonly int _playerId;
    private readonly RaceCarSelector _raceCarManager;

    public RaceProgressInitializer(Racer[] racers, int playerId, RaceCarSelector raceCarManager)
    {
        if (racers == null || racers.Length == 0)
        {
            Debug.LogWarning("[RaceFlowInitializer] Массив гонщиков пуст или null.");
        }

        if (playerId < 0)
        {
            Debug.LogError("[RaceFlowInitializer] Некорректный playerId < 0, отключаем скрипт.");
        }

        if (raceCarManager == null)
        {
            Debug.LogError("[RaceFlowInitializer] RaceCarManager не назначен, отключаем скрипт.");
        }

        _racers = racers;
        _playerId = playerId;
        _raceCarManager = raceCarManager;
    }

    public void InsertPlayerCarIntoRacers()
    {
        Racer racer = _raceCarManager.GetPlayerRacer();

        if (racer == null)
        {
            Debug.LogWarning(WarningNoPlayerCar);
            return;
        }

        racer.RacerId = _playerId;

        if (_racers == null || _racers.Length == 0)
        {
            Racer[] newArr = new Racer[1];
            newArr[0] = racer;
            SetArrayToOriginal(newArr);
            return;
        }

        for (int i = 0; i < _racers.Length; i++)
        {
            if (_racers[i] == null)
            {
                _racers[i] = racer;

                return;
            }

            if (_racers[i].RacerId == _playerId)
            {
                _racers[i] = racer;

                return;
            }
        }

        ExpandArray(racer);
    }

    public void InitializeRacersPositions()
    {
        if (_racers == null)
        {
            return;
        }

        int count = _racers.Length;

        for (int i = 0; i < count; i++)
        {
            Racer racer = _racers[i];

            if (racer == null)
            {
                continue;
            }

            int startPosition = count - i;
            racer.SetPosition(startPosition);
            racer.UpdatePreviousPosition();
        }
    }

    public Racer FindPlayerRacer()
    {
        if (_racers == null)
        {
            return null;
        }

        for (int i = 0; i < _racers.Length; i++)
        {
            Racer racer = _racers[i];
            if (racer == null)
            {
                continue;
            }
            if (racer.RacerId == _playerId)
            {
                return racer;
            }
        }
        return null;
    }

    private void SetArrayToOriginal(Racer[] newArray)
    {
        for (int i = 0; i < newArray.Length; i++)
        {
            _racers[i] = newArray[i];
        }
    }

    private void ExpandArray(Racer racer)
    {
        if (_racers == null)
        {
            return;
        }

        int oldLength = _racers.Length;
        Racer[] newArray = new Racer[oldLength + 1];

        for (int i = 0; i < oldLength; i++)
        {
            newArray[i] = _racers[i];
        }

        newArray[oldLength] = racer;

        for (int i = 0; i < newArray.Length; i++)
        {
            if (i < _racers.Length)
            {
                _racers[i] = newArray[i];
            }
            else
            {
                Debug.Log("[RaceFlowInitializer] Требуется расширение массива");
            }
        }
    }
}
