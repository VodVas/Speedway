using UnityEngine;
using System.Collections.Generic;
using System;

public class CarUpgrades : MonoBehaviour
{
    [SerializeField] private List<CarUpgrade> _upgrades;

    [field: SerializeField] public int CarId { get; private set; } = 0;

    public IReadOnlyList<CarUpgrade> Upgrades => _upgrades;

    private void Awake()
    {
        if (_upgrades == null)
        {
            Debug.LogError($"CarUpgrades: список _upgrades не назначен на {name}", this);
            enabled = false;
        }
    }

    public void InitializePurchasedUpgrades(Func<int, int, bool> hasCarUpgrade)
    {
        if (_upgrades == null || _upgrades.Count == 0) return;

        for (int i = 0; i < _upgrades.Count; i++)
        {
            CarUpgrade upgrade = _upgrades[i];
            bool purchased = hasCarUpgrade(CarId, upgrade.UpgradeId);

            upgrade.SetActive(purchased);
        }
    }
}