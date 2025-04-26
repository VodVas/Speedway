#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MaterialReplacerEditor : EditorWindow
{
    private PhysicMaterial _targetMaterial;
    private GUIStyle _headerStyle;
    private Texture2D _headerTexture;
    private string[] _rainbowTextColors;
    private bool _stylesInitialized;
    private float _lastRepaintTime;
    private readonly float _animationSpeed = 2.5f;

    [MenuItem("Tools/Apocalypse/Material Replacer")]
    public static void ShowWindow()
    {
        GetWindow<MaterialReplacerEditor>("Apocalypse Tools");
    }

    private void OnEnable() => EditorApplication.update += EditorUpdate;
    private void OnDisable() => EditorApplication.update -= EditorUpdate;

    private void EditorUpdate()
    {
        if (Time.realtimeSinceStartup - _lastRepaintTime <= 0.03f) return;
        _lastRepaintTime = Time.realtimeSinceStartup;
        Repaint();
    }

    private void InitializeStyles()
    {
        if (_stylesInitialized) return;

        _headerStyle = new GUIStyle(EditorStyles.helpBox)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 20,
            richText = true,
            padding = new RectOffset(15, 15, 12, 12),
            margin = new RectOffset(0, 0, 15, 15)
        };

        _headerTexture = CreateGradientTexture(
            new Color(0.1f, 0.1f, 0.4f),
            new Color(0.2f, 0.2f, 0.8f)
        );
        _headerStyle.normal.background = _headerTexture;

        _rainbowTextColors = new string[60];
        for (var i = 0; i < _rainbowTextColors.Length; i++)
            _rainbowTextColors[i] = ColorUtility.ToHtmlStringRGB(GetRainbowColor((float)i / _rainbowTextColors.Length));

        _stylesInitialized = true;
    }

    private void OnGUI()
    {
        InitializeStyles();
        DrawHeader();
        DrawMaterialSelector();
        DrawReplaceButton();
    }

    private void DrawHeader()
    {
        var headerText = GetAnimatedHeaderText("Apocalypse Tools");
        GUILayout.Label(headerText, _headerStyle);
        EditorGUILayout.Space(20);
    }

    private void DrawMaterialSelector()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Physics Material:", GUILayout.Width(120));
        _targetMaterial = (PhysicMaterial)EditorGUILayout.ObjectField(
            _targetMaterial,
            typeof(PhysicMaterial),
            false
        );
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(15);
    }

    private void DrawReplaceButton()
    {
        if (GUILayout.Button("Replace Materials", GUILayout.Height(40)))
        {
            if (_targetMaterial == null)
            {
                Debug.LogError("Please assign a Physics Material first!");
                return;
            }

            ReplaceMaterials();
        }
    }

    private void ReplaceMaterials()
    {
        var sphereObjects = new List<GameObject>();
        var sceneObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (var obj in sceneObjects)
        {
            if (obj.name == "SphereRB" &&
                obj.GetComponent<SphereCollider>() != null &&
                !EditorUtility.IsPersistent(obj))
            {
                sphereObjects.Add(obj);
            }
        }

        if (sphereObjects.Count == 0)
        {
            Debug.LogWarning("No objects named 'SphereRB' with SphereCollider found!");
            return;
        }

        foreach (var obj in sphereObjects)
        {
            var collider = obj.GetComponent<SphereCollider>();
            Undo.RecordObject(collider, "Change Physics Material");
            collider.material = _targetMaterial;
        }

        Debug.Log($"Updated {sphereObjects.Count} SphereRB objects");
    }

    private string GetAnimatedHeaderText(string text)
    {
        var result = "";
        var timeOffset = (float)EditorApplication.timeSinceStartup * _animationSpeed;

        for (int i = 0; i < text.Length; i++)
        {
            var colorIndex = (int)((timeOffset + i * 0.5f) * 5) % _rainbowTextColors.Length;
            result += $"<color=#{_rainbowTextColors[colorIndex]}>{text[i]}</color>";
        }
        return result;
    }

    private Texture2D CreateGradientTexture(Color top, Color bottom)
    {
        var texture = new Texture2D(1, 64) { hideFlags = HideFlags.HideAndDontSave };
        var pixels = new Color[64];
        for (var y = 0; y < 64; y++)
            pixels[y] = Color.Lerp(top, bottom, (float)y / 64);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private static Color GetRainbowColor(float t) => new(
        Mathf.Sin(t * 2 * Mathf.PI) * 0.5f + 0.5f,
        Mathf.Sin((t + 0.33f) * 2 * Mathf.PI) * 0.5f + 0.5f,
        Mathf.Sin((t + 0.67f) * 2 * Mathf.PI) * 0.5f + 0.5f
    );
}
#endif