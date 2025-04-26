#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ForbiddenWeaponsLinker
{
    private static readonly string[] _targetParentNames = { "AntiaircraftMachinegunSmart", "AntiaircraftMachinegun" };

    [MenuItem("Tools/Apocalypse/Particle damage/Link Forbidden Weapons")]
    public static void LinkForbiddenWeapons()
    {
        ParticleWeaponLinker linker = Object.FindFirstObjectByType<ParticleWeaponLinker>();
        if (linker == null)
        {
            Debug.LogError("ParticleWeaponLinker not found!");
            return;
        }

        List<WeaponParticlePair> newPairs = new List<WeaponParticlePair>();

        foreach (var parentName in _targetParentNames)
        {
            var parents = GameObject.FindObjectsOfType<GameObject>(true)
                                .Where(g => g.name == parentName && g.activeInHierarchy);

            foreach (var parent in parents)
            {
                IWeapon[] weapons = parent.GetComponents<IWeapon>();
                ParticleWeaponMarker[] markers = parent.GetComponentsInChildren<ParticleWeaponMarker>(true);

                if (weapons.Length != markers.Length)
                {
                    Debug.LogError($"Mismatch in {parent.name}: {weapons.Length} weapons vs {markers.Length} markers");
                    continue;
                }

                for (int i = 0; i < weapons.Length; i++)
                {
                    ParticleSystem ps = markers[i].GetComponent<ParticleSystem>();
                    if (ps != null && weapons[i] is MonoBehaviour weaponBehaviour)
                    {
                        newPairs.Add(new WeaponParticlePair(ps, weaponBehaviour));
                    }
                }
            }
        }

        if (newPairs.Count == 0)
        {
            Debug.Log("No valid forbidden weapon pairs found");
            return;
        }

        UpdateLinker(linker, newPairs, merge: true);
        Debug.Log($"Successfully added {newPairs.Count} forbidden weapon pairs");
    }

    private static void UpdateLinker(ParticleWeaponLinker linker, List<WeaponParticlePair> newPairs, bool merge = false)
    {
        Undo.RecordObject(linker, "Update Forbidden Weapon Links");
        SerializedObject so = new SerializedObject(linker);
        SerializedProperty arrayProp = so.FindProperty("_linkedPairs");

        if (!merge) arrayProp.ClearArray();

        int startIndex = arrayProp.arraySize;
        arrayProp.arraySize += newPairs.Count;

        for (int i = 0; i < newPairs.Count; i++)
        {
            SerializedProperty element = arrayProp.GetArrayElementAtIndex(startIndex + i);
            element.FindPropertyRelative("_particle").objectReferenceValue = newPairs[i].ParticleSystem;
            element.FindPropertyRelative("_iWeapon").objectReferenceValue = newPairs[i].Weapon as MonoBehaviour;
        }

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(linker);
    }
}
#endif