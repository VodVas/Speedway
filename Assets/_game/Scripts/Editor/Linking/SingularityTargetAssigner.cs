#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public sealed class SingularityTargetAssigner : EditorWindow
{
    private static class Styles
    {
        public static readonly GUIStyle HeaderStyle;
        public static readonly Texture2D HeaderTexture;
        public static readonly Color[] RainbowColors = {
            new(1,0,0), new(1,0.5f,0), new(1,1,0),
            new(0,1,0), new(0,1,1), new(0,0,1), new(0.5f,0,1)
        };

        static Styles()
        {
            HeaderTexture = CreateGradientTexture(new Color(0.15f, 0.15f, 0.45f), new Color(0.25f, 0.25f, 0.85f));
            HeaderStyle = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 22,
                fontStyle = FontStyle.Bold,
                richText = true,
                padding = new RectOffset(20, 20, 15, 15),
                normal = { background = HeaderTexture }
            };
        }

        private static Texture2D CreateGradientTexture(Color top, Color bottom)
        {
            var tex = new Texture2D(1, 64) { hideFlags = HideFlags.HideAndDontSave };
            var pixels = new Color[64];
            for (int y = 0; y < 64; y++)
                pixels[y] = Color.Lerp(top, bottom, y / 63f);
            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }
    }

    private Transform _rootParent;
    private Singularity _targetSingularity;
    private Vector2 _scrollPos;
    private readonly List<Rigidbody> _foundBodies = new();

    [MenuItem("Tools/Apocalypse/Assign Singularity Targets")]
    public static void ShowWindow() => GetWindow<SingularityTargetAssigner>("Singularity Setup");

    private void OnGUI()
    {
        DrawHeader();
        DrawSetupUI();
    }

    private void DrawHeader()
    {
        var animatedHeader = BuildAnimatedHeader("Apocalypse Tools");
        GUILayout.Label(animatedHeader, Styles.HeaderStyle);
        EditorGUILayout.Space(10);
    }

    private string BuildAnimatedHeader(string text)
    {
        var time = EditorApplication.timeSinceStartup * 2.0;
        var result = "";
        for (int i = 0; i < text.Length; i++)
        {
            var colorIndex = (int)((time + i * 0.3) * 2) % Styles.RainbowColors.Length;
            result += $"<color=#{ColorUtility.ToHtmlStringRGB(Styles.RainbowColors[colorIndex])}>{text[i]}</color>";
        }
        return result;
    }

    private void DrawSetupUI()
    {
        using (var scroll = new EditorGUILayout.ScrollViewScope(_scrollPos))
        {
            _scrollPos = scroll.scrollPosition;

            EditorGUI.BeginChangeCheck();
            _targetSingularity = (Singularity)EditorGUILayout.ObjectField("Target Singularity", _targetSingularity, typeof(Singularity), true);
            _rootParent = EditorGUILayout.ObjectField("Root Object", _rootParent, typeof(Transform), true) as Transform;

            if (EditorGUI.EndChangeCheck())
                RefreshComponents();

            EditorGUILayout.Space(15);

            using (new EditorGUI.DisabledScope(!IsReadyForAssign()))
            {
                if (GUILayout.Button("Auto Assign Targets", GUILayout.Height(40)))
                    ProcessHierarchy();
            }

            if (_foundBodies.Count > 0)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField($"Found {_foundBodies.Count} Rigidbodies", EditorStyles.centeredGreyMiniLabel);
            }
        }
    }

    private bool IsReadyForAssign() =>
    _targetSingularity != null &&
    _rootParent != null &&
    _rootParent.childCount > 0 &&
    _foundBodies.Count > 0;

    private void RefreshComponents()
    {
        _foundBodies.Clear();

        if (_rootParent == null) return;

        foreach (Transform child in _rootParent)
        {
            if (child.TryGetComponent<Rigidbody>(out var rb) && !rb.isKinematic)
            {
                _foundBodies.Add(rb);
            }
        }
    }

    private void FindAllRigidbodies(Transform root, List<Rigidbody> output)
    {
        output.Clear();

        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out var rb) && !rb.isKinematic)
            {
                output.Add(rb);
            }
        }
    }

    private void ProcessHierarchy()
    {
        if (!IsReadyForAssign()) return;

        Undo.RecordObject(_targetSingularity, "Update Singularity Targets");

        var existingTargets = new List<PullTarget>(_targetSingularity.GetTargets());
        var existingHashes = new HashSet<int>(existingTargets.Count);

        foreach (var target in existingTargets)
        {
            if (target?.GetRigidbody() == null) continue;
            existingHashes.Add(target.GetRigidbody().GetInstanceID());
        }

        var newEntries = 0;
        foreach (var rb in _foundBodies)
        {
            if (rb == null) continue;

            if (existingHashes.Contains(rb.GetInstanceID())) continue;

            var pt = new PullTarget();
            pt.SetRigidbody(rb);
            pt.SetPullable(true);
            pt.SetCachedMass(rb.mass);
            existingTargets.Add(pt);
            newEntries++;
        }

        if (newEntries == 0)
        {
            Debug.Log("No new Rigidbodies found to add");
            return;
        }

        _targetSingularity.SetTargets(existingTargets.ToArray());
        EditorUtility.SetDirty(_targetSingularity);
        Debug.Log($"Added {newEntries} new targets. Total: {existingTargets.Count}");
    }

    private void OnInspectorUpdate() => Repaint();
}
#endif