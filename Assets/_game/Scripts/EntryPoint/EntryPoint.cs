using UnityEngine;
using YG;

public class EntryPoint : MonoBehaviour
{
    private void Awake()
    {
        YandexGame.LoadProgress();

        if (YandexGame.savesData.isFirstSession)
        {
            YandexGame.savesData.isFirstSession = false;
            // YandexGame.savesData.AddMoney(1500);
            // YandexGame.savesData.AddCar(0);
            YandexGame.SaveProgress();
        }
    }
}