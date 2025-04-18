using UnityEngine;

public class ColorCarModification : BaseCarModification
{
    [SerializeField] private Material[] _defaultMaterials;
    [SerializeField] private Renderer _targetRenderer;

    private Material[] _runtimeMaterials;
    private int _runtimeCount;

    protected override void Awake()
    {
        base.Awake();

        if (_targetRenderer == null)
        {
            Debug.LogError($"[{GetType().Name}] Missing renderer!", this);
            enabled = false;
            return;
        }

        if (_defaultMaterials == null || _defaultMaterials.Length == 0)
        {
            Debug.LogError($"[{GetType().Name}] No materials!", this);
            enabled = false;
            return;
        }
    }

    public void RefreshMaterials(PaintIntegrationSystem paintSystem)
    {
        if (paintSystem == null)
        {
            Debug.LogError($"[{GetType().Name}] PaintIntegrationSystem is null!", this);
            enabled = false;
            return;
        }

        if (!paintSystem.IsInitialized)
        {
            paintSystem.RefreshMaterials();
        }

        Material[] buffer = new Material[128];
        int total = paintSystem.GetAvailableMaterials(this, ref buffer);

        if (_runtimeMaterials == null || _runtimeMaterials.Length != total)
        {
            _runtimeMaterials = new Material[total];
        }

        System.Array.Copy(buffer, _runtimeMaterials, total);

        _runtimeCount = total;
    }

    public Material GetMaterial(int index) =>
        (index >= 0 && index < _runtimeCount) ? _runtimeMaterials[index] : null;

    public int MaterialsCount => _runtimeCount;
    public Renderer TargetRenderer => _targetRenderer;
    public Material[] DefaultMaterials => _defaultMaterials;

    public override string GetEffectDescription() => "Custom color";
}