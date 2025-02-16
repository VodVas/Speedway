using TMPro;
using UnityEngine;

internal sealed class RaceProgressPositionUI
{
    private const string PositionFormat = "{0} / 6";

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

    public void UpdatePlayerUI(Racer playerRacer)
    {
        if (playerRacer == null || _playerPositionText == null)
        {
            return;
        }

        string positionText = string.Format(PositionFormat, playerRacer.Position);
        _playerPositionText.text = positionText;
    }
}