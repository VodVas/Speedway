using System.Collections;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [SerializeField] private Vehicle[] _enemies;
    [SerializeField] private float _delayUntilRespawn = 3f;

    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delayUntilRespawn);

        if (_enemies.Length > 0)
        {
            for (int i = 0; i < _enemies.Length; i++)
            {
                //DamageHandler damageHandler = _enemies[i].GetComponent<DamageHandler>();

                if (_enemies[i].TryGetComponent(out DamageHandler damageHandler))
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
        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i] != null)
            {
                //DamageHandler damageHandler = _enemies[i].GetComponent<DamageHandler>();

                //if (damageHandler != null)
                //{
                //    damageHandler.Died -= OnEnemyDied;
                //}

                if (_enemies[i].TryGetComponent(out DamageHandler damageHandler))
                {
                    damageHandler.Died -= OnEnemyDied;
                }
            }
        }
    }

    private void OnEnemyDied(Vehicle enemy)
    {
        enemy.SetPosition();
        Debug.LogWarning("OnEnemyDied");
        if (enemy.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
        }

        enemy.gameObject.SetActive(false);

        StartCoroutine(DelayRespawn(enemy));
    }

    private IEnumerator DelayRespawn(Vehicle enemy)
    {
        enemy.SpawnParts();

        yield return _wait;

        enemy.gameObject.SetActive(true);

        enemy.Respawn();
    }
}