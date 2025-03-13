using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LastLapRacerEliminator : MonoBehaviour
{
    [Header("Ссылка на массив гонщиков (тот же, что в RaceProgressTracker)")]
    [SerializeField] private Racer[] _racers = null;

    [Header("Число кругов (должно совпадать с RaceProgressTracker)")]
    [SerializeField] private int _totalLaps = 3;

    [Header("Включить ли логику выбывания?")]
    [SerializeField] private bool _enableElimination = true;

    // Храним "предыдущий" LapsCompleted для каждого гонщика, чтобы отследить переход на новый круг:
    private Dictionary<Racer, int> _prevLapCount;
    // Храним время, когда гонщик "перешёл" на очередной круг:
    private Dictionary<Racer, float> _lapFinishTime;

    private void Awake()
    {
        if (!_enableElimination)
        {
            enabled = false;
            return;
        }

        if (_racers == null || _racers.Length == 0)
        {
            Debug.LogWarning("[EliminationHandler] Нет гонщиков для выбывания!");
            enabled = false;
            return;
        }

        if (_totalLaps < 1)
        {
            Debug.LogError("[EliminationHandler] Некорректное число кругов!");
            enabled = false;
            return;
        }

        // Инициализируем словари
        _prevLapCount = new Dictionary<Racer, int>(_racers.Length);
        _lapFinishTime = new Dictionary<Racer, float>(_racers.Length);

        // Заполним начальные значения
        for (int i = 0; i < _racers.Length; i++)
        {
            Racer r = _racers[i];
            if (r != null)
            {
                // Сразу возьмём текущее LapsCompleted
                _prevLapCount[r] = r.LapsCompleted;
                // Поставим время "минимальное" или 0
                _lapFinishTime[r] = 0f;
            }
        }
    }

    private void LateUpdate()
    {
        // 1) Обновим данные о том, кто пересёк круг
        for (int i = 0; i < _racers.Length; i++)
        {
            Racer r = _racers[i];
            if (r == null)
                continue;

            if (r.HasFinished)
                continue; // Уже выбыл или полностью закончил гонку

            int oldLap = _prevLapCount[r];
            int newLap = r.LapsCompleted;

            // Если гонщик "поднял" число кругов => значит пересёк финиш круга сейчас
            if (newLap > oldLap)
            {
                _prevLapCount[r] = newLap;
                _lapFinishTime[r] = Time.time;
            }
        }

        // 2) Проверим, не закончился ли очередной круг для всех ещё активных гонщиков
        //    По сути, если все живые гонщики имеют одинаковый LapsCompleted, 
        //    то круг завершён (последний пересёк линию), и мы можем "выбить" последнего.
        EliminateLastIfNeeded();
    }

    private void EliminateLastIfNeeded()
    {
        // Собираем не-финишировавших гонщиков
        // и узнаём их текущий LapsCompleted
        int referenceLap = -1;
        List<Racer> activeRacers = new List<Racer>();
        for (int i = 0; i < _racers.Length; i++)
        {
            Racer r = _racers[i];
            if (r == null)
                continue;

            if (r.HasFinished)
                continue;

            // Если гонщик уже прошёл все круги, он фактически финишировал
            if (r.LapsCompleted >= _totalLaps)
                continue;

            // Это актуальный гонщик, участвующий в текущем круге
            activeRacers.Add(r);

            // Возьмём lap как "базу" для сравнения
            if (referenceLap < 0)
            {
                referenceLap = r.LapsCompleted;
            }
        }

        // Если вообще нет активных гонщиков — ничего не делаем
        if (activeRacers.Count == 0)
            return;

        // Проверим, все ли они на одном и том же lap
        for (int i = 0; i < activeRacers.Count; i++)
        {
            Racer r = activeRacers[i];
            if (r.LapsCompleted != referenceLap)
            {
                // Как только находим, что кто-то ещё не добрался до referenceLap, 
                // значит ещё не все завершили этот круг → выбывание не делаем.
                return;
            }
        }

        // Если дошли сюда, значит все активные гонщики = referenceLap
        // Последний, кто финишировал этот lap, имеет max _lapFinishTime[r].
        float maxTime = float.MinValue;
        Racer lastRacer = null;

        for (int i = 0; i < activeRacers.Count; i++)
        {
            Racer r = activeRacers[i];
            float t = _lapFinishTime[r];
            if (t > maxTime)
            {
                maxTime = t;
                lastRacer = r;
            }
        }

        // Теперь "lastRacer" - гонщик, который пересёк линию последним
        if (lastRacer != null)
        {
            // Устанавливаем "финиш" и отключаем
            lastRacer.SetFinished(true);
            lastRacer.gameObject.SetActive(false);

            Debug.Log($"[EliminationHandler] {lastRacer.Name} был последним и выбыл из гонки.");
        }
    }
}