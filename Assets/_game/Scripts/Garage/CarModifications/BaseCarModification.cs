using UnityEngine;

public abstract class BaseCarModification : MonoBehaviour
{
    [field: SerializeField] public int ModificationId { get; private set; }
    [field: SerializeField] public string ModificationName { get; private set; } = "DefaultMod";
    [field: SerializeField] public int Price { get; private set; } = 100;

    protected virtual void Awake()
    {
        if (ModificationId < 0)
        {
            Debug.LogError($"[{GetType().Name}] Invalid ID: {ModificationId}", this);
            enabled = false;
            return;
        }
    }

    public abstract string GetEffectDescription();
}