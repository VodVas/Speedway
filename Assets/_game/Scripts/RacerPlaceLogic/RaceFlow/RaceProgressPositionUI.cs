using TMPro;
using UnityEngine;

public class RaceProgressPositionUI
{
    private readonly TextMeshProUGUI _playerPositionText;

    public RaceProgressPositionUI(TextMeshProUGUI playerPositionText)
    {
        if (playerPositionText == null)
        {
            Debug.LogError("[RaceFlowUserInterface] Поле playerPositionText не назначено.");
            return;
        }

        _playerPositionText = playerPositionText;
    }

    public void UpdatePlayerUI(Racer playerRacer, int totalRacersNumber)
    {
        if (playerRacer == null || _playerPositionText == null)
        {
            return;
        }

        string positionText = string.Format("{0} / {1}", playerRacer.Position, totalRacersNumber);
        _playerPositionText.text = positionText;
    }
}