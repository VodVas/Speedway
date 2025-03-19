using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Player[] _playerCars;
    [SerializeField] private GameObject[] _forceDisableObjects;

    private bool _isPaused;
    private bool[] _cachedStates;

    private void Start()
    {
        CachePlayersState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (_isPaused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    private void CachePlayersState()
    {
        _cachedStates = new bool[_playerCars.Length];

        for (int i = 0; i < _playerCars.Length; i++)
        {
            if (_playerCars[i] != null)
            {
                _cachedStates[i] = _playerCars[i].gameObject.activeSelf;
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;

        SetPlayersActive(false);

        SetGroupActive(_forceDisableObjects, false);

        if (_pauseMenu)
        {
            _pauseMenu.SetActive(true);
        }

        _isPaused = true;
    }

    private void UnPause()
    {
        Time.timeScale = 1f;

        RestorePlayersState();

        SetGroupActive(_forceDisableObjects, true);

        if (_pauseMenu)
        {
            _pauseMenu.SetActive(false);
        }

        _isPaused = false;
    }

    private void SetPlayersActive(bool state)
    {
        for (int i = 0; i < _playerCars.Length; i++)
        {
            var player = _playerCars[i];

            if (player != null && player.gameObject)
            {
                player.gameObject.SetActive(state);
            }
        }
    }

    private void SetGroupActive(GameObject[] group, bool state)
    {
        if (group == null) return;

        for (int i = 0; i < group.Length; i++)
        {
            var obj = group[i];

            if (obj != null)
            {
                obj.SetActive(state);
            }
        }
    }

    private void RestorePlayersState()
    {
        for (int i = 0; i < _playerCars.Length; i++)
        {
            if (_playerCars[i] != null)
            {
                _playerCars[i].gameObject.SetActive(_cachedStates[i]);
            }
        }
    }
}