using UnityEngine;
using YG;

public class ResetSavesTest : MonoBehaviour
{
    public void ResetSaves()
    {
        PlayerPrefs.DeleteKey("savesData");
        PlayerPrefs.Save();

        YandexGame.ResetSaveProgress();
        Debug.Log("Сохранения сброшены.");
    }
}