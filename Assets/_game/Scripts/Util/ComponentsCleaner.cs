using ArcadeVP;
using UnityEngine;

public class ComponentsCleaner : MonoBehaviour
{
    [SerializeField] private bool _isWeaponNeedDestroy = false;

    public void RemoveAllPhysicsComponents(GameObject rootObject)
    {
        if (ValidateRootObject(rootObject) == false) return;

        if (_isWeaponNeedDestroy)
        {
            DestroyAllWeapons(rootObject);
        }

        DestroyAllParticleSystems(rootObject);
        DisableAllAudioSources(rootObject);
        DisableAllSkidMarks(rootObject);
        DestroyAllJoints(rootObject);
        DestroyAllColliders(rootObject);
        DestroyAllRigidbodies(rootObject);
    }

    private bool ValidateRootObject(GameObject rootObject)
    {
        if (rootObject != null) return true;

        Debug.LogError("[CarPhysicsCleaner] Root object is null!", this);
        enabled = false;
        return false;
    }

    private void DestroyAllParticleSystems(GameObject root)
    {
        foreach (var system in root.GetComponentsInChildren<ParticleSystem>(true))
        {
            if (system != null) Destroy(system);
        }
    }

    private void DisableAllAudioSources(GameObject root)
    {
        foreach (var audioSource in root.GetComponentsInChildren<AudioSource>(true))
        {
            if (audioSource != null) audioSource.enabled = false;
        }
    }

    private void DisableAllSkidMarks(GameObject root)
    {
        foreach (var skidMark in root.GetComponentsInChildren<SkidMarks>(true))
        {
            if (skidMark != null) skidMark.enabled = false;
        }
    }

    private void DestroyAllJoints(GameObject root)
    {
        foreach (var joint in root.GetComponentsInChildren<Joint>(true))
        {
            if (joint != null) Destroy(joint);
        }
    }

    private void DestroyAllColliders(GameObject root)
    {
        foreach (var collider in root.GetComponentsInChildren<Collider>(true))
        {
            if (collider != null) Destroy(collider);
        }
    }

    private void DestroyAllRigidbodies(GameObject root)
    {
        foreach (var rb in root.GetComponentsInChildren<Rigidbody>(true))
        {
            if (rb != null) Destroy(rb);
        }
    }

    private void DestroyAllWeapons(GameObject root)
    {
        Weapon[] weapons = root.GetComponentsInChildren<Weapon>(true);

        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        Resources.UnloadUnusedAssets();
    }
}