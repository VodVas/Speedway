using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSoundLoadingButton : SoundButton
{
    [SerializeField] private string _targetSceneName;

    protected override void Awake()
    {
        base.Awake();
        InitializeSceneName();
    }

    private void InitializeSceneName()
    {
        if (string.IsNullOrEmpty(_targetSceneName))
        {
            Debug.LogError("Target scene name is not set.", this);
            enabled = false;
            return;
        }
    }

    protected override void OnSoundCompleted()
    {
        SceneManager.LoadScene(_targetSceneName, LoadSceneMode.Single);
    }
}