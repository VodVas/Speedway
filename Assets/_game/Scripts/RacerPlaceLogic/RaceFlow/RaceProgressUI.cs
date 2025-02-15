using TMPro;
using UnityEngine;

internal sealed class RaceProgressUI
{
    private readonly TextMeshProUGUI _playerPositionText;
    //private const string PositionFormat = "Ваша позиция: {0}";
    private const string PositionFormat = "{0} / 6";

    public RaceProgressUI(TextMeshProUGUI playerPositionText)
    {
        if (playerPositionText == null)
        {
            Debug.LogError("[RaceFlowUserInterface] Поле playerPositionText не назначено.");
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