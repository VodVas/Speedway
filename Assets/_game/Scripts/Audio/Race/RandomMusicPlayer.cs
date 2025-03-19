using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] _tracks;
    [SerializeField] private bool _playOnStart = true;
    [SerializeField] private bool _loopQueue = false;

    private AudioSource _audioSource;
    private int[] _shuffledIndices;
    private int _currentTrackIndex;
    private bool _isPlaying;

    private void Awake()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (_playOnStart)
        {
            StartRandomPlay();
        }
    }

    private void Update()
    {
        if (_isPlaying && !_audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    public void StartRandomPlay()
    {
        if (_tracks == null || _tracks.Length == 0)
        {
            Debug.LogWarning("[RandomMusicPlayer] Список AudioClip пуст, нечего проигрывать.", this);
            return;
        }

        ShuffleTracks();
        _currentTrackIndex = 0;
        PlayTrackByIndex(_shuffledIndices[_currentTrackIndex]);
    }

    public void StopPlay()
    {
        _isPlaying = false;
        _audioSource.Stop();
    }

    private void PlayNextTrack()
    {
        _currentTrackIndex++;
        if (_currentTrackIndex >= _shuffledIndices.Length)
        {
            if (_loopQueue)
            {
                ShuffleTracks();
                _currentTrackIndex = 0;
                PlayTrackByIndex(_shuffledIndices[_currentTrackIndex]);
            }
            else
            {
                StopPlay();
            }
        }
        else
        {
            PlayTrackByIndex(_shuffledIndices[_currentTrackIndex]);
        }
    }

    private void PlayTrackByIndex(int clipIndex)
    {
        AudioClip clip = _tracks[clipIndex];
        if (clip == null)
        {
            Debug.LogWarning($"[RandomMusicPlayer] Пустой AudioClip в _tracks[{clipIndex}].", this);
            return;
        }

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
        _isPlaying = true;
    }

    private void ShuffleTracks()
    {
        if (_shuffledIndices == null || _shuffledIndices.Length != _tracks.Length)
        {
            _shuffledIndices = new int[_tracks.Length];
            for (int i = 0; i < _shuffledIndices.Length; i++)
            {
                _shuffledIndices[i] = i;
            }
        }

        for (int i = 0; i < _shuffledIndices.Length; i++)
        {
            int randomIndex = Random.Range(i, _shuffledIndices.Length);
            int temp = _shuffledIndices[i];
            _shuffledIndices[i] = _shuffledIndices[randomIndex];
            _shuffledIndices[randomIndex] = temp;
        }
    }
}