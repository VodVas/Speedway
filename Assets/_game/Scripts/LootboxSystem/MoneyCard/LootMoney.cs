using System;
using UnityEngine;

public class LootMoney : MonoBehaviour, ITerminatable
{
    public event Action<ITerminatable> Terminated;

    public void TerminateLoot()
    {
        Terminated?.Invoke(this);
    }
}