#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class ParticleLinkerAutomator
{
    private static readonly HashSet<string> _forbiddenNames = new()
    {
        "AntiaircraftMachinegunSmart",
        "AntiaircraftMachinegun"
    };

    [MenuItem("Tools/Apocalypse/Auto-Link Enemy Particles")]
    private static void AutoLinkEnemyParticles()
    {
        ParticleWeaponLinker linker = Object.FindFirstObjectByType<ParticleWeaponLinker>();
        if (linker == null)
        {
            Debug.LogError("ParticleWeaponLinker not found in active scene!");
            return;
        }

        var markers = FindMarkersInActiveScene();
        var pairs = new List<WeaponParticlePair>(markers.Count);

        foreach (var marker in markers)
        {
            if (IsForbiddenHierarchy(marker.transform)) continue;

            var weapon = marker.GetComponentInParent<IWeapon>(true) as MonoBehaviour;
            var particle = marker.GetComponent<ParticleSystem>();

            if (weapon != null && particle != null)
            {
                pairs.Add(new WeaponParticlePair(particle, weapon));
            }
        }

        UpdateLinker(linker, pairs);
        Debug.Log($"Successfully linked {pairs.Count} particle systems");
    }

    private static List<ParticleWeaponMarker> FindMarkersInActiveScene()
    {
        var results = new List<ParticleWeaponMarker>();
        var activeScene = SceneManager.GetActiveScene();

        foreach (var root in activeScene.GetRootGameObjects())
        {
            results.AddRange(root.GetComponentsInChildren<ParticleWeaponMarker>(true));
        }

        return results;
    }

    private static bool IsForbiddenHierarchy(Transform child)
    {
        var current = child;
        while (current != null)
        {
            if (_forbiddenNames.Contains(current.name)) return true;
            current = current.parent;
        }
        return false;
    }

    private static void UpdateLinker(ParticleWeaponLinker linker, List<WeaponParticlePair> pairs)
    {
        Undo.RecordObject(linker, "Update Particle Weapon Links");

        var so = new SerializedObject(linker);
        var arrayProp = so.FindProperty("_linkedPairs");
        arrayProp.ClearArray();

        for (int i = 0; i < pairs.Count; i++)
        {
            arrayProp.InsertArrayElementAtIndex(i);
            var element = arrayProp.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("_particle").objectReferenceValue = pairs[i].ParticleSystem;
            element.FindPropertyRelative("_iWeapon").objectReferenceValue = pairs[i].Weapon as MonoBehaviour;
        }

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(linker);
    }
}
#endif