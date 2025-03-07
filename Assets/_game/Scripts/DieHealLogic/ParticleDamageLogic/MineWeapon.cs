using UnityEngine;

public abstract class MineWeapon : Weapon
{
    protected abstract bool IsMineReadyToSpawn();

    [SerializeField] private MineSpawner _mineSpawner;

    protected override void Awake()
    {
        if (_mineSpawner == null)
        {
            _mineSpawner = GetComponent<MineSpawner>();
        }
    }

    protected override void Update()
    {
        if (IsMineReadyToSpawn())
        {
            StartSpawn();
        }
    }

    protected void StartSpawn()
    {
        if (_mineSpawner != null)
        {
            _mineSpawner.StartSpawn(transform.position);
        }
    }
}