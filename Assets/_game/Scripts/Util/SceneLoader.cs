using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;

    public void LoadScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("Неверный индекс сцены!");
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Имя сцены не указано!");
            return;
        }

        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            Debug.LogWarning($"Сцена \"{sceneName}\" уже загружена.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}