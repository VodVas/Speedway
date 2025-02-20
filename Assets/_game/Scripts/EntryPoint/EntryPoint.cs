using UnityEngine;
using YG;

public class EntryPoint : MonoBehaviour
{
    private void Awake()
    {
        LoadGame();
    }

    private void LoadGame()
    {
        YandexGame.LoadProgress();

        if (YandexGame.savesData.isFirstSession)
        {
            YandexGame.savesData.isFirstSession = false;
        }
    }
}