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

    public void PlaySoundWithPitchChange()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource не назначен!");
            return;
        }

        audioSource.pitch = originalPitch;
        audioSource.volume = originalVolume;
        audioSource.Play();

        StartCoroutine(ChangePitchAndVolumeRoutine());
    }

    private IEnumerator ChangePitchAndVolumeRoutine()
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


//public class PitchChanger : MonoBehaviour
//{

//    [SerializeField] private AudioSource audioSource;
//    [SerializeField] private float duration = 0.5f;
//    [SerializeField] private float targetPitch = 1.2f;
//    [SerializeField] private float originalPitch = 0.9f;

//    public void PlaySoundWithPitchChange()
//    {
//        if (audioSource == null)
//        {
//            Debug.LogError("AudioSource не назначен!");
//            return;
//        }

//        // —бросить pitch к исходному значению перед воспроизведением
//        audioSource.pitch = originalPitch;

//        // ¬оспроизвести звук
//        audioSource.Play();

//        // «апустить корутину дл€ изменени€ pitch
//        StartCoroutine(ChangePitchRoutine());
//    }

//    private IEnumerator ChangePitchRoutine()
//    {
//        float timeElapsed = 0f;

//        // ѕлавно увеличиваем pitch
//        while (timeElapsed < duration / 2)
//        {
//            timeElapsed += Time.deltaTime;
//            audioSource.pitch = Mathf.Lerp(originalPitch, targetPitch, timeElapsed / (duration / 2));
//            yield return null;
//        }

//        timeElapsed = 0f;

//        // ѕлавно уменьшаем pitch обратно
//        while (timeElapsed < duration / 2)
//        {
//            timeElapsed += Time.deltaTime;
//            audioSource.pitch = Mathf.Lerp(targetPitch, originalPitch, timeElapsed / (duration / 2));
//            yield return null;
//        }

//        // ”бедитьс€, что pitch возвращаетс€ точно к исходному значению
//        audioSource.pitch = originalPitch;
//    }
//}