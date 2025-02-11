using System.Collections;
using UnityEngine;

public class PitchChanger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float targetPitch = 1.2f;
    [SerializeField] private float originalPitch = 0.9f;
    [SerializeField] private float targetVolume = 1.0f;
    [SerializeField] private float originalVolume = 0.5f;

    public void PlaySound()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource не назначен!");
            return;
        }

        audioSource.pitch = originalPitch;
        audioSource.volume = originalVolume;
        audioSource.Play();

        StartCoroutine(AudioToneTransition());
    }

    private IEnumerator AudioToneTransition()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration / 2)
        {
            timeElapsed += Time.deltaTime;

            audioSource.pitch = Mathf.Lerp(originalPitch, targetPitch, timeElapsed / (duration / 2));
            audioSource.volume = Mathf.Lerp(originalVolume, targetVolume, timeElapsed / (duration / 2));

            yield return null;
        }

        timeElapsed = 0f;

        while (timeElapsed < duration / 2)
        {
            timeElapsed += Time.deltaTime;

            audioSource.pitch = Mathf.Lerp(targetPitch, originalPitch, timeElapsed / (duration / 2));
            audioSource.volume = Mathf.Lerp(targetVolume, originalVolume, timeElapsed / (duration / 2));

            yield return null;
        }

        audioSource.pitch = originalPitch;
        audioSource.volume = originalVolume;
    }
}