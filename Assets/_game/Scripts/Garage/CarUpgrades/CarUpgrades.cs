using UnityEngine;
using System.Collections.Generic;

public class CarUpgrades : MonoBehaviour
{
    [SerializeField] private List<CarUpgrade> _upgrades;
    [SerializeField] private int _carId = 0;

    public IReadOnlyList<CarUpgrade> Upgrades => _upgrades;

    public int CarId => _carId;

    private void Awake()
    {
        if (_upgrades == null)
        {
            Debug.LogError($"CarUpgrades: список _upgrades не назначен на {name}", this);
            enabled = false;
        }
    }

    public void InitializePurchasedUpgrades(System.Func<int, int, bool> hasCarUpgrade)
    {
        if (_upgrades == null || _upgrades.Count == 0) return;

        for (int i = 0; i < _upgrades.Count; i++)
        {
            CarUpgrade upgrade = _upgrades[i];
            bool purchased = hasCarUpgrade(_carId, upgrade.UpgradeId);
            upgrade.SetActive(purchased);
        }
    }
}