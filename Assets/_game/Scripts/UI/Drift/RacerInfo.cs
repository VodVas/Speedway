using UnityEngine;

public class RacerInfo
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float Score { get; private set; }
    [field: SerializeField] public bool IsPlayer { get; private set; }


    public RacerInfo(string name, float score, bool isPlayer)
    {
        Name = name;
        Score = score;
        IsPlayer = isPlayer;
    }
}