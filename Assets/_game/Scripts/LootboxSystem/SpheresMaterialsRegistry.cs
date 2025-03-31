using UnityEngine;

public class SpheresMaterialsRegistry : MonoBehaviour
{
    [Header("Materials by Rarity")]
    [SerializeField] private Material[] _commonMaterials;
    [SerializeField] private Material[] _rareMaterials;
    [SerializeField] private Material[] _uniqueMaterials;
    [SerializeField] private Material[] _legendaryMaterials;
    [SerializeField] private Material[] _epicMaterials;

    public Material GetRandomMaterial(Rarity rarity)
    {
        Material[] arrayToUse = null;

        switch (rarity)
        {
            case Rarity.Common:
                arrayToUse = _commonMaterials;
                break;
            case Rarity.Rare:
                arrayToUse = _rareMaterials;
                break;
            case Rarity.Unique:
                arrayToUse = _uniqueMaterials;
                break;
            case Rarity.Legendary:
                arrayToUse = _legendaryMaterials;
                break;
            case Rarity.Epic:
                arrayToUse = _epicMaterials;
                break;
        }

        if (arrayToUse == null || arrayToUse.Length == 0)
        {
            Debug.LogWarning($"[SpheresMaterialsRegistry] No materials array for rarity {rarity}");
            return null;
        }

        int randomIndex = Random.Range(0, arrayToUse.Length);
        return arrayToUse[randomIndex];
    }
}