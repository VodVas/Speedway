#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class AudioListenerCleaner
{
    [MenuItem("Tools/Apocalypse/Remove All Audio Listeners")]
    private static void RemoveAllAudioListeners()
    {
        AudioListener[] listeners = Object.FindObjectsOfType<AudioListener>(true);

        if (listeners.Length == 0)
        {
            Debug.Log("No AudioListeners found in the scene");
            return;
        }

        foreach (AudioListener listener in listeners)
        {
            Undo.DestroyObjectImmediate(listener);
        }

        Debug.Log($"Removed {listeners.Length} AudioListener(s). You can undo this action with Ctrl+Z");
    }
} 
#endif