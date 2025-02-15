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

    [field: SerializeField] public int UpgradeId { get; private set; } = 0;
    [field: SerializeField] public string UpgradeName { get; private set; } = "Engine";
    [field: SerializeField] public int Price { get; private set; } = 100;
    [field: SerializeField] public CarUpgradeType UpgradeType { get; private set; } = CarUpgradeType.Weapon;
    [field: SerializeField] public float UpgradeValue { get; private set; } = 5f;
    [field: SerializeField] public string UpgradeDescription { get; private set; } = string.Empty;

    private void Awake()
    {
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
}