using System;
using TMPro;
using UnityEngine;
using Zenject;

internal sealed class RaceProgressCheckpointLogic
{
    private const string IgnoreCheckpointLog = "Игнорируем пересечение {0}, ждали {1}";

    private readonly int _totalLaps;

    public RaceProgressCheckpointLogic(int totalLaps)
    {
        if (totalLaps < 1)
        {
            Debug.LogError("[RaceFlowCheckpointLogic] Некорректное число кругов.");
        }

        _totalLaps = totalLaps;
    }

    public void ProcessCheckpoint(int totalCheckpoints, Racer racer, int checkpointIndex, bool isPlayer, out bool lapCompleted)
    {
        lapCompleted = false;

        if (isPlayer)
        {
            int expectedCheckpointIndex = (racer.LastCheckpoint + 1) % totalCheckpoints;

            if (checkpointIndex != expectedCheckpointIndex)
            {
                Debug.Log(string.Format(IgnoreCheckpointLog, checkpointIndex, expectedCheckpointIndex));

                return;
            }
        }

        racer.UpdateLastCheckpoint(checkpointIndex);

        if (checkpointIndex == totalCheckpoints - 1)
        {
            racer.CompleteLap();
            lapCompleted = true;
        }
    }
}