#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TMPro;
using YG;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class CarUpgradeEditorTool : EditorWindow
{
    private const string NameChild = "[Name]";
    private const string DescriptionChild = "[Description]";

    private static readonly Regex _cleanRegex = new Regex(@"[\W\d]", RegexOptions.Compiled);
    private static List<string> _sortedKeys;

    private static readonly Dictionary<string, (string ru, string en, string tr)> _nameTranslations = new()
    {
        { "Light_Machinegun", ("Лёгкий пулемёт", "Light Machinegun", "Hafif Makineli") },
        { "MiniGun", ("Миниган", "Minigun", "Mini Top") },
        { "AntiaircraftMachinegun", ("Зенитка", "AA Gun", "Uçaksavar") },
        { "Middle_MachineGun", ("Средний пулемёт", "Medium MG", "Orta Makineli") },
        { "Rocket_Launcher", ("РПГ", "RPG", "Roketatar") },
        { "SpikesSpawner", ("Шипы", "Spikes", "Dikenler") },
        { "PlayerMine", ("Мины", "Mines", "Mayın") },

        { "Armour", ("Броня", "Armor", "Zırh") },
        { "Engine", ("Двигатель", "Engine", "Motor") },
        { "Glass_Armour", ("Броня на стекло", "Armor Glass", "Cam Zırh") }
    };

    [InitializeOnLoadMethod]
    private static void InitStatic()
    {
        _sortedKeys = _nameTranslations.Keys
            .OrderByDescending(k => k.Length)
            .ToList();
    }

    [MenuItem("Tools/Apocalypse/Car Upgrade TMP AutoSetup")]
    private static void Init() => GetWindow<CarUpgradeEditorTool>().Show();

    private void OnGUI()
    {
        if (GUILayout.Button("Process Scene", GUILayout.Height(40)))
            ProcessEntireScene();
    }

    private void ProcessEntireScene()
    {
        try
        {
            var upgrades = FindObjectsByType<CarUpgrade>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (var upgrade in upgrades)
                ProcessSingleUpgrade(upgrade);

            Debug.Log($"Processed {upgrades.Length} upgrades");
            AssetDatabase.SaveAssets();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Processing error: {e.Message}");
        }
    }

    private void ProcessSingleUpgrade(CarUpgrade upgrade)
    {
        var transform = upgrade.transform;

        var nameTMP = CreateOrGetTMPChild(transform, NameChild);
        var descTMP = CreateOrGetTMPChild(transform, DescriptionChild);

        BindTMPComponentsToParent(upgrade, nameTMP, descTMP);
        ProcessNameComponent(nameTMP.gameObject, upgrade);
        ProcessDescriptionComponent(descTMP.gameObject, upgrade);

        EditorUtility.SetDirty(upgrade);
    }

    private void BindTMPComponentsToParent(CarUpgrade upgrade, TextMeshProUGUI nameTMP, TextMeshProUGUI descTMP)
    {
        var so = new SerializedObject(upgrade);
        so.FindProperty("_upgradeName").objectReferenceValue = nameTMP;
        so.FindProperty("_upgradeDescription").objectReferenceValue = descTMP;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private TextMeshProUGUI CreateOrGetTMPChild(Transform parent, string name)
    {
        var child = parent.Find(name) ?? CreateChild(parent, name);
        return GetOrAddTMPComponent(child.gameObject);
    }

    private Transform CreateChild(Transform parent, string name)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        return go.transform;
    }

    private TextMeshProUGUI GetOrAddTMPComponent(GameObject target)
    {
        if (!target.TryGetComponent(out TextMeshProUGUI tmp))
        {
            tmp = target.AddComponent<TextMeshProUGUI>();
            tmp.raycastTarget = false;
            tmp.alignment = TextAlignmentOptions.Center;
        }
        return tmp;
    }

    private void ProcessNameComponent(GameObject target, CarUpgrade upgrade)
    {
        if (!target.TryGetComponent(out LanguageYG lang))
            lang = target.AddComponent<LanguageYG>();

        var key = ExtractLocalizationKey(upgrade.gameObject.name);
        ApplyTranslations(lang, key);
    }

    private string ExtractLocalizationKey(string originalName)
    {
        var cleanName = _cleanRegex.Replace(originalName, "");
        foreach (var key in _sortedKeys)
        {
            if (cleanName.Contains(key))
                return key;
        }
        return "Unknown";
    }

    private void ApplyTranslations(LanguageYG lang, string key)
    {
        if (_nameTranslations.TryGetValue(key, out var texts))
        {
            var so = new SerializedObject(lang);
            so.FindProperty("ru").stringValue = texts.ru;
            so.FindProperty("en").stringValue = texts.en;
            so.FindProperty("tr").stringValue = texts.tr;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
        else
        {
            Debug.LogWarning($"Missing translation for key: {key}", lang.gameObject);
        }
    }

    private void ProcessDescriptionComponent(GameObject target, CarUpgrade upgrade)
    {
        if (!target.TryGetComponent(out LanguageYG lang))
            lang = target.AddComponent<LanguageYG>();

        var so = new SerializedObject(lang);
        string valueText = $"{Mathf.RoundToInt(upgrade.UpgradeValue)}";

        switch (upgrade.UpgradeType)
        {
            case CarUpgrade.CarUpgradeType.Weapon:
                so.FindProperty("ru").stringValue = $"Урон {valueText}";
                so.FindProperty("en").stringValue = $"Damage {valueText}";
                so.FindProperty("tr").stringValue = $"Hasar {valueText}";
                break;

            case CarUpgrade.CarUpgradeType.Speed:
                so.FindProperty("ru").stringValue = $"Скорость {valueText}";
                so.FindProperty("en").stringValue = $"Speed {valueText}";
                so.FindProperty("tr").stringValue = $"Hız {valueText}";
                break;

            case CarUpgrade.CarUpgradeType.Acceleration:
                so.FindProperty("ru").stringValue = $"Ускорение {valueText}";
                so.FindProperty("en").stringValue = $"Accel {valueText}";
                so.FindProperty("tr").stringValue = $"İvme {valueText}";
                break;

            case CarUpgrade.CarUpgradeType.Turn:
                so.FindProperty("ru").stringValue = $"Поворот {valueText}";
                so.FindProperty("en").stringValue = $"Turn {valueText}";
                so.FindProperty("tr").stringValue = $"Dönüş {valueText}";
                break;

            case CarUpgrade.CarUpgradeType.Health:
                so.FindProperty("ru").stringValue = $"Броня {valueText}";
                so.FindProperty("en").stringValue = $"Armor {valueText}";
                so.FindProperty("tr").stringValue = $"Zırh {valueText}";
                break;

            default:
                Debug.LogWarning($"Unknown upgrade type: {upgrade.UpgradeType}", target);
                so.FindProperty("ru").stringValue = $"Эффект {valueText}";
                so.FindProperty("en").stringValue = $"Effect {valueText}";
                so.FindProperty("tr").stringValue = $"Etki {valueText}";
                break;
        }

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(lang);
    }
}
#endif