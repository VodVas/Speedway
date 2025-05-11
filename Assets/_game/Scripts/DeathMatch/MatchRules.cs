using UnityEngine;

public class MatchRules : MonoBehaviour
{
    [SerializeField] private int _requiredKills = 10;

    private RacerDataContainer _racerData;
    private ResultsProcessor _resultsProcessor;

    public void Configure(RacerDataContainer data, ResultsProcessor processor)
    {
        _racerData = data;
        _resultsProcessor = processor;
        _racerData.OnKill += OnKillOccurred;
    }

    private void OnKillOccurred(int killerId) => CheckForMatchEnd(killerId);

    private void CheckForMatchEnd(int killerId)
    {
        var racers = _racerData.GetActiveRacers();
        for (int i = 0; i < racers.Length; i++)
        {
            if (racers[i].RacerId == killerId && racers[i].Kills >= _requiredKills)
            {
                _resultsProcessor.ProcessResults(racers);
                Time.timeScale = 0f;
                return;
            }
        }
    }

    private void OnDestroy()
    {
        if (_racerData != null)
            _racerData.OnKill -= OnKillOccurred;
    }
}