using UnityEngine;
using TMPro;
using YG;

public class RaceRewardHandler : MonoBehaviour
{
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private int[] _positionRewards;

    [Header("Desktop settings")]
    [SerializeField] private GameObject _desktopRewardMenu;
    [SerializeField] private GameObject[] _desktopPlayerDisabledUIElements;
    [SerializeField] private TextMeshProUGUI[] _desktopResultTexts;

    [Space(1)]

    [Header("Mobile settings")]
    [SerializeField] private GameObject _mobileRewardMenu;
    [SerializeField] private GameObject[] _mobilePlayerDisabledUIElements;
    [SerializeField] private TextMeshProUGUI[] _mobileResultTexts;

    [Header("V8 Fury Settings")]
    [SerializeField] private int[] _furyRewards = new int[3] { 100, 60, 30 };

    private RaceProgressFinisher _finisher;
    private GameObject[] _currentPlayerUIElements;
    private GameObject _currentRewardCanvas;
    private TextMeshProUGUI[] _currentResultTexts;
    private bool _isRewardGiven;

    private void Awake()
    {
        if (!ValidateSerializedData())
        {
            enabled = false;
            return;
        }

        SetupPlatformSpecificUI();
        InitializeSystems();
    }

    public bool ValidateRewardsSize(int racersCount)
    {
        if (_positionRewards.Length != racersCount)
        {
            Debug.LogError($"RaceRewardHandler:  оличество наград ({_positionRewards.Length}) должно совпадать с количеством гонщиков ({racersCount})!", this);
            return false;
        }

        return true;
    }

    public void HandleRaceFinish(Racer[] racers, Racer finishingRacer)
    {
        DisablePlayerUI();
        DeactivatePauseMenu();
        ActivateRewardCanvas();
        ProcessRaceResults(racers, finishingRacer);
    }

    private void AddFuryReward(int position)
    {
        if (position < 1 || position > 3) return;

        int fury = _furyRewards[position - 1];
        YandexGame.savesData.AddRespect(fury);
    }

    private void SetupPlatformSpecificUI()
    {
        bool isMobile = YandexGame.EnvironmentData.isMobile;
        _currentPlayerUIElements = isMobile ? _mobilePlayerDisabledUIElements : _desktopPlayerDisabledUIElements;
        _currentRewardCanvas = isMobile ? _mobileRewardMenu : _desktopRewardMenu;
        _currentResultTexts = isMobile ? _mobileResultTexts : _desktopResultTexts;
    }

    private void InitializeSystems()
    {
        _finisher = new RaceProgressFinisher(_currentResultTexts, _positionRewards);
    }

    private void DisablePlayerUI()
    {
        for (int i = 0; i < _currentPlayerUIElements.Length; i++)
        {
            if (_currentPlayerUIElements[i] != null)
                _currentPlayerUIElements[i].SetActive(false);
        }
    }

    private void ActivateRewardCanvas()
    {
        if (_currentRewardCanvas != null)
            _currentRewardCanvas.SetActive(true);
    }

    private void DeactivatePauseMenu()
    {
        if (_pauseManager != null)
            _pauseManager.gameObject.SetActive(false);
    }

    private void ProcessRaceResults(Racer[] racers, Racer finishingRacer)
    {
        _finisher.PrintFinalResults(racers, finishingRacer);

        if (!_isRewardGiven && finishingRacer != null)
        {
            YandexGame.savesData.AddMoney(_finisher.GetRewardForPosition(finishingRacer.Position));
            AddFuryReward(finishingRacer.Position);

            _isRewardGiven = true;
        }
    }

    private bool ValidateSerializedData()
    {
        bool isValid = true;

        if (_positionRewards == null || _positionRewards.Length == 0)
        {
            Debug.LogError("RaceRewardHandler: Ќаграды за позиции не настроены!", this);
            isValid = false;
        }

        if (_desktopRewardMenu == null || _mobileRewardMenu == null)
        {
            Debug.LogError("RaceRewardHandler: Reward canvases не назначены!", this);
            isValid = false;
        }

        if (_desktopPlayerDisabledUIElements == null || _desktopPlayerDisabledUIElements.Length == 0 ||
            _mobilePlayerDisabledUIElements == null || _mobilePlayerDisabledUIElements.Length == 0)
        {
            Debug.LogError("RaceRewardHandler: Ёлементы UI игрока не назначены!", this);
            isValid = false;
        }

        if (_pauseManager == null)
        {
            Debug.LogError("RaceRewardHandler: Pause manager не назначен!", this);
            isValid = false;
        }

        return isValid;
    }
}