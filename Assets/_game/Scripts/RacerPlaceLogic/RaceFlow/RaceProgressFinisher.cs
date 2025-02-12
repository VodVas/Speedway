using UnityEngine;

internal sealed class RaceProgressFinisher
{
    public void PrintFinalResults(Racer[] racers, Racer finishingRacer)
    {
        if (racers == null || finishingRacer == null)
        {
            Debug.LogWarning("[RaceFlowFinisher] Неверные данные для вывода результатов.");
            return;
        }

        Debug.Log("Гонка завершена! Итоговые результаты:");

        for (int i = 0; i < racers.Length; i++)
        {
            Racer racer = racers[i];

            if (racer == null)
            {
                continue;
            }

            Debug.Log($"Место: {i + 1} | RacerID: {racer.RacerId} | Круги: {racer.LapsCompleted}");
        }

        Debug.Log(
            $"Игрок с ID {finishingRacer.RacerId} закончил гонку на месте {finishingRacer.Position}."
        );
    }
}
