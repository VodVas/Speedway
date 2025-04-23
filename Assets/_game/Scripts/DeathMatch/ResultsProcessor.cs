using UnityEngine;

public sealed class ResultsProcessor : MonoBehaviour
{
    [SerializeField] private RaceRewardHandler _rewardHandler;

    private int _playerRacerId = 6;

    public void ProcessResults(RacerState[] racerStates)
    {
        System.Array.Sort(racerStates, (a, b) =>
        {
            int killComparison = b.Kills.CompareTo(a.Kills);
            return killComparison != 0 ? killComparison : b.RacerId.CompareTo(a.RacerId);
        });

        Racer[] racers = new Racer[racerStates.Length];
        Racer player = null;

        for (int i = 0; i < racerStates.Length; i++)
        {
            racers[i] = racerStates[i].Racer;
            racers[i].SetPosition(i + 1);

            if (racerStates[i].RacerId == _playerRacerId)
                player = racers[i];
        }

        if (player != null)
        {
            Racer[] finalResults = new Racer[6];
            int validCount = Mathf.Min(racers.Length, finalResults.Length);

            for (int i = 0; i < validCount; i++)
                finalResults[i] = racers[i];

            _rewardHandler.HandleRaceFinish(finalResults, player);
        }
        else
        {
            Debug.LogError("[ResultsProcessor] Player racer not found in results!");
        }
    }
}