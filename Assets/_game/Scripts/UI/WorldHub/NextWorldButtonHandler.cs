using UnityEngine;
using UnityEngine.UI;
using YG;

public class NextWorldButtonHandler : MonoBehaviour
{
    [SerializeField] private RaceRewardHandler.BossType _requiredBoss;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Blinker _blinker;
    [SerializeField] private Transform _arrowImage;

    private void Awake()
    {
        if (!ValidateDependencies())
        {
            enabled = false;
            return;
        }

        UpdateButtonState();
    }

    private bool ValidateDependencies()
    {
        if (_nextLevelButton == null)
        {
            Debug.LogError("Button reference is missing!", this);
            return false;
        }

        if (_blinker == null)
        {
            Debug.LogError("Blinker reference is missing!", this);
            return false;
        }

        if (_requiredBoss == RaceRewardHandler.BossType.None)
        {
            Debug.LogError("No boss selected as requirement.", this);
            return false;
        }

        return true;
    }

    private void UpdateButtonState()
    {
        int bossIndex = (int)_requiredBoss;
        bool isBossDefeated = YandexGame.savesData.IsBossDefeated(bossIndex);
        _nextLevelButton.interactable = isBossDefeated;
        _blinker.enabled = isBossDefeated;
        _arrowImage.gameObject.SetActive(isBossDefeated);
    }
}