using System;

public interface ITerminatable
{
    public void Terminate();

    public event Action<ITerminatable> Terminated;
}