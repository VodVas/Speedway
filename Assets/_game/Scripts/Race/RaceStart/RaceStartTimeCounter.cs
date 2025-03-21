using Reflex.Attributes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class RaceStartTimeCounter : MonoBehaviour
{
    [SerializeField] private int _countdownDuration = 3;
    [SerializeField] private float _lightDuration = 0.5f;
    [SerializeField, Range(0, 3)] private int _bossStartTime = 2;

    [Header("Mobile References")]
    [SerializeField] private TextMeshProUGUI _mobileCountText;
    [SerializeField] private Image[] _mobileLights;

    [Header("Desktop References")]
    [SerializeField] private TextMeshProUGUI _desktopCountText;
    [SerializeField] private Image[] _desktopLights;

    [Inject] private GameObject _mobileUICounter;
    [Inject] private GameObject _desktopUICounter;
    [Inject] private AiStuckHelper _aiStuckHelper;

    private TextMeshProUGUI _currentCountText;
    private Image[] _currentLights;
    private GameObject _currentUICounter;
    private WaitForSeconds _wait1Sec;
    private WaitForSeconds _waitLightDuration;

    public event Action Started;
    public event Action BossStarted;

    private void Awake()
    {
        InitializePlatformSpecificComponents();
        InitializeWaitObjects();
        StartCoroutine(Countdown());
    }

    private void InitializePlatformSpecificComponents()
    {
        bool isMobile = YandexGame.EnvironmentData.isMobile;

        _currentCountText = isMobile ? _mobileCountText : _desktopCountText;
        _currentLights = isMobile ? _mobileLights : _desktopLights;
        _currentUICounter = isMobile ? _mobileUICounter : _desktopUICounter;

        _mobileUICounter.SetActive(isMobile);
        _desktopUICounter.SetActive(!isMobile);
    }

    private void InitializeWaitObjects()
    {
        _wait1Sec = new WaitForSeconds(1f);
        _waitLightDuration = new WaitForSeconds(_lightDuration);
    }

    private IEnumerator Countdown()
    {
        yield return _wait1Sec;

        for (int i = _countdownDuration; i > 0; i--)
        {
            UpdateCounterUI(i);
            HandleBossStart(i);

            yield return _waitLightDuration;
            ToggleLights(false);
            yield return _wait1Sec;
        }

        FinalizeCountdown();
    }

    private void UpdateCounterUI(int count)
    {
        _currentCountText.text = count.ToString();
        ToggleLights(true);
    }

    private void HandleBossStart(int currentCount)
    {
        if (currentCount == _bossStartTime)
        {
            BossStarted?.Invoke();
        }
    }

    private void FinalizeCountdown()
    {
        _currentUICounter.SetActive(false);
        _currentCountText.text = string.Empty;
        _aiStuckHelper.enabled = true;
        Started?.Invoke();
    }

    private void ToggleLights(bool enable)
    {
        foreach (var image in _currentLights)
        {
            image.enabled = enable;
        }
    }
}





//public class RaceStartTimeCounter : MonoBehaviour
//{
//    [SerializeField] private int _countdownDuration = 3;
//    [SerializeField] private float _lightDuration = 0.5f;
//    [SerializeField, Range(0, 3)] private int _bossStartTime = 2;
//    [SerializeField] private TextMeshProUGUI _mobileCountText;
//    [SerializeField] private TextMeshProUGUI _desktopCountText;
//    [SerializeField] private Image[] _mobileLights;
//    [SerializeField] private Image[] _desktopLights;

//    [Inject] private GameObject _mobileUICounter;
//    [Inject] private GameObject _desktopUICounter;
//    [Inject] private AiStuckHelper _aiStuckHelper;
//    private WaitForSeconds _wait1Sec;
//    private WaitForSeconds _waitLightDuration;

//    public event Action Started;
//    public event Action BossStarted;

//    public void Awake()
//    {
//        _wait1Sec = new WaitForSeconds(1f);
//        _waitLightDuration = new WaitForSeconds(_lightDuration);

//        StartCoroutine(Countdown());
//    }

//    private IEnumerator Countdown()
//    {
//        yield return _wait1Sec;

//        for (int i = _countdownDuration; i > 0; i--)
//        {
//            _mobileCountText.text = $"{i}";
//            SetLightsToggle(true);

//            if (i == _bossStartTime)
//            {
//                BossStarted?.Invoke();
//            }

//            yield return _waitLightDuration;
//            SetLightsToggle(false);
//            yield return _wait1Sec;
//        }

//        FinalizeCountdown();
//    }

//    private void FinalizeCountdown()
//    {
//        _mobileUICounter.SetActive(false);
//        _aiStuckHelper.enabled = true;
//        _mobileCountText.text = string.Empty;
//        Started?.Invoke();
//    }

//    private void SetLightsToggle(bool enable)
//    {
//        foreach (var image in _mobileLights)
//            image.enabled = enable;
//    }
//}