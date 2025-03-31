using UnityEngine;
using UnityEngine.UI;


public class LootBoxManager : MonoBehaviour
{
    [SerializeField] private LootCardController[] _lootCards;
    [SerializeField] private LootDatabase _lootDatabase;
    [SerializeField] private LootSpheresSpawner _lootSpheresSpawner;
    [SerializeField] private Button[] _boxButtons;
    [SerializeField] private Transform[] _spawnPoints;

    private void Awake()
    {
        InitializeSystem();
    }

    private void OnDisable()
    {
        UnsubscribeFromButtons();
    }

    private void InitializeSystem()
    {
        _lootDatabase.Initialize();

        SubscribeToButtons();
    }

    private void SubscribeToButtons()
    {
        for (int i = 0; i < _boxButtons.Length; i++)
        {
            int index = i;
            _boxButtons[i].onClick.AddListener(() => OnBoxSelected(index));
        }
    }

    private void OnBoxSelected(int selectedIndex)
    {
        DisableAllButtons();

        if (_lootCards.Length != _spawnPoints.Length)
        {
            Debug.LogError("[LootBoxManager] Arrays size mismatch!");
            enabled = false;
            return;
        }

        for (int i = 0; i < _lootCards.Length; i++)
        {
            var card = _lootCards[i];
            var spawnPoint = _spawnPoints[i];

            Rarity rarity = _lootDatabase.GetRandomRarity();
            LootItem item = _lootDatabase.GetRandomItem(rarity);

            if (_lootSpheresSpawner != null && spawnPoint != null)
            {
                LootSphere sphere = _lootSpheresSpawner.SpawnLootSphere(rarity, spawnPoint.position);
                card.gameObject.SetActive(true);
                card.ShowCard(item, sphere.gameObject);
            }
            else
            {
                Debug.LogWarning("[LootBoxManager] Spawner or SpawnPoint is null!");
                enabled = false;
                return;
            }
        }
    }

    private void DisableAllButtons()
    {
        for (int i = 0; i < _boxButtons.Length; i++)
        {
            _boxButtons[i].interactable = false;
        }
    }

    private void UnsubscribeFromButtons()
    {
        for (int i = 0; i < _boxButtons.Length; i++)
        {
            _boxButtons[i].onClick.RemoveAllListeners();
        }
    }
}