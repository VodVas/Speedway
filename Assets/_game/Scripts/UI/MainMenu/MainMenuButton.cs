using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    private const string SceneTitle = "CarShop";

    [SerializeField] private Button _startNewGame;
    [SerializeField] private PitchChanger _pitchChanger;
    [SerializeField] private float _waitBeforeLoad = 1f;

    private WaitForSeconds _wait;
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        if (_startNewGame == null)
        {
            Debug.LogError("StartNewGame button is not set.");
            enabled = false;
            return;
        }

        if (_pitchChanger == null)
        {
            Debug.LogError("PitchChanger is not set.");
            enabled = false;
            return;
        }

        _wait = new WaitForSeconds(_waitBeforeLoad);
        _sceneLoader = GetComponent<SceneLoader>();

        if (_sceneLoader == null)
        {
            Debug.LogError("SceneLoader component is not found.");
            enabled = false;
            return;
        }

        _sceneLoader.SetSceneName(SceneTitle);
    }

    private void OnEnable()
    {
        _startNewGame.onClick.AddListener(BeginGameWithDelay);
    }

    private void OnDisable()
    {
        _startNewGame.onClick.RemoveListener(BeginGameWithDelay);
    }


    private void BeginGameWithDelay()
    {
        StartCoroutine(DelayingLoadScene());
    }

    private IEnumerator DelayingLoadScene()
    {
        _pitchChanger.PlaySound();

        yield return _wait;

        _sceneLoader.LoadScene();
    }
}