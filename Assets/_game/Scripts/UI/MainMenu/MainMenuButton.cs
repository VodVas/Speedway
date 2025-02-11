using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Button _startNewGame;
    [SerializeField] private PitchChanger _pitchChanger;
    [SerializeField] private int _sceneIndex = 1;
    [SerializeField] private float _waitBeforeLoad = 1f;


    private WaitForSeconds _wait;
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _wait = new WaitForSeconds(_waitBeforeLoad);
        _sceneLoader = GetComponent<SceneLoader>();
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

        _sceneLoader.LoadScene(_sceneIndex);
    }
}