using UnityEngine;
using YG;

public class PurchaseHandler : MonoBehaviour
{
    private const string Purchase100Money = "100";
    private const string Purchase500Money = "500";
    private const string Purchase1000Money = "1000";

    [SerializeField] private GameObject _confirmWindow;
    [SerializeField] private GameObject _errorWindow;

    private void OnEnable()
    {
        YandexGame.PurchaseSuccessEvent += SuccessPurchased;
        YandexGame.PurchaseFailedEvent += FailedPurchased;
    }

    private void OnDisable()
    {
        YandexGame.PurchaseSuccessEvent -= SuccessPurchased;
        YandexGame.PurchaseFailedEvent -= FailedPurchased;
    }

    void SuccessPurchased(string id)
    {
        Debug.Log($"Покупка успешна: {id}");

        switch (id)
        {
            case Purchase100Money:
                YandexGame.savesData.AddMoney(100);
                break;

            case Purchase500Money:
                YandexGame.savesData.AddMoney(500);
                break;

            case Purchase1000Money:
                YandexGame.savesData.AddMoney(1000);
                break;
        }

        YandexGame.ConsumePurchaseByID(id);
        YandexGame.SaveProgress();

        ShowPurchaseSuccessMessage();
    }

    void FailedPurchased(string id)
    {
        Debug.Log($"Покупка не удалась: {id}");

        ShowPurchaseFailedMessage();
    }

    private void ShowPurchaseSuccessMessage()
    {
        _confirmWindow.SetActive(true);
    }

    private void ShowPurchaseFailedMessage()
    {
        _errorWindow.SetActive(true);
    }
}