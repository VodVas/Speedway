using UnityEngine;

public class CarUpgrade : MonoBehaviour
{
    [SerializeField] private int _upgradeId = 0;
    [SerializeField] private string _upgradeName = "Engine";
    [SerializeField] private int _price = 100;
    [SerializeField] private GameObject _upgradeRoot = null;

    public int UpgradeId => _upgradeId;
    public string UpgradeName => _upgradeName;
    public int Price => _price;

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