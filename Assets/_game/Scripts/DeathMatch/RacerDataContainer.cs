using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RacerDataContainer : IDisposable
{
    [SerializeField] private DamageHandler[] _damageHandlers;

    public event Action OnDataChanged;
    public event Action<int> OnKill;

    private RacerState[] _racerStates;
    private bool[] _isActive;
    private Dictionary<Vehicle, int> _vehicleToRacerId = new(6);
    private RacerState[] _cachedActiveRacers;

    public void Initialize()
    {
        int count = _damageHandlers.Length;
        _racerStates = new RacerState[count];
        _isActive = new bool[count];

        for (int i = 0; i < count; i++)
        {
            if (!_damageHandlers[i]) continue;

            var vehicle = _damageHandlers[i].GetComponent<Vehicle>();
            var racer = vehicle.GetComponent<Racer>();

            _racerStates[i] = new RacerState(
                racer.RacerId,
                racer,
                string.IsNullOrEmpty(racer.Name) ? $"Racer {racer.RacerId}" : racer.Name,
                vehicle
            );

            _vehicleToRacerId[vehicle] = racer.RacerId;
            _isActive[i] = _damageHandlers[i].isActiveAndEnabled;
            _damageHandlers[i].VehicleKilled += OnVehicleKilled;
        }

        UpdateActiveRacersCache();
        OnDataChanged?.Invoke();
    }

    public void RefreshActiveStates()
    {
        for (int i = 0; i < _damageHandlers.Length; i++)
        {
            _isActive[i] = _damageHandlers[i] != null && _damageHandlers[i].isActiveAndEnabled;
        }

        UpdateActiveRacersCache();
        OnDataChanged?.Invoke();
    }

    private void OnVehicleKilled(Vehicle victim, IWeapon killer)
    {
        if (killer?.OwnerVehicle == null) return;

        if (_vehicleToRacerId.TryGetValue(killer.OwnerVehicle, out int killerId))
        {
            for (int i = 0; i < _racerStates.Length; i++)
            {
                if (_racerStates[i].RacerId == killerId)
                {
                    _racerStates[i].IncrementKills();
                    UpdateActiveRacersCache();
                    OnDataChanged?.Invoke();
                    OnKill?.Invoke(killerId);
                    break;
                }
            }
        }
    }

    public RacerState[] GetActiveRacers() => _cachedActiveRacers;

    private void UpdateActiveRacersCache()
    {
        int count = 0;
        for (int i = 0; i < _isActive.Length; i++)
            if (_isActive[i]) count++;

        var result = new RacerState[count];
        int index = 0;
        for (int i = 0; i < _racerStates.Length; i++)
            if (_isActive[i]) result[index++] = _racerStates[i];

        _cachedActiveRacers = result;
    }

    public void Dispose()
    {
        for (int i = 0; i < _damageHandlers.Length; i++)
        {
            if (_damageHandlers[i])
                _damageHandlers[i].VehicleKilled -= OnVehicleKilled;
        }

        _vehicleToRacerId.Clear();
    }
}