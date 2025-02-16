using TMPro;

public class RaceProgressUILaps
{
    private readonly TextMeshProUGUI _lapCounterText;

    public RaceProgressUILaps(TextMeshProUGUI lapCounterText)
    {
        _lapCounterText = lapCounterText;
    }

    public void UpdateLapCounter(int currentLap, int totalLaps)
    {
        if (currentLap == totalLaps + 1)
        {
            return;
        }

        _lapCounterText.text = $"{currentLap}/{totalLaps}";
    }
}