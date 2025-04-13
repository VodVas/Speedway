using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Button))]
public class BossAccessHandler : MonoBehaviour
{
    [SerializeField] private int _requiredRespect = 100;
    [SerializeField] private GameObject _lockPanel;
    [SerializeField] private TextMeshProUGUI _requirementText;
    [SerializeField] private int _furyTest = 50;


    private Button _button;
    private bool _componentsCached;

    private void Awake()
    {
        CacheComponents();
        InitializeElements();
    }

    private void OnEnable()
    {
        UpdateAllElements();
    }

    [ContextMenu("FuryTest")]
    public void FuryTest()
    {
        if (YandexGame.savesData == null)
        {
            Debug.LogError("Saves not loaded!");
            return;
        }

        YandexGame.savesData.AddRespect(_furyTest);
        YandexGame.SaveProgress();
        UpdateAllElements();

        Debug.Log($"Current fury after test: {YandexGame.savesData.GetRespect()}");
    }

    private void CacheComponents()
    {
        if (_componentsCached) return;

        _button = GetComponent<Button>();

        if (_lockPanel == null)
        { 
            Debug.Log("LockPanel reference is missing!", this);
            enabled = false;
            return;
        }

        if (_requirementText == null)
        { 
            Debug.Log("RequirementText reference is missing!", this);
            enabled = false;
            return;
        }

        _componentsCached = true;
    }

    private void InitializeElements()
    {
        _requirementText.text = $"Требует\nранг:{_requiredRespect}";
    }

    private void UpdateAllElements()
    {
        if (YandexGame.savesData == null) return;

        int currentFury = YandexGame.savesData.GetRespect();
        UpdateButtonState(currentFury);
    }

    private void UpdateButtonState(int currentFury)
    {
        bool isUnlocked = currentFury >= _requiredRespect;
        _button.interactable = isUnlocked;
        _lockPanel.SetActive(!isUnlocked);
    }
}