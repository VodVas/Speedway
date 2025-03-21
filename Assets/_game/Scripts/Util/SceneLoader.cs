using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private enum LoadSceneMode
    {
        CurrentScene,
        SpecifiedScene
    }

    [SerializeField] private LoadSceneMode _loadMode = LoadSceneMode.SpecifiedScene;
    [SerializeField] private string _sceneName;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (_button == null)
        {
            Debug.LogError("SceneLoader requires a Button component!");
            enabled = false;
            return;
        }

        ValidateSceneSettings();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        switch (_loadMode)
        {
            case LoadSceneMode.CurrentScene:
                ReloadCurrentScene();
                break;
            case LoadSceneMode.SpecifiedScene:
                LoadTargetScene();
                break;
        }
    }

    private void ValidateSceneSettings()
    {
        if (_loadMode == LoadSceneMode.SpecifiedScene && string.IsNullOrEmpty(_sceneName))
        {
            Debug.LogError("Scene name is required in SpecifiedScene mode!");
            enabled = false;
        }
    }

    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(_sceneName))
            SceneManager.LoadScene(_sceneName);
    }

    //private void SetSceneName(string sceneName)
    //{
    //    if (_loadMode != LoadSceneMode.SpecifiedScene)
    //    {
    //        Debug.LogWarning("Scene name change ignored - current mode is not SpecifiedScene");
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(sceneName))
    //    {
    //        Debug.LogError("Invalid scene name!");
    //        return;
    //    }

    //    _sceneName = sceneName;
    //}
}





//public class SceneLoader : MonoBehaviour
//{
//    [SerializeField] private string _sceneName ;

//    private Button _button;

//    private void Awake()
//    {
//        _button = GetComponent<Button>();

//        if (_button == null)
//        {
//            Debug.LogError("SceneLoader requires a Button component to be attached to the same GameObject.");
//            enabled = false;
//            return;
//        }

//        ValidateSceneName();
//    }

//    private void OnEnable()
//    {
//        _button.onClick.AddListener(LoadScene);
//    }

//    private void OnDisable()
//    {
//        _button.onClick.RemoveListener(LoadScene);
//    }

//    public void LoadScene()
//    {
//        SceneManager.LoadScene(_sceneName);
//    }

//    private void ValidateSceneName()
//    {
//        if (string.IsNullOrEmpty(_sceneName))
//        {
//            Debug.LogError("Scene name is not set");
//            enabled = false;
//            return;
//        }
//    }

//    public void SetSceneName(string sceneName)
//    {
//        if (string.IsNullOrEmpty(sceneName))
//        {
//            Debug.LogError("Scene name cannot be null or empty.");
//            return;
//        }

//        _sceneName = sceneName;
//    }
//}