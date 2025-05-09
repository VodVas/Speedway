using TMPro;
using UnityEngine;

public class CarUpgrade : MonoBehaviour
{
    public enum CarUpgradeType
    {
        Weapon,
        Speed,
        Acceleration,
        Turn,
        Health
    }

    [SerializeField] private GameObject _upgradeRoot = null;
    [SerializeField] private TextMeshProUGUI _upgradeName = null;
    [SerializeField] private TextMeshProUGUI _upgradeDescription = null;

    [field: SerializeField] public int UpgradeId { get; private set; } = 0;
    [field: SerializeField] public int Price { get; private set; } = 100;
    [field: SerializeField] public CarUpgradeType UpgradeType { get; private set; } = CarUpgradeType.Weapon;
    [field: SerializeField] public float UpgradeValue { get; private set; } = 5f;

    public string UpgradeName { get; private set; } = "Engine";
    public string UpgradeDescription { get; private set; } = string.Empty;

    private void Awake()
    {
        if (_upgradeName != null || _upgradeDescription != null)
        {
            UpgradeName = _upgradeName.text;
            UpgradeDescription = _upgradeDescription.text;
        }

        if (_upgradeRoot == null)
        {
            Debug.LogError($"CarUpgrade: upgradeRoot не назначен на {gameObject.name}", this);
        }
        else
        {
            _upgradeRoot.SetActive(false);
        }
    }

    public void SetActive(bool value)
    {
        if (_upgradeRoot != null)
            _upgradeRoot.SetActive(value);
    }

#if UNITY_EDITOR
    public void Editor_SetUpgradeName(TextMeshProUGUI tmp)
    {
        if (!Application.isPlaying && tmp != null)
        {
            _upgradeName = tmp;
            UpgradeName = tmp.text;
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }

    public void Editor_SetUpgradeDescription(TextMeshProUGUI tmp)
    {
        if (!Application.isPlaying && tmp != null)
        {
            _upgradeDescription = tmp;
            UpgradeDescription = tmp.text;
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}