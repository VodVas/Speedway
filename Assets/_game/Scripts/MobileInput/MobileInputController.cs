using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileInputController : MonoBehaviour
{
    [Header("Mobile Controls")]
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _gasButton;
    [SerializeField] private Button _driftButton;
    [SerializeField] private Button _breakButton;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool IsDriftPressed { get; private set; }

    private void Awake()
    {
        InitializeButton(_leftButton, OnLeftPressed, OnLeftReleased);
        InitializeButton(_rightButton, OnRightPressed, OnRightReleased);
        InitializeButton(_gasButton, OnGasPressed, OnGasReleased);
        InitializeButton(_breakButton, OnBreakPressed, OnBreakReleased);
        InitializeButton(_driftButton, OnDriftPressed, OnDriftReleased);
    }

    private void InitializeButton(Button button, Action<BaseEventData> pressAction, Action<BaseEventData> releaseAction)
    {
        if (button == null) return;

        var trigger = button.gameObject.AddComponent<EventTrigger>();
        AddEvent(trigger, EventTriggerType.PointerDown, pressAction);
        AddEvent(trigger, EventTriggerType.PointerUp, releaseAction);
    }

    private void AddEvent(EventTrigger trigger, EventTriggerType type, Action<BaseEventData> action)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(data => action((BaseEventData)data));
        trigger.triggers.Add(entry);
    }

    private void OnLeftPressed(BaseEventData data) => Horizontal = -1f;
    private void OnLeftReleased(BaseEventData data) { if (Horizontal < 0f) Horizontal = 0f; }
    private void OnRightPressed(BaseEventData data) => Horizontal = 1f;
    private void OnRightReleased(BaseEventData data) { if (Horizontal > 0f) Horizontal = 0f; }
    private void OnGasPressed(BaseEventData data) => Vertical = 1f;
    private void OnGasReleased(BaseEventData data) { if (Vertical > 0f) Vertical = 0f; }
    private void OnBreakPressed(BaseEventData data) => Vertical = -1f;
    private void OnBreakReleased(BaseEventData data) { if (Vertical < 0f) Vertical = 0f; }
    private void OnDriftPressed(BaseEventData data) => IsDriftPressed = true;
    private void OnDriftReleased(BaseEventData data) => IsDriftPressed = false;
}