using TMPro;
using UnityEngine;

public abstract class BaseCarModification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] protected SystemLocalization _localization;

    [field: SerializeField] public int ModificationId { get; private set; }
    [field: SerializeField] public int Price { get; private set; } = 100;

    public string ModificationName { get; private set; } = "DefaultMod";

    protected virtual void Awake()
    {
        InitializeLocalization();

        ModificationName = _name.text;

        if (ModificationId < 0)
        {
            Debug.LogError($"[{GetType().Name}] Invalid ID: {ModificationId}", this);
            enabled = false;
            return;
        }
    }

    private void InitializeLocalization()
    {
        if (_localization == null)
            _localization = GetComponentInParent<SystemLocalization>();
    }

    public abstract string GetEffectDescription();
}