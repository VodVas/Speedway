using UnityEngine;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine.Experimental.GlobalIllumination;
using System.Threading;
using Unity.VisualScripting;
using static System.Net.WebRequestMethods;
using YG.Utils.LB;

public class MineSpawner : Spawner<Detonator>
{
    private Vector3 _cachedPosition;

    protected override Type GetObjectTypeToSpawn() => typeof(Detonator);

    protected override Vector3 GetSpawnPosition() => _cachedPosition;

    public void StartSpawn(Vector3 spawnPosition)
    {
        _cachedPosition = spawnPosition;

        SpawnObject();
    }

    protected override Detonator CreateObject()
    {
        var mine = base.CreateObject();

        return mine;
    }
}

public class UniTest : MonoBehaviour
{
    [SerializeField] private Transform _pointLight;
    [SerializeField] private int _delay = 500;

    private CancellationTokenSource _cts;

    private async void OnEnable()
    {
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
        await IntervalBlink(_cts.Token);
    }

    private void OnDisable()
    {
        _cts?.Cancel();
        _cts.Dispose();
        _cts = null;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts.Dispose();
    }

    async UniTask IntervalBlink(CancellationToken ct)
    {
        while (ct.IsCancellationRequested == false)
        {
            if (_pointLight == null)
            {
                Debug.Log("_pointLight not assign");
                enabled = false;
                return;
            }

            await UniTask.Delay(_delay, cancellationToken: ct);
            _pointLight.gameObject.SetActive(true);

            await UniTask.Delay(_delay, cancellationToken: ct);
            _pointLight.gameObject.SetActive(false);
        }
    }
}