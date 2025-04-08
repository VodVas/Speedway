using UnityEngine;
using YG;

public class RewardMoneyGiver : MonoBehaviour
{
    [SerializeField] private int _adID = 1;
    [SerializeField] private int _moneyAmount = 100;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnRewarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnRewarded;
    }

    private void OnRewarded(int id)
    {
        if (id == _adID)
        {
            AddMoney();
        }
    }

    private void AddMoney()
    {
        YandexGame.savesData.AddMoney(_moneyAmount);
    }
}