using System;

internal sealed class RaceProgressPositionSorter
{
    public void SortRacers(ref Racer[] racers)
    {
        if (racers == null)
        {
            return;
        }

        Array.Sort(racers, (x, y) =>
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return 1;
            }
            if (y == null)
            {
                return -1;
            }

            int compByLaps = y.LapsCompleted.CompareTo(x.LapsCompleted);

            if (compByLaps != 0)
            {
                return compByLaps;
            }

            return y.LastCheckpoint.CompareTo(x.LastCheckpoint);
        });
    }
}
