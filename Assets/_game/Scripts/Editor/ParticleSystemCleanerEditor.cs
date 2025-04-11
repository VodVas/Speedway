using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class ParticleSystemCleanerEditor
{
    [MenuItem("Tools/Apocalypse/Remove All Particle Systems")]
    public static void RemoveAllParticleSystems()
    {
        List<ParticleSystem> systemsToRemove = new List<ParticleSystem>();

        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject root in rootObjects)
        {
            systemsToRemove.AddRange(root.GetComponentsInChildren<ParticleSystem>(true));
        }

        Undo.IncrementCurrentGroup();

        foreach (ParticleSystem system in systemsToRemove)
        {
            Undo.DestroyObjectImmediate(system);
        }

        Undo.SetCurrentGroupName("Remove All Particle Systems");

        Debug.Log($"Removed {systemsToRemove.Count} particle systems");
    }
}