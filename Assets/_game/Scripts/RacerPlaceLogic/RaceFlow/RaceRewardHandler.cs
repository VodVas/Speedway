using UnityEngine;
using TMPro;
using YG;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RaceRewardHandler : MonoBehaviour
{
    private const string ERROR_REWARDS_SIZE = "RaceRewardHandler: Количество наград ({0}) должно совпадать с количеством гонщиков ({1})!";
    private const string ERROR_PENALTIES_LENGTH = "RaceRewardHandler: Должно быть 3 элемента штрафов!";
    private const string ERROR_REWARDS_EMPTY = "RaceRewardHandler: Награды не заданы!";
    private const string ERROR_CANVASES_NULL = "RaceRewardHandler: Reward canvases не назначены!";
    private const string ERROR_UI_ELEMENTS = "RaceRewardHandler: Элементы UI игрока не назначены!";
    private const string ERROR_PAUSE_MANAGER = "RaceRewardHandler: Pause manager не назначен!";
    private const string ERROR_RESPECT_REWARDS = "RaceRewardHandler: Должно быть 3 элемента наград!";
    private const string RESPECT_FORMAT = "Ранг {0}{1}";
    private const string POSITIVE_SIGN = "+";
    private const string NEGATIVE_SIGN = "-";
    private const int NORMAL_RACE_SIZE = 6;
    private const int BOSS_BATTLE_SIZE = 2;


    [Serializable]
    private class RacerDisplayData
    {
        [field: SerializeField] public TextMeshProUGUI ResultText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI RespectText { get; private set; }
    }

    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private int[] _positionRewards;
    [Space(1)]

    [Header("Boss settings")]
    [SerializeField] private bool _isBossBattle;
    [SerializeField] private int _bossWinRespect = 50;
    [SerializeField] private int _bossLoseRespect = -50;
    [SerializeField] private BossType _currentBoss = BossType.None;

    public enum BossType
    {
        None = -1,
        FirstBoss = 0,
        SecondBoss = 1,
        ThirdBoss = 2
    }

    [Header("Desktop settings")]
    [SerializeField] private GameObject _desktopRewardMenu;
    [SerializeField] private GameObject[] _desktopPlayerDisabledUIElements;
    [SerializeField] private RacerDisplayData[] _desktopRacerDisplays = new RacerDisplayData[6];

    [Space(1)]

    [Header("Mobile settings")]
    [SerializeField] private GameObject _mobileRewardMenu;
    [SerializeField] private GameObject[] _mobilePlayerDisabledUIElements;
    [SerializeField] private RacerDisplayData[] _mobileRacerDisplays = new RacerDisplayData[6];

    [Header("Respect Settings")]
    [SerializeField]
    [HideInInspector]
    private int[] _respectRewards = new int[3] { 50, 30, 10 };

    [Header("Penalty Settings")]
    [SerializeField]
    [HideInInspector]
    private int[] _positionPenalties = new int[3] { 10, 20, 30 };

    private RaceProgressFinisher _finisher;
    private GameObject[] _currentPlayerUIElements;
    private GameObject _currentRewardCanvas;
    private RacerDisplayData[] _currentRacerDisplays;
    private bool _isRewardGiven;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!_isBossBattle && _currentBoss != BossType.None)
        {
            _currentBoss = BossType.None;
        }
        else if (_isBossBattle && _currentBoss == BossType.None)
        {
            _currentBoss = BossType.FirstBoss;
        }

        SerializedObject serializedObject = new SerializedObject(this);

        SerializedProperty respectProperty = serializedObject.FindProperty("_respectRewards");
        SerializedProperty penaltyProperty = serializedObject.FindProperty("_positionPenalties");
        SerializedProperty rewardsProperty = serializedObject.FindProperty("_positionRewards");

        if (_isBossBattle)
        {
            HideInInspector hideAttribute = new HideInInspector();

            EditorUtility.SetDirty(this);

            if (_positionRewards != null && _positionRewards.Length != BOSS_BATTLE_SIZE)
            {
                Array.Resize(ref _positionRewards, BOSS_BATTLE_SIZE);
                Debug.Log("Boss battle mode: Position rewards resized to 2 elements");
            }
        }
        else
        {
            if (_positionRewards != null && _positionRewards.Length != NORMAL_RACE_SIZE)
            {
                Array.Resize(ref _positionRewards, NORMAL_RACE_SIZE);
                Debug.Log("Normal race mode: Position rewards resized to 6 elements");
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
#endif

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
        if (_isBossBattle && racersCount != BOSS_BATTLE_SIZE)
        {
            Debug.LogError("Boss battle should have exactly 2 participants!", this);
            return false;
        }

        if (!_isBossBattle && _positionRewards.Length != racersCount)
        {
            Debug.LogError(string.Format(ERROR_REWARDS_SIZE, _positionRewards.Length, racersCount), this);
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

    public void SetBossBattleMode(bool isBossBattle)
    {
        _isBossBattle = isBossBattle;
    }

    private void SetupPlatformSpecificUI()
    {
        bool isMobile = YandexGame.EnvironmentData.isMobile;
        _currentPlayerUIElements = isMobile ? _mobilePlayerDisabledUIElements : _desktopPlayerDisabledUIElements;
        _currentRewardCanvas = isMobile ? _mobileRewardMenu : _desktopRewardMenu;
        _currentRacerDisplays = isMobile ? _mobileRacerDisplays : _desktopRacerDisplays;
    }

    private void InitializeSystems()
    {
        _finisher = new RaceProgressFinisher(GetResultTextsArray(), _positionRewards);

        for (int i = 0; i < _currentRacerDisplays.Length; i++)
        {
            if (_currentRacerDisplays[i] != null && _currentRacerDisplays[i].RespectText != null)
            {
                _currentRacerDisplays[i].RespectText.gameObject.SetActive(false);
            }
        }
    }

    private TextMeshProUGUI[] GetResultTextsArray()
    {
        int arraySize = _isBossBattle ? BOSS_BATTLE_SIZE : _currentRacerDisplays.Length;
        TextMeshProUGUI[] resultTexts = new TextMeshProUGUI[arraySize];

        for (int i = 0; i < arraySize; i++)
        {
            if (i < _currentRacerDisplays.Length && _currentRacerDisplays[i] != null)
            {
                resultTexts[i] = _currentRacerDisplays[i].ResultText;
            }
        }

        return resultTexts;
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

    private int GetRespectChange(int position)
    {
        if (_isBossBattle)
        {
            return position == 1 ? _bossWinRespect : _bossLoseRespect;
        }

        if (position <= 3)
        {
            return _respectRewards[position - 1];
        }

        if (position <= 6)
        {
            return -_positionPenalties[position - 4];
        }

        return 0;
    }

    private void DisplayRespectChange(int position)
    {
        if (position < 1 || position > _currentRacerDisplays.Length)
            return;

        int index = position - 1;
        if (_currentRacerDisplays[index] == null || _currentRacerDisplays[index].RespectText == null)
            return;

        int respectChange = GetRespectChange(position);

        if (respectChange != 0)
        {
            _currentRacerDisplays[index].RespectText.gameObject.SetActive(true);

            bool isPositive = respectChange > 0;
            string sign = isPositive ? POSITIVE_SIGN : NEGATIVE_SIGN;
            int absValue = Mathf.Abs(respectChange);

            _currentRacerDisplays[index].RespectText.text = string.Format(RESPECT_FORMAT, sign, absValue);
            _currentRacerDisplays[index].RespectText.ForceMeshUpdate();
        }
        else
        {
            _currentRacerDisplays[index].RespectText.gameObject.SetActive(false);
        }
    }

    private void ApplyRespectForPosition(int position)
    {
        try
        {
            int respectChange = GetRespectChange(position);
            if (respectChange != 0)
            {
                YandexGame.savesData.AddRespect(respectChange);
                Debug.Log($"Applied respect change for position {position}: {respectChange}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error applying respect changes: {ex.Message}");
        }
    }

    private void ProcessRaceResults(Racer[] racers, Racer finishingRacer)
    {
        _finisher.PrintFinalResults(racers, finishingRacer);

        if (!_isRewardGiven && finishingRacer != null)
        {
            int position = finishingRacer.Position;

            YandexGame.savesData.AddMoney(_finisher.GetRewardForPosition(position));
            ApplyRespectForPosition(position);
            DisplayRespectChange(position);

            if (_isBossBattle && _currentBoss != BossType.None && position == 1)
            {
                SaveBossVictory();
            }

            _isRewardGiven = true;
        }
    }

    private void SaveBossVictory()
    {
        try
        {
            int bossIndex = (int)_currentBoss;
            YandexGame.savesData.SetBossDefeated(bossIndex);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error saving boss victory: {ex.Message}");
        }
    }

    private bool ValidateSerializedData()
    {
        bool isValid = true;

        if (!_isBossBattle)
        {
            if (_respectRewards == null || _respectRewards.Length != 3)
            {
                Debug.LogError(ERROR_RESPECT_REWARDS, this);
                isValid = false;
            }

            if (_positionPenalties == null || _positionPenalties.Length != 3)
            {
                Debug.LogError(ERROR_PENALTIES_LENGTH, this);
                isValid = false;
            }
        }

        if (_positionRewards == null || _positionRewards.Length == 0)
        {
            Debug.LogError(ERROR_REWARDS_EMPTY, this);
            isValid = false;
        }

        if (_isBossBattle && _positionRewards.Length != BOSS_BATTLE_SIZE)
        {
            Debug.LogError("Boss battle requires exactly 2 position rewards!", this);
            isValid = false;
        }

        if (_desktopRewardMenu == null || _mobileRewardMenu == null)
        {
            Debug.LogError(ERROR_CANVASES_NULL, this);
            isValid = false;
        }

        if (_desktopPlayerDisabledUIElements == null || _desktopPlayerDisabledUIElements.Length == 0 ||
            _mobilePlayerDisabledUIElements == null || _mobilePlayerDisabledUIElements.Length == 0)
        {
            Debug.LogError(ERROR_UI_ELEMENTS, this);
            isValid = false;
        }

        if (_pauseManager == null)
        {
            Debug.LogError(ERROR_PAUSE_MANAGER, this);
            isValid = false;
        }

        return isValid;
    }
}