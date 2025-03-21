using UnityEngine;
using TMPro;

public class RaceProgressFinisher
{
    private TextMeshProUGUI[] _resultTexts;
    private int[] _positionRewards;

    public RaceProgressFinisher(TextMeshProUGUI[] resultTexts, int[] positionRewards)
    {
        _resultTexts = resultTexts;
        _positionRewards = positionRewards;
    }

    public void PrintFinalResults(Racer[] racers, Racer finishingRacer)
    {
        if (racers == null || finishingRacer == null || _resultTexts == null)
        {
            Debug.LogWarning("[RaceProgressFinisher] Invalid results data!");
            return;
        }

        ClearResultsUI();
        UpdateResultsUI(racers);
    }

    private void ClearResultsUI()
    {
        for (int i = 0; i < _resultTexts.Length; i++)
        {
            if (_resultTexts[i] != null)
            {
                _resultTexts[i].text = "";
            }
        }
    }

    private void UpdateResultsUI(Racer[] racers)
    {
        int maxEntries = Mathf.Min(_resultTexts.Length, racers.Length);

        for (int i = 0; i < maxEntries; i++)
        {
            Racer racer = racers[i];
            TextMeshProUGUI textField = _resultTexts[i];

            if (racer != null && textField != null)
            {
                int reward = GetRewardForPosition(racer.Position);
                textField.text = $"{racer.Position}. {racer.Name} {reward}$";
            }
        }
    }

    public int GetRewardForPosition(int position)
    {
        if (position < 1 || position > _positionRewards.Length)
        {
            Debug.LogError($"Invalid position for a reward: {position}");
            return 0;
        }

        return _positionRewards[position - 1];
    }
}