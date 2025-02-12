using UnityEngine;

internal sealed class RaceProgressFinisher
{
    public void PrintFinalResults(Racer[] racers, Racer finishingRacer)
    {
        if (racers == null || finishingRacer == null)
        {
            Debug.LogWarning("[RaceFlowFinisher] �������� ������ ��� ������ �����������.");
            return;
        }

        Debug.Log("����� ���������! �������� ����������:");

        for (int i = 0; i < racers.Length; i++)
        {
            Racer racer = racers[i];

            if (racer == null)
            {
                continue;
            }

            Debug.Log($"�����: {i + 1} | RacerID: {racer.RacerId} | �����: {racer.LapsCompleted}");
        }

        Debug.Log(
            $"����� � ID {finishingRacer.RacerId} �������� ����� �� ����� {finishingRacer.Position}."
        );
    }
}
