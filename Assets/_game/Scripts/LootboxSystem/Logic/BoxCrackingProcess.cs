using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Отвечает только за визуальную часть открытия коробки
public class BoxCrackingProcess : MonoBehaviour
{
    [SerializeField] private Button[] _boxButtons;
    [SerializeField] private BoxOpener[] _boxOpeners;
    [SerializeField] private ObjectOnceShaker[] _boxShakers;

    private int _buttonsCount;
    private int _selectedButtonIndex = -1;

    // Событие завершения анимации открытия коробки
    public event Action<int> BoxOpeningAnimationComplete;

    private void Awake()
    {
        _buttonsCount = _boxButtons?.Length ?? 0;
        ValidateComponents();
        SubscribeToButtons();
    }

    private void ValidateComponents()
    {
        if (_boxButtons == null || _boxButtons.Length == 0 ||
            _boxOpeners == null || _boxOpeners.Length != _buttonsCount ||
            _boxShakers == null || _boxShakers.Length != _buttonsCount)
        {
            Debug.LogError("[BoxCrackingProcess] Box components mismatch!");
            enabled = false;
        }
    }

    private void SubscribeToButtons()
    {
        for (int i = 0; i < _buttonsCount; i++)
        {
            int index = i;
            _boxButtons[i].onClick.AddListener(() => OnBoxSelected(index));
        }
    }

    private void OnBoxSelected(int buttonIndex)
    {
        if (_selectedButtonIndex != -1 || !enabled ||
            buttonIndex < 0 || buttonIndex >= _buttonsCount)
            return;

        _selectedButtonIndex = buttonIndex;
        DisableAllButtons();

        if (buttonIndex < _boxShakers.Length)
        {
            _boxShakers[buttonIndex].Shake();
        }

        StartCoroutine(OpenLidsAfterShake(buttonIndex));
    }

    private IEnumerator OpenLidsAfterShake(int buttonIndex)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < _boxOpeners.Length; i++)
        {
            _boxOpeners[i].OpenBox();
        }

        yield return new WaitForSeconds(1f);

        // Уведомляем о завершении анимации
        BoxOpeningAnimationComplete?.Invoke(_selectedButtonIndex);
    }

    public void DisableAllButtons()
    {
        for (int i = 0; i < _buttonsCount; i++)
        {
            _boxButtons[i].interactable = false;
        }
    }

    public void EnableAllButtons()
    {
        _selectedButtonIndex = -1;
        for (int i = 0; i < _buttonsCount; i++)
        {
            _boxButtons[i].interactable = true;
        }
    }

    public void ResetState()
    {
        _selectedButtonIndex = -1;
        EnableAllButtons();
    }

    private void OnDisable()
    {
        UnsubscribeFromButtons();
    }

    private void UnsubscribeFromButtons()
    {
        for (int i = 0; i < _buttonsCount; i++)
        {
            if (_boxButtons[i] != null)
            {
                _boxButtons[i].onClick.RemoveAllListeners();
            }
        }
    }
}
