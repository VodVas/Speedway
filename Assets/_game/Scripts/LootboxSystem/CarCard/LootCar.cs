using System;
using UnityEngine;

public class LootCar : MonoBehaviour, ITerminatable
{
    public event Action<ITerminatable> Terminated;

    public void TerminateLoot()
    {
        Terminated?.Invoke(this);
    }
}