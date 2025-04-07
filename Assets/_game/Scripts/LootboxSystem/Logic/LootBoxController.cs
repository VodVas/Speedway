using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _guaranteedEpicAfterAttempts = 5;
    [SerializeField] private float _commonChance = 0.5f;
    [SerializeField] private float _rareChance = 0.2f;
    [SerializeField] private float _uniqueChance = 0.1f;
    [SerializeField] private float _legendaryChance = 0.1f;
    [SerializeField] private float _epicChance = 0.1f;
    [SerializeField] private float _waitBetweenCardSpawning = 0.5f;

    [Header("References")]
    [SerializeField] private BoxCrackingProcess _boxCrackingProcess;
    [SerializeField] private LootDatabaseMediator _databaseMediator;
    [SerializeField] private LootVisualizationBuilder _visualizationBuilder;
    [SerializeField] private PlayerProgressBridge _progressBridge;

    private LootGeneratorCore _lootGenerator;
    private int _selectedIndex = -1;
    private LootGeneratorCore.LootResult[] _currentResults;
    private WaitForSeconds _wait;

    public event Action LootboxesShowed;

    private void Awake()
    {
        _wait = new WaitForSeconds(_waitBetweenCardSpawning);

        ValidateComponents();
    }

    private void Start()
    {
        InitializeSystem();
    }

    private void OnDisable()
    {
        if (_boxCrackingProcess != null)
        {
            _boxCrackingProcess.BoxOpeningAnimationComplete -= OnBoxOpeningComplete;
        }

        if (_visualizationBuilder != null)
        {
            _visualizationBuilder.CardVisualized -= OnCardVisualized;
        }
    }

    private void ValidateComponents()
    {
        if (_boxCrackingProcess == null || _databaseMediator == null ||
            _visualizationBuilder == null || _progressBridge == null)
        {
            Debug.Log("[LootBoxController] Required components are missing!");
            enabled = false;
            return;
        }
    }

    private void InitializeSystem()
    {
        _lootGenerator = new LootGeneratorCore(
            _guaranteedEpicAfterAttempts, 3,
            _commonChance, _rareChance, _uniqueChance,
            _legendaryChance, _epicChance);

        _boxCrackingProcess.BoxOpeningAnimationComplete += OnBoxOpeningComplete;
        _visualizationBuilder.CardVisualized += OnCardVisualized;
    }

    private void OnBoxOpeningComplete(int selectedButtonIndex)
    {
        _selectedIndex = selectedButtonIndex;
        _currentResults = _lootGenerator.GenerateLootSet();

        ProcessCard(selectedButtonIndex, _currentResults[selectedButtonIndex]);
        StartCoroutine(ProcessRemainingCards());
    }

    private IEnumerator ProcessRemainingCards()
    {
        yield return _wait;

        for (int i = 0; i < _currentResults.Length; i++)
        {
            if (i != _selectedIndex)
            {
                ProcessCard(i, _currentResults[i]);

                yield return _wait;
            }
        }

        LootboxesShowed?.Invoke();
    }

    private void ProcessCard(int cardIndex, LootGeneratorCore.LootResult result)
    {
        switch (result.Type)
        {
            case LootRewardType.Car:
                var carItem = _databaseMediator.GetRandomCarItem(result.Rarity);
                if (carItem != null)
                {
                    _visualizationBuilder.VisualizeCar(cardIndex, carItem);
                }
                break;

            case LootRewardType.Money:
                var moneyItem = _databaseMediator.GetRandomMoneyItem(result.Rarity);
                if (moneyItem != null)
                {
                    _visualizationBuilder.VisualizeMoney(cardIndex, moneyItem);
                }
                break;

            case LootRewardType.Paint:
                var paintItem = _databaseMediator.GetRandomPaintItem(result.Rarity);
                if (paintItem != null)
                {
                    _visualizationBuilder.VisualizePaint(cardIndex, paintItem, result.Rarity);
                }
                break;
        }
    }

    private void OnCardVisualized(int cardIndex, LootRewardType type, object item)
    {
        if (cardIndex != _selectedIndex)
            return;

        switch (type)
        {
            case LootRewardType.Car:
                if (item is CarLootItem lootCar)
                {
                    _progressBridge.UnlockCar(lootCar.CarId);
                }
                break;

            case LootRewardType.Money:
                if (item is MoneyLootItem moneyItem)
                {
                    _progressBridge.AddMoney(moneyItem.Amount);
                }
                break;

            case LootRewardType.Paint:
                if (item is LootPaintSphere sphere)
                {
                    int paintId = sphere.GetPaintId();
                    _progressBridge.UnlockPaint(paintId);
                }
                break;
        }
    }

    public void ResetLootBox()
    {
        _boxCrackingProcess.ResetState();
        _visualizationBuilder.ResetAllVisuals();
        _selectedIndex = -1;
        _currentResults = null;
    }
}