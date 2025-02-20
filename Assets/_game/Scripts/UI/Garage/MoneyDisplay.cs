using TMPro;
using UnityEngine;
using YG;
using Zenject;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    //[Inject] private SaveService _saveManager;

    private void Start()
    {
        UpdateMoneyDisplay();
    }

    private void OnEnable()
    {
        YandexGame.savesData.OnMoneyChanged += UpdateMoneyDisplay;
    }

    private void OnDisable()
    {
        YandexGame.savesData.OnMoneyChanged -= UpdateMoneyDisplay;
    }

    public void UpdateMoneyDisplay()
    {
        moneyText.text = $"{YandexGame.savesData.Money}";
    }
}