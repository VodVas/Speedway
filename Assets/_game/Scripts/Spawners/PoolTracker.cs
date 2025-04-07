using UnityEngine;

public class PoolTracker : MonoBehaviour
{
    public int CardIndex { get; private set; }

    public void SetCardIndex(int index)
    {
        if (CardIndex != 0)
            throw new System.InvalidOperationException("Index already set");
        CardIndex = index;
    }
}