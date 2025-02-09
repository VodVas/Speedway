using System.Collections;
using UnityEngine;

public class DeadCarRespawner : MonoBehaviour
{
    [SerializeField] private Vehicle[] _vehicle;
    [SerializeField] private float _delayUntilRespawn = 3f;

    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delayUntilRespawn);

        if (_vehicle.Length > 0)
        {
            for (int i = 0; i < _vehicle.Length; i++)
            {
                if (_vehicle[i].TryGetComponent(out DamageHandler damageHandler))
                {
                    damageHandler.Died += OnEnemyDied;
                }
            }
        }
        else
        {
            Debug.LogWarning("List is empty!");
            enabled = false;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _vehicle.Length; i++)
        {
            if (_vehicle[i] != null)
            {
                if (_vehicle[i].TryGetComponent(out DamageHandler damageHandler))
                {
                    damageHandler.Died -= OnEnemyDied;
                }
            }
        }
    }

    private void OnEnemyDied(Vehicle vehicle)
    {
        vehicle.SetPosition();

        if (vehicle.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
        }

        vehicle.gameObject.SetActive(false);

        StartCoroutine(DelayRespawn(vehicle));
    }

    private IEnumerator DelayRespawn(Vehicle enemy)
    {
        enemy.SpawnParts();

        yield return _wait;

        enemy.gameObject.SetActive(true);

        enemy.Respawn();
    }
}