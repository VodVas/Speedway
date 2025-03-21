using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PitchChanger))]
public class SoundButton : MonoBehaviour
{
    [SerializeField] private Button _targetButton;
    [SerializeField] private float _soundDelay = 1f;
    [SerializeField] private PitchChanger _pitchChanger;

    private WaitForSeconds _cachedWait;

    protected virtual void Awake()
    {
        ValidateComponents();
        CacheWaitObjects();
    }

    private void ValidateComponents()
    {
        if (_targetButton == null)
        {
            _targetButton = GetComponent<Button>();

            if (_targetButton == null)
            {
                Debug.LogError("Button component is missing.", this);
                enabled = false;
                return;
            }
        }

        if (_pitchChanger == null)
        {
            _pitchChanger = GetComponent<PitchChanger>();

            if (_pitchChanger == null)
            {
                Debug.LogError("PitchChanger component is missing.", this);
                enabled = false;
            }
        }
    }

    private void CacheWaitObjects()
    {
        _cachedWait = new WaitForSeconds(_soundDelay);
    }

    private void OnEnable()
    {
        _targetButton.onClick.AddListener(HandleButtonClick);
    }

    private void OnDisable()
    {
        _targetButton.onClick.RemoveListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        StartCoroutine(PlaySoundRoutine());
    }

    private IEnumerator PlaySoundRoutine()
    {
        _pitchChanger.PlaySound();
        yield return _cachedWait;
        OnSoundCompleted();
    }

    protected virtual void OnSoundCompleted() { }
}

//public class SoundButton : MonoBehaviour
//{
//    private const string SceneTitle = "CarShop";

//    [SerializeField] private Button _startNewGame;
//    [SerializeField] private PitchChanger _pitchChanger;
//    [SerializeField] private float _waitBeforeLoad = 1f;

//    private WaitForSeconds _wait;
//    private SceneLoader _sceneLoader;

//    private void Awake()
//    {
//        if (_startNewGame == null)
//        {
//            Debug.LogError("StartNewGame button is not set.");
//            enabled = false;
//            return;
//        }

//        if (_pitchChanger == null)
//        {
//            Debug.LogError("PitchChanger is not set.");
//            enabled = false;
//            return;
//        }

//        _wait = new WaitForSeconds(_waitBeforeLoad);
//        _sceneLoader = GetComponent<SceneLoader>();

//        if (_sceneLoader == null)
//        {
//            Debug.LogError("SceneLoader component is not found.");
//            enabled = false;
//            return;
//        }

//        _sceneLoader.SetSceneName(SceneTitle);
//    }

//    private void OnEnable()
//    {
//        _startNewGame.onClick.AddListener(BeginGameWithDelay);
//    }

//    private void OnDisable()
//    {
//        _startNewGame.onClick.RemoveListener(BeginGameWithDelay);
//    }


//    private void BeginGameWithDelay()
//    {
//        StartCoroutine(DelayingLoadScene());
//    }

//    private IEnumerator DelayingLoadScene()
//    {
//        _pitchChanger.PlaySound();

//        yield return _wait;

//        _sceneLoader.LoadScene();
//    }
//}