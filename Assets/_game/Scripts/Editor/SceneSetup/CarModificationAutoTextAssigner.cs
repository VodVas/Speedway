#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TMPro;
using YG;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CarModificationAutoTextAssigner : EditorWindow
{
    private const string NameSuffix = "[Name]";
    private static readonly Regex _nameMatcher = new Regex(@"\b(Speed|Armor|Acc|Turn|Color)\b", RegexOptions.IgnoreCase);

    private static readonly Dictionary<string, (string ru, string en, string tr)> _translations = new()
    {
        { "Speed", ("Скорость", "Speed", "Hız") },
        { "Armor", ("Броня", "Armor", "Zırh") },
        { "Acc", ("Ускорение", "Acceleration", "İvme") },
        { "Turn", ("Маневры", "Maneuver", "Manevra") },
        { "Color", ("Цвет", "Color", "Renk") }
    };

    [MenuItem("Tools/Apocalypse/Car Modifications TMP AutoSetup")]
    private static void Init() => GetWindow<CarModificationAutoTextAssigner>().Show();

    private void OnGUI()
    {
        if (GUILayout.Button("Process All Modifications", GUILayout.Height(40)))
            ProcessSceneModifications();
    }

    private void ProcessSceneModifications()
    {
        try
        {
            var targets = FindAllTargetObjects();
            if (targets.Count == 0)
            {
                Debug.LogWarning("No modification objects found in scene");
                return;
            }

            foreach (var target in targets)
            {
                ProcessSingleObject(target);
                EditorUtility.SetDirty(target);
            }

            Debug.Log($"Successfully processed {targets.Count} objects");
            AssetDatabase.SaveAssets();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Processing failed: {e.Message}");
        }
    }

    private List<GameObject> FindAllTargetObjects()
    {
        var result = new List<GameObject>();
        var sceneObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (var go in sceneObjects)
        {
            if (EditorUtility.IsPersistent(go)) continue;
            if ((go.hideFlags & HideFlags.HideInHierarchy) != 0) continue;

            var match = _nameMatcher.Match(go.name);
            if (match.Success && !result.Contains(go))
            {
                result.Add(go);
            }
        }

        return result;
    }

    private void ProcessSingleObject(GameObject target)
    {
        var nameChild = target.transform.Find(NameSuffix);
        var tmp = nameChild != null ?
            GetOrCreateTMP(nameChild.gameObject) :
            CreateNewNameChild(target.transform);

        ApplyTranslations(tmp.gameObject, target.name);
        LinkToModificationComponent(target, tmp);
    }

    private TextMeshProUGUI CreateNewNameChild(Transform parent)
    {
        var nameGO = new GameObject(NameSuffix, typeof(RectTransform));
        nameGO.transform.SetParent(parent, false);
        nameGO.transform.localPosition = Vector3.zero;
        nameGO.transform.localRotation = Quaternion.identity;
        nameGO.transform.localScale = Vector3.one;
        var tmp = nameGO.AddComponent<TextMeshProUGUI>();
        tmp.raycastTarget = false;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 24;

        nameGO.AddComponent<LanguageYG>();
        return tmp;
    }

    private TextMeshProUGUI GetOrCreateTMP(GameObject target)
    {
        if (!target.TryGetComponent<TextMeshProUGUI>(out var tmp))
        {
            tmp = target.AddComponent<TextMeshProUGUI>();
        }
        return tmp;
    }

    private void ApplyTranslations(GameObject target, string sourceName)
    {
        var match = _nameMatcher.Match(sourceName);
        if (!match.Success) return;

        var key = match.Groups[1].Value;
        if (!_translations.TryGetValue(key, out var texts)) return;

        var langYG = target.GetComponent<LanguageYG>();
        var so = new SerializedObject(langYG);
        so.FindProperty("ru").stringValue = texts.ru;
        so.FindProperty("en").stringValue = texts.en;
        so.FindProperty("tr").stringValue = texts.tr;
        so.ApplyModifiedProperties();
    }

    private void LinkToModificationComponent(GameObject target, TextMeshProUGUI tmp)
    {
        var modification = target.GetComponent<BaseCarModification>();
        if (modification == null)
        {
            Debug.LogWarning($"Missing BaseCarModification on {target.name}", target);
            return;
        }

        var so = new SerializedObject(modification);
        so.FindProperty("_name").objectReferenceValue = tmp;
        so.ApplyModifiedProperties();
    }
}
#endif