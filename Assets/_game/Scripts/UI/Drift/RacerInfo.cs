using UnityEngine;

public class RacerInfo
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public float Score { get; set; }

    public RacerInfo(string name, float score)
    {
        Name = name;
        Score = score;
    }
}