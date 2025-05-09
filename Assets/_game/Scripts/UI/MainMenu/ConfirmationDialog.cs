using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ConfirmationDialog : MonoBehaviour
{
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    private Action _onConfirm;

    private void OnEnable()
    {
        _confirmButton.onClick.AddListener(Confirm);
        _cancelButton.onClick.AddListener(Cancel);
    }

    private void OnDisable()
    {
        _confirmButton.onClick.RemoveListener(Confirm);
        _cancelButton.onClick.RemoveListener(Cancel);
    }

    public void Show(Action confirmCallback)
    {
        _onConfirm = confirmCallback;
        gameObject.SetActive(true);
    }

    private void Confirm()
    {
        _onConfirm?.Invoke();
        DeactivateAsync().Forget();
    }

    private void Cancel()
    {
        DeactivateAsync().Forget();
    }

    private async UniTaskVoid DeactivateAsync()
    {
        await UniTask.Yield();
        gameObject.SetActive(false);
    }
}