#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using ArcadeVP;

public class RespawnPointsSetter : EditorWindow
{
    private Transform _pointsContainer;
    private List<Transform> _respawnTransforms = new List<Transform>();
    private Vector2 _scrollPosition;
    private GUIStyle _headerStyle, _compactStyle;
    private Texture2D _headerTexture;
    private string[] _rainbowTextColors;
    private bool _stylesInitialized;
    private float _lastRepaintTime;
    private readonly float _animationSpeed = 2.5f;
    private string _searchFilter = "";
    private bool _sortByName = true;

    [MenuItem("Tools/Apocalypse/Set Respawn Points")]
    public static void ShowWindow()
    {
        GetWindow<RespawnPointsSetter>("Apocalypse Tools").minSize = new Vector2(400, 500);
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

        _compactStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            alignment = TextAnchor.MiddleLeft,
            clipping = TextClipping.Clip,
            padding = new RectOffset(5, 5, 2, 2)
        };

        _headerTexture = CreateGradientTexture(
            new Color(0.4f, 0.1f, 0.1f),
            new Color(0.8f, 0.2f, 0.2f)
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
        DrawMassImportSection();
        DrawListControls();
        DrawPointsList();
        DrawActions();
    }

    private void DrawHeader()
    {
        var headerText = GetAnimatedHeaderText("Apocalypse Tools");
        GUILayout.Label(headerText, _headerStyle);
        EditorGUILayout.Space(10);
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

    private void DrawMassImportSection()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Mass Import Options", EditorStyles.boldLabel);

        var rect = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(rect, "Drag Transforms or Parent Container Here");

        if (rect.Contains(Event.current.mousePosition))
        {
            HandleDragAndDrop();
        }

        EditorGUILayout.BeginHorizontal();
        _pointsContainer = (Transform)EditorGUILayout.ObjectField(
            "Parent Container",
            _pointsContainer,
            typeof(Transform),
            true
        );

        if (GUILayout.Button("Load Children", GUILayout.Width(100)))
        {
            LoadFromContainer();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);
    }

    private void HandleDragAndDrop()
    {
        if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                AddTransforms(DragAndDrop.objectReferences.OfType<GameObject>()
                    .Select(g => g.transform)
                    .Where(t => t != null));
            }
            Event.current.Use();
        }
    }

    private void LoadFromContainer()
    {
        if (_pointsContainer == null) return;

        var children = new List<Transform>();
        foreach (Transform child in _pointsContainer)
        {
            children.Add(child);
        }
        AddTransforms(children);
    }

    private void AddTransforms(IEnumerable<Transform> transforms)
    {
        _respawnTransforms.AddRange(transforms
            .Where(t => !_respawnTransforms.Contains(t)));

        if (_sortByName)
        {
            _respawnTransforms = _respawnTransforms
                .OrderBy(t => t.name)
                .ToList();
        }
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

    private void DrawListControls()
    {
        EditorGUILayout.BeginHorizontal();

        _searchFilter = EditorGUILayout.TextField("Search:", _searchFilter, GUILayout.Width(250));

        EditorGUILayout.LabelField("Sort:", GUILayout.Width(40));
        _sortByName = GUILayout.Toggle(_sortByName, "By Name", EditorStyles.miniButton, GUILayout.Width(80));
        if (GUILayout.Button("Clear List", EditorStyles.miniButton, GUILayout.Width(80)))
        {
            _respawnTransforms.Clear();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(5);
    }

    private void DrawPointsList()
    {
        EditorGUILayout.LabelField($"Respawn Points ({_respawnTransforms.Count})", EditorStyles.boldLabel);
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));

        var filtered = _respawnTransforms
            .Where(t => t != null && t.name.ToLower().Contains(_searchFilter.ToLower()))
            .ToList();

        for (int i = 0; i < filtered.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{i + 1}.", GUILayout.Width(40));
            EditorGUILayout.ObjectField(filtered[i], typeof(Transform), true, GUILayout.Height(20));

            if (GUILayout.Button("×", _compactStyle, GUILayout.Width(20)))
            {
                _respawnTransforms.Remove(filtered[i]);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space(10);
    }

    private void DrawActions()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Set Respawn Points", GUILayout.Height(40)))
        {
            if (_respawnTransforms.Count == 0)
            {
                EditorUtility.DisplayDialog("Error", "No respawn points assigned!", "OK");
                return;
            }

            SetRespawnPoints();
        }

        if (GUILayout.Button("Validate Points", GUILayout.Height(40)))
        {
            _respawnTransforms = _respawnTransforms
                .Where(t => t != null)
                .Distinct()
                .ToList();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void SetRespawnPoints()
    {
        var controllers = FindAllControllers();
        if (controllers.Count == 0)
        {
            EditorUtility.DisplayDialog("Warning", "No vehicles found!", "OK");
            return;
        }

        Undo.SetCurrentGroupName("Update Respawn Points");
        var groupIndex = Undo.GetCurrentGroup();

        foreach (var controller in controllers)
        {
            Undo.RecordObject(controller, "Respawn Points");

            var so = new SerializedObject(controller);
            var points = so.FindProperty("_respawnPoints");

            points.arraySize = _respawnTransforms.Count;
            for (int i = 0; i < _respawnTransforms.Count; i++)
            {
                points.GetArrayElementAtIndex(i).objectReferenceValue = _respawnTransforms[i];
            }

            so.ApplyModifiedProperties();
        }

        Undo.CollapseUndoOperations(groupIndex);
        Debug.Log($"Updated {controllers.Count} controllers with {_respawnTransforms.Count} points");
    }

    private List<ArcadeVehicleController> FindAllControllers()
    {
        return Resources.FindObjectsOfTypeAll<ArcadeVehicleController>()
            .Where(c => !EditorUtility.IsPersistent(c))
            .ToList();
    }
}
#endif