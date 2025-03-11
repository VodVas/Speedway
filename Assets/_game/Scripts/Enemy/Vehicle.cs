using UnityEngine;

[RequireComponent(typeof(CrashPartsCreator))]
[RequireComponent(typeof(Health))]
public abstract class Vehicle : MonoBehaviour
{
    private CrashPartsCreator _crashPartsCreator;
    private Health _health;
    private Vector3 _deathPosition;
    private bool _isRespawning;

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
        _health?.ResetValue();

        transform.position = _deathPosition;
    }

    public void SpawnParts()
    {
        _crashPartsCreator.StartSpawn(transform.position, GetType());
    }

    public bool TryStartRespawn()
    {
        if (!_isRespawning)
        {
            _isRespawning = true;

            return true;
        }

        return false;
    }

    public void FinishRespawn()
    {
         _isRespawning = false;
    }
}