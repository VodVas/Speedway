using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class DeathMatchKillManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreboardUI;
    [SerializeField] private DamageHandler[] _allDamageHandlers;

    private readonly Dictionary<int, int> _killsByRacerId = new Dictionary<int, int>();
    private readonly Dictionary<int, string> _racerNames = new Dictionary<int, string>();

    private void Awake()
    {
        for (int i = 0; i < _allDamageHandlers.Length; i++)
        {
            var handler = _allDamageHandlers[i];

            handler.Died += OnVehicleDied;

            var vehicle = handler.GetComponent<Vehicle>();

            if (vehicle != null)
            {
                var racer = vehicle.GetComponent<Racer>();

                if (racer != null)
                {
                    int rId = racer.RacerId;

                    if (!_racerNames.ContainsKey(rId))
                    {
                        _racerNames[rId] = string.IsNullOrEmpty(racer.Name) ? $"Racer {rId}" : racer.Name;
                    }
                    if (!_killsByRacerId.ContainsKey(rId))
                    {
                        _killsByRacerId[rId] = 0;
                    }
                }
            }
        }
    }

    private void OnVehicleDied(Vehicle victimVehicle, IWeapon killerWeapon)
    {
        if (killerWeapon == null)
        {
            UpdateScoreboard();

            return;
        }

        Vehicle killerVehicle = killerWeapon.OwnerVehicle;

        if (killerVehicle == null)
        {
            UpdateScoreboard();

            return;
        }

        Racer killerRacer = killerVehicle.GetComponent<Racer>();

        if (killerRacer != null)
        {
            int killerId = killerRacer.RacerId;

            if (!_killsByRacerId.ContainsKey(killerId))
            {
                _killsByRacerId[killerId] = 0;
            }

            _killsByRacerId[killerId]++;
        }

        UpdateScoreboard();
    }

    private void UpdateScoreboard()
    {
        if (_scoreboardUI == null)
            return;

        System.Text.StringBuilder sb = new System.Text.StringBuilder(200);

        foreach (var kvp in _killsByRacerId)
        {
            int rId = kvp.Key;
            int kills = kvp.Value;
            string racerName = _racerNames.ContainsKey(rId) ? _racerNames[rId] : $"Racer {rId}";

            sb.Append(racerName);
            sb.Append(" — ");
            sb.Append(kills);
            sb.AppendLine();
        }

        _scoreboardUI.text = sb.ToString();
    }
}