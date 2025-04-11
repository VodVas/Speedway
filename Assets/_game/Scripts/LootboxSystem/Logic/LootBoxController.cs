using System;
using System.Collections;
using UnityEngine;

public class LootBoxController : MonoBehaviour
{
    [Serializable]
    private struct RewardChances
    {
        public float Common;
        public float Rare;
        public float Unique;
        public float Legendary;
        public float Epic;
    }

    [Header("Settings")]
    [SerializeField] private int _guaranteedEpicAfterAttempts = 5;
    [SerializeField] private RewardChances _rewardChances;
    [SerializeField] private float _waitBetweenCardSpawning = 0.5f;

    [Header("References")]
    [SerializeField] private BoxCrackingProcess _boxCrackingProcess;
    [SerializeField] private LootDatabaseMediator _databaseMediator;
    [SerializeField] private LootVisualizationBuilder _visualizationBuilder;
    [SerializeField] private PlayerProgressBridge _progressBridge;

    private LootGeneratorCore _lootGenerator;
    private LootGeneratorCore.LootResult[] _currentResults;
    private WaitForSeconds _waitYieldInstruction;
    private int _selectedIndex = -1;

    public event Action LootboxesShowed;
    private event Action<int> CarUnlocked;

    private void Awake()
    {
        _waitYieldInstruction = new WaitForSeconds(_waitBetweenCardSpawning);
        ValidateCriticalComponents();
    }

    private void Start() => InitializeLootSystem();

    private void OnDisable()
    {
        _boxCrackingProcess.BoxOpeningAnimationComplete -= HandleBoxOpeningComplete;
        _visualizationBuilder.CardVisualized -= HandleCardVisualization;
        CarUnlocked -= HandleCarUnlock;
    }

    private void ValidateCriticalComponents()
    {
        if (!_boxCrackingProcess || !_databaseMediator || !_visualizationBuilder || !_progressBridge)
        {
            enabled = false;
            Debug.LogError("[LootBoxController] Critical components missing!");
        }
    }

    private void InitializeLootSystem()
    {
        _lootGenerator = new LootGeneratorCore(
            _guaranteedEpicAfterAttempts,
            3,
            _rewardChances.Common,
            _rewardChances.Rare,
            _rewardChances.Unique,
            _rewardChances.Legendary,
            _rewardChances.Epic
        );

        _boxCrackingProcess.BoxOpeningAnimationComplete += HandleBoxOpeningComplete;
        _visualizationBuilder.CardVisualized += HandleCardVisualization;
        CarUnlocked += HandleCarUnlock;
    }

    private void HandleBoxOpeningComplete(int selectedIndex)
    {
        _selectedIndex = selectedIndex;
        _currentResults = _lootGenerator.GenerateLootSet();

        ProcessSelectedCard();
        StartCoroutine(ProcessRemainingCardsRoutine());
    }

    private IEnumerator ProcessRemainingCardsRoutine()
    {
        for (int i = 0; i < _currentResults.Length; i++)
        {
            if (i == _selectedIndex) continue;

            yield return _waitYieldInstruction;
            ProcessCard(i, _currentResults[i]);
        }

        LootboxesShowed?.Invoke();
    }

    private void ProcessSelectedCard()
    {
        var selectedResult = _currentResults[_selectedIndex];
        ProcessCard(_selectedIndex, selectedResult);
        HandleRewardApplication(selectedResult);
    }

    private void ProcessCard(int index, LootGeneratorCore.LootResult result)
    {
        switch (result.Type)
        {
            case LootRewardType.Car:
                VisualizeCar(index, result.Rarity);
                break;
            case LootRewardType.Money:
                VisualizeMoney(index, result.Rarity);
                break;
            case LootRewardType.Paint:
                VisualizePaint(index, result.Rarity);
                break;
        }
    }

    private void VisualizeCar(int index, Rarity rarity)
    {
        var carItem = _databaseMediator.GetRandomCarItem(rarity);
        if (!carItem) return;

        _visualizationBuilder.VisualizeCar(index, carItem);
        if (index == _selectedIndex) CarUnlocked?.Invoke(carItem.CarId);
    }

    private void VisualizeMoney(int index, Rarity rarity)
    {
        var moneyItem = _databaseMediator.GetRandomMoneyItem(rarity);
        if (moneyItem) _visualizationBuilder.VisualizeMoney(index, moneyItem);
    }

    private void VisualizePaint(int index, Rarity rarity)
    {
        var paintItem = _databaseMediator.GetRandomPaintItem(rarity);
        if (paintItem) _visualizationBuilder.VisualizePaint(index, paintItem, rarity);
    }

    private void HandleRewardApplication(LootGeneratorCore.LootResult result)
    {
        switch (result.Type)
        {
            case LootRewardType.Money:
                ApplyMoneyReward(result.Rarity);
                break;
            case LootRewardType.Paint:
                ApplyPaintReward(result.Rarity);
                break;
        }
    }

    private void ApplyMoneyReward(Rarity rarity)
    {
        var moneyItem = _databaseMediator.GetRandomMoneyItem(rarity);
        if (moneyItem) _progressBridge.AddMoney(moneyItem.Amount);
    }

    private void ApplyPaintReward(Rarity rarity)
    {
        var paintItem = _databaseMediator.GetRandomPaintItem(rarity);
        if (paintItem is PaintLootItemSO paintSO)
            _progressBridge.UnlockPaint(paintSO.PaintId);
    }

    private void HandleCardVisualization(int index, LootRewardType type, object item)
    {
        if (index != _selectedIndex) return;

        switch (type)
        {
            case LootRewardType.Money when item is MoneyLootItem moneyItem:
                _progressBridge.AddMoney(moneyItem.Amount);
                break;
            case LootRewardType.Paint when item is PaintLootItemSO paintSO:
                _progressBridge.UnlockPaint(paintSO.PaintId);
                break;
        }
    }

    private void HandleCarUnlock(int carId) => _progressBridge.UnlockCar(carId);

    public void ResetLootBox()
    {
        _boxCrackingProcess.ResetState();
        _visualizationBuilder.ResetAllVisuals();
        _selectedIndex = -1;
        _currentResults = null;
    }
}





//public class LootBoxController : MonoBehaviour
//{
//    [Header("Settings")]
//    [SerializeField] private int _guaranteedEpicAfterAttempts = 5;
//    [SerializeField] private float _commonChance = 0.5f;
//    [SerializeField] private float _rareChance = 0.2f;
//    [SerializeField] private float _uniqueChance = 0.1f;
//    [SerializeField] private float _legendaryChance = 0.1f;
//    [SerializeField] private float _epicChance = 0.1f;
//    [SerializeField] private float _waitBetweenCardSpawning = 0.5f;

//    [Header("References")]
//    [SerializeField] private BoxCrackingProcess _boxCrackingProcess;
//    [SerializeField] private LootDatabaseMediator _databaseMediator;
//    [SerializeField] private LootVisualizationBuilder _visualizationBuilder;
//    [SerializeField] private PlayerProgressBridge _progressBridge;

//    private LootGeneratorCore _lootGenerator;
//    private int _selectedIndex = -1;
//    private LootGeneratorCore.LootResult[] _currentResults;
//    private WaitForSeconds _wait;

//    public event Action LootboxesShowed;
//    private event Action<int> CarUnlocked;

//    private void Awake()
//    {
//        _wait = new WaitForSeconds(_waitBetweenCardSpawning);

//        ValidateComponents();
//    }

//    private void Start()
//    {
//        InitializeSystem();
//    }

//    private void OnDisable()
//    {
//        if (_boxCrackingProcess != null)
//        {
//            _boxCrackingProcess.BoxOpeningAnimationComplete -= OnBoxOpeningComplete;
//        }

//        if (_visualizationBuilder != null)
//        {
//            _visualizationBuilder.CardVisualized -= OnCardVisualized;
//        }

//        CarUnlocked -= OnCarUnlocked;
//    }

//    private void OnCarUnlocked(int carId)
//    {
//        _progressBridge.UnlockCar(carId);
//    }

//    private void ValidateComponents()
//    {
//        if (_boxCrackingProcess == null || _databaseMediator == null ||
//            _visualizationBuilder == null || _progressBridge == null)
//        {
//            Debug.Log("[LootBoxController] Required components are missing!");
//            enabled = false;
//            return;
//        }
//    }

//    private void InitializeSystem()
//    {
//        _lootGenerator = new LootGeneratorCore(
//            _guaranteedEpicAfterAttempts, 3,
//            _commonChance, _rareChance, _uniqueChance,
//            _legendaryChance, _epicChance);

//        _boxCrackingProcess.BoxOpeningAnimationComplete += OnBoxOpeningComplete;
//        _visualizationBuilder.CardVisualized += OnCardVisualized;
//        CarUnlocked += OnCarUnlocked;
//    }

//    private void OnBoxOpeningComplete(int selectedButtonIndex)
//    {
//        _selectedIndex = selectedButtonIndex;
//        _currentResults = _lootGenerator.GenerateLootSet();

//        ProcessCard(selectedButtonIndex, _currentResults[selectedButtonIndex]);
//        StartCoroutine(ProcessRemainingCards());
//    }

//    private IEnumerator ProcessRemainingCards()
//    {
//        yield return _wait;

//        for (int i = 0; i < _currentResults.Length; i++)
//        {
//            if (i != _selectedIndex)
//            {
//                ProcessCard(i, _currentResults[i]);

//                yield return _wait;
//            }
//        }

//        LootboxesShowed?.Invoke();
//    }

//    private void ProcessCard(int cardIndex, LootGeneratorCore.LootResult result)
//    {
//        switch (result.Type)
//        {
//            case LootRewardType.Car:
//                var carItem = _databaseMediator.GetRandomCarItem(result.Rarity);
//                if (carItem != null)
//                {
//                    _visualizationBuilder.VisualizeCar(cardIndex, carItem);
//                }

//                if (cardIndex == _selectedIndex)
//                {
//                    CarUnlocked?.Invoke(carItem.CarId);
//                }

//                break;

//            case LootRewardType.Money:
//                var moneyItem = _databaseMediator.GetRandomMoneyItem(result.Rarity);
//                if (moneyItem != null)
//                {
//                    _visualizationBuilder.VisualizeMoney(cardIndex, moneyItem);
//                }
//                break;

//            case LootRewardType.Paint:
//                var paintItem = _databaseMediator.GetRandomPaintItem(result.Rarity);
//                if (paintItem != null)
//                {
//                    _visualizationBuilder.VisualizePaint(cardIndex, paintItem, result.Rarity);
//                }
//                break;
//        }
//    }

//    private void OnCardVisualized(int cardIndex, LootRewardType type, object item)
//    {
//        if (cardIndex != _selectedIndex)
//            return;

//        switch (type)
//        {
//            case LootRewardType.Car:
//                break;

//            case LootRewardType.Money:
//                if (item is MoneyLootItem moneyItem)
//                {
//                    Debug.Log("item is MoneyLootItem moneyItem)");

//                    _progressBridge.AddMoney(moneyItem.Amount);
//                }
//                break;

//            case LootRewardType.Paint:
//                if (item is LootPaintSphere sphere)
//                {
//                    Debug.Log("item is LootPaintSphere sphere");

//                    int paintId = sphere.GetPaintId();
//                    _progressBridge.UnlockPaint(paintId);
//                }
//                break;
//        }
//    }

//    public void ResetLootBox()
//    {
//        _boxCrackingProcess.ResetState();
//        _visualizationBuilder.ResetAllVisuals();
//        _selectedIndex = -1;
//        _currentResults = null;
//    }
//}