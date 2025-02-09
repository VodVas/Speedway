using System.Collections.Generic;
using UnityEngine;

public class RandomMusicPlayer : MonoBehaviour
{
    public AudioClip[] musicClips; // Массив с аудиоклипами
    private AudioSource audioSource;
    private List<int> availableIndices = new List<int>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Не найден компонент AudioSource на этом объекте.");
            return;
        }

        if (musicClips == null || musicClips.Length == 0)
        {
            Debug.LogError("Массив musicClips пуст или не задан.");
            return;
        }

        // Заполняем список доступных индексов
        for (int i = 0; i < musicClips.Length; i++)
        {
            availableIndices.Add(i);
        }

        PlayNextRandomClip();
    }

    void PlayNextRandomClip()
    {
        if (availableIndices.Count == 0)
        {
            Debug.Log("Все треки были воспроизведены.");
            return;
        }

        int randomIndex = Random.Range(0, availableIndices.Count);
        int clipIndexToPlay = availableIndices[randomIndex];

        audioSource.clip = musicClips[clipIndexToPlay];
        audioSource.Play();

        // Удаляем использованный индекс, чтобы он больше не выбирался
        availableIndices.RemoveAt(randomIndex);

        // Подписываемся на событие окончания проигрывания клипа
        audioSource.loop = false;
        audioSource.Play();
        Invoke(nameof(PlayNextRandomClip), audioSource.clip.length);
    }
}