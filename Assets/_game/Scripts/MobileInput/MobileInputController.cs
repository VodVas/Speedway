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






//public class MobileInputController : MonoBehaviour
//{
//    [SerializeField] private Button _leftButton;
//    [SerializeField] private Button _rightButton;
//    [SerializeField] private Button _gasButton;
//    [SerializeField] private Button _driftButton;
//    [SerializeField] private Button _breakButton;

//    public float Horizontal { get; private set; }
//    public float Vertical { get; private set; }
//    public bool IsDriftPressed { get; private set; }

//    private void Awake()
//    {
//        if (_leftButton != null)
//        {
//            EventTrigger leftTrigger = _leftButton.gameObject.AddComponent<EventTrigger>();
//            AddEvent(leftTrigger, EventTriggerType.PointerDown, (data) => { Horizontal = -1f; });
//            AddEvent(leftTrigger, EventTriggerType.PointerUp, (data) => { if (Horizontal < 0f) Horizontal = 0f; });
//        }

//        if (_rightButton != null)
//        {
//            EventTrigger rightTrigger = _rightButton.gameObject.AddComponent<EventTrigger>();
//            AddEvent(rightTrigger, EventTriggerType.PointerDown, (data) => { Horizontal = 1f; });
//            AddEvent(rightTrigger, EventTriggerType.PointerUp, (data) => { if (Horizontal > 0f) Horizontal = 0f; });
//        }

//        if (_gasButton != null)
//        {
//            EventTrigger gasTrigger = _gasButton.gameObject.AddComponent<EventTrigger>();
//            AddEvent(gasTrigger, EventTriggerType.PointerDown, (data) => { Vertical = 1f; });
//            AddEvent(gasTrigger, EventTriggerType.PointerUp, (data) => { if (Vertical > 0f) Vertical = 0f; });
//        }

//        if (_breakButton != null)
//        {
//            EventTrigger breakTrigger = _breakButton.gameObject.AddComponent<EventTrigger>();
//            AddEvent(breakTrigger, EventTriggerType.PointerDown, (data) => { Vertical = -1f; });
//            AddEvent(breakTrigger, EventTriggerType.PointerUp, (data) => { if (Vertical < 0f) Vertical = 0f; });
//        }

//        if (_driftButton != null)
//        {
//            EventTrigger driftTrigger = _driftButton.gameObject.AddComponent<EventTrigger>();
//            AddEvent(driftTrigger, EventTriggerType.PointerDown, (data) => { IsDriftPressed = true; });
//            AddEvent(driftTrigger, EventTriggerType.PointerUp, (data) => { IsDriftPressed = false; });
//        }
//    }

//    private void AddEvent(EventTrigger trigger, EventTriggerType eventType, Action<BaseEventData> action)
//    {
//        EventTrigger.Entry entry = new EventTrigger.Entry();
//        entry.eventID = eventType;
//        entry.callback.AddListener((data) => { action(data); });
//        trigger.triggers.Add(entry);
//    }
//}