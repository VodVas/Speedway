public struct RacerState
{
    public readonly int RacerId;
    public readonly Racer Racer;
    public readonly string Name;
    public readonly Vehicle Vehicle;
    public int Kills { get; private set; }

    public RacerState(int id, Racer racer, string name, Vehicle vehicle)
    {
        RacerId = id;
        Racer = racer;
        Name = name;
        Vehicle = vehicle;
        Kills = 0;
    }

    public void IncrementKills() => Kills++;
}