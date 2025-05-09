using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class TemporaryMessageDisplay : MonoBehaviour
{
    [SerializeField] private float _displayTime = 1f;

    private CancellationTokenSource _cts;

    private void OnDestroy() => _cts?.Cancel();

    public void Show()
    {
        gameObject.SetActive(true);

        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        HideAfterDelay(_cts.Token).Forget();
    }

    private async UniTaskVoid HideAfterDelay(CancellationToken token)
    {
        await UniTask.Delay((int)(_displayTime * 1000), cancellationToken: token);

        if (!token.IsCancellationRequested)
        {
            gameObject.SetActive(false);
        }
    }
}