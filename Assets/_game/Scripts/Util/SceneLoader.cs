using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName ;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (_button == null)
        {
            Debug.LogError("SceneLoader requires a Button component to be attached to the same GameObject.");
            enabled = false;
            return;
        }

        ValidateSceneName();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadScene);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadScene);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneName);
    }

    private void ValidateSceneName()
    {
        if (string.IsNullOrEmpty(_sceneName))
        {
            Debug.LogError("Scene name is not set");
            enabled = false;
            return;
        }
    }

    public void SetSceneName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name cannot be null or empty.");
            return;
        }

        _sceneName = sceneName;
    }
}