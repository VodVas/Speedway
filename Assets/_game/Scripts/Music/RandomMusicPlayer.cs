using System.Collections.Generic;
using UnityEngine;

public class RandomMusicPlayer : MonoBehaviour
{
    public AudioClip[] musicClips; // ������ � ������������
    private AudioSource audioSource;
    private List<int> availableIndices = new List<int>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("�� ������ ��������� AudioSource �� ���� �������.");
            return;
        }

        if (musicClips == null || musicClips.Length == 0)
        {
            Debug.LogError("������ musicClips ���� ��� �� �����.");
            return;
        }

        // ��������� ������ ��������� ��������
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
            Debug.Log("��� ����� ���� ��������������.");
            return;
        }

        int randomIndex = Random.Range(0, availableIndices.Count);
        int clipIndexToPlay = availableIndices[randomIndex];

        audioSource.clip = musicClips[clipIndexToPlay];
        audioSource.Play();

        // ������� �������������� ������, ����� �� ������ �� ���������
        availableIndices.RemoveAt(randomIndex);

        // ������������� �� ������� ��������� ������������ �����
        audioSource.loop = false;
        audioSource.Play();
        Invoke(nameof(PlayNextRandomClip), audioSource.clip.length);
    }
}