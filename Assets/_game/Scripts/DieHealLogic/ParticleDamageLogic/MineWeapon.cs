using UnityEngine;

public abstract class MineWeapon : Weapon
{

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

    protected abstract bool IsMineReadyToSpawn();

    protected void StartSpawn()
    {
        if (_mineSpawner != null)
        {
            _mineSpawner.StartSpawn(transform.position);
        }
    }
}