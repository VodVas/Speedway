using UnityEngine;
using YG;

public class ResetSaves : MonoBehaviour
{
    public void ResetSavesProgress()
    {
        PlayerPrefs.DeleteKey("savesData");
        PlayerPrefs.Save();

        YandexGame.ResetSaveProgress();
        Debug.Log("Сохранения сброшены.");
    }
}