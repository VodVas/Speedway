using UnityEngine;

[RequireComponent(typeof(CrashPartsCreator))]
[RequireComponent(typeof(Health))]
public abstract class Vehicle : MonoBehaviour
{
    private CrashPartsCreator _crashPartsCreator;
    private Health _health;
    private Vector3 _deathPosition;

    private void Awake()
    {
        _crashPartsCreator = GetComponent<CrashPartsCreator>();
        _health = GetComponent<Health>();
    }

    public void SetPosition()
    {
        _deathPosition = transform.position;
    }

    public void Respawn()
    {
        if (_health != null)
        {
            _health.ResetValue();
        }

        transform.position = _deathPosition;
    }

    public void SpawnParts()
    {
        _crashPartsCreator.StartSpawn(transform.position, GetType());
    }
}