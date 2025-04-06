using UnityEngine;
using UnityEngine.UI;

public class LootboxReloadButton : MonoBehaviour
{
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _reloadButtonObject;
    [SerializeField] private GameObject[] _disableObjects;
    [SerializeField] private LootBoxController _lootBoxController;
    [SerializeField] private BoxOpener[] _boxOpener;
    [SerializeField] private Button _reloadButton;

    private void OnEnable()
    {
        _reloadButton.onClick.AddListener(ResetObjects);
    }

    private void OnDisable()
    {
        _reloadButton.onClick.RemoveListener(ResetObjects);
    }

    [ContextMenu("ResetObjects")]
    private void ResetObjects()
    {
        _startButton.SetActive(true);

        for (int i = 0; i < _boxOpener.Length; i++)
        {
            _boxOpener[i].ResetState();
        }

        for (int i = 0; i < _disableObjects.Length; i++)
        {
            _disableObjects[i].SetActive(false);
        }


        _lootBoxController.ResetLootBox();
        _reloadButtonObject.SetActive(false);
    }
}