#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using ModestTree;

[CustomEditor(typeof(CheckpointCircuitBinder))]
public class CheckpointCircuitBinderEditor : Editor
{
    private GUIStyle _headerStyle, _buttonStyle;
    private Texture2D _headerTexture, _buttonTexture;
    private string[] _rainbowTextColors;
    private bool _stylesInitialized;
    private float _lastRepaintTime;
    private readonly float _animationSpeed = 2.5f;

    private readonly Color[] _buttonGradient = {
        new(1.0f, 0.2f, 0.3f), new(1.0f, 0.6f, 0.0f),
        new(1.0f, 0.9f, 0.0f), new(0.0f, 0.8f, 0.2f),
        new(0.0f, 0.6f, 1.0f), new(0.5f, 0.2f, 1.0f)
    };

    private void OnEnable() => EditorApplication.update += EditorUpdate;
    private void OnDisable() => EditorApplication.update -= EditorUpdate;

    private void EditorUpdate()
    {
        if ((float)EditorApplication.timeSinceStartup - _lastRepaintTime <= 0.03f) return;
        _lastRepaintTime = (float)EditorApplication.timeSinceStartup;
        Repaint();
    }

    private void InitializeStyles()
    {
        if (_stylesInitialized) return;

        _headerStyle = new GUIStyle(EditorStyles.helpBox)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 16,
            richText = true,
            padding = new RectOffset(10, 10, 12, 12),
            margin = new RectOffset(0, 0, 10, 10)
        };

        _buttonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 14,
            padding = new RectOffset(10, 10, 8, 8),
            margin = new RectOffset(30, 30, 5, 15)
        };

        _headerTexture = CreateGradientTexture(new Color(0.1f, 0.4f, 0.1f), new Color(0.2f, 0.8f, 0.2f));
        _headerStyle.normal.background = _headerTexture;

        _rainbowTextColors = new string[60];
        for (var i = 0; i < _rainbowTextColors.Length; i++)
            _rainbowTextColors[i] = ColorUtility.ToHtmlStringRGB(GetRainbowColor((float)i / _rainbowTextColors.Length));

        _stylesInitialized = true;
    }

    public override void OnInspectorGUI()
    {
        InitializeStyles();
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "_checkpointsData");
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10);
        GUILayout.Box(GetAnimatedHeaderText(), _headerStyle);

        if (GUILayout.Button("Set checkpoints", _buttonStyle, GUILayout.Height(40)))
        {
            var manager = (CheckpointCircuitBinder)target;
            var tracker = manager.GetRaceProgressTracker() ?? FindObjectOfType<RaceProgressTracker>();
            if (tracker) manager.SetRaceProgressTracker(tracker);

            var checkpoints = manager.GetCheckpointTransforms();
            for (int i = 0; i < checkpoints.Length; i++)
                SetupCheckpoint(checkpoints[i], i, tracker);

            Debug.Log($"Configured {checkpoints.Length} checkpoints");
        }
    }

    private string GetAnimatedHeaderText()
    {
        var headerText = "Checkpoint Circuit Configurator";
        var result = "";
        var timeOffset = (float)EditorApplication.timeSinceStartup * _animationSpeed;

        for (int i = 0; i < headerText.Length; i++)
        {
            var colorIndex = (int)((timeOffset + i * 0.5f) * 5) % _rainbowTextColors.Length;
            result += $"<color=#{_rainbowTextColors[colorIndex]}>{headerText[i]}</color>";
        }

        return result;
    }

    private static void SetupCheckpoint(Transform checkpoint, int index, RaceProgressTracker tracker)
    {
        var collider = checkpoint.GetComponent<Collider>() ?? checkpoint.gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        if (checkpoint.TryGetComponent<MeshRenderer>(out var renderer)) renderer.enabled = false;

        var trigger = checkpoint.GetComponent<CheckpointTrigger>() ?? checkpoint.gameObject.AddComponent<CheckpointTrigger>();
        var so = new SerializedObject(trigger);
        so.FindProperty("_checkpointIndex").intValue = index;
        if (tracker) so.FindProperty("_raceProgressTracker").objectReferenceValue = tracker;
        so.ApplyModifiedProperties();
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