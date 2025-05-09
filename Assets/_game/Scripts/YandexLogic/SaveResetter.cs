using UnityEngine;
using YG;

public class SaveResetter : MonoBehaviour
{
    [SerializeField] private TemporaryMessageDisplay _messageDisplay;

    public void Execute()
    {
        PlayerPrefs.DeleteKey("savesData");
        PlayerPrefs.Save();
        YandexGame.ResetSaveProgress();
        _messageDisplay.Show();
        Debug.Log("Сохранения сброшены.");
    }
}