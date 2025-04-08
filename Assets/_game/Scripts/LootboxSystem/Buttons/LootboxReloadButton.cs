using UnityEngine;
using UnityEngine.UI;

public class LootboxReloadButton : MonoBehaviour
{
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _reloadButtonObject;
    [SerializeField] private GameObject _buyCurrencyButtonObject;
    [SerializeField] private GameObject[] _disableObjects;
    [SerializeField] private LootBoxController _lootBoxController;
    [SerializeField] private BoxOpener[] _boxOpener;
    [SerializeField] private Button _reloadButton;

    private void OnEnable()
    {
        _reloadButton.onClick.AddListener(ResetObjects);
        _lootBoxController.LootboxesShowed += ShowReloadButton;
    }

    private void OnDisable()
    {
        _reloadButton.onClick.RemoveListener(ResetObjects);
        _lootBoxController.LootboxesShowed -= ShowReloadButton;
    }

    private void ShowReloadButton()
    {
        _reloadButtonObject.SetActive(true);
    }

    private void ResetObjects()
    {
        _startButton.SetActive(true);
        _lootBoxController.ResetLootBox();

        for (int i = 0; i < _boxOpener.Length; i++)
        {
            _boxOpener[i].ResetState();
        }

        for (int i = 0; i < _disableObjects.Length; i++)
        {
            _disableObjects[i].SetActive(false);
        }

        _reloadButtonObject.SetActive(false);
        _buyCurrencyButtonObject.SetActive(true);
    }
}