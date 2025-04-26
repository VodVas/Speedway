#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class ShadowDisablerEditor : EditorWindow
{
    [SerializeField] private List<GameObject> targetObjects = new List<GameObject>();
    private SerializedObject serializedObject;
    private SerializedProperty objectsProperty;
    private Vector2 scrollPosition;
    private GUIStyle headerStyle, buttonStyle;
    private Texture2D headerTexture;
    private string[] rainbowColors;
    private bool stylesInitialized;
    private float lastRepaintTime;
    private const float ANIM_SPEED = 2.5f;
    private string searchFilter = "";
    private bool sortByName = true;

    [MenuItem("Tools/Apocalypse/Shadow Disabler")]
    public static void ShowWindow() => GetWindow<ShadowDisablerEditor>("Shadow Tools").minSize = new Vector2(400, 500);

    private void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
        serializedObject = new SerializedObject(this);
        objectsProperty = serializedObject.FindProperty("targetObjects");
    }

    private void OnDisable() => EditorApplication.update -= EditorUpdate;

    private void EditorUpdate()
    {
        if (Time.realtimeSinceStartup - lastRepaintTime <= 0.03f) return;
        lastRepaintTime = Time.realtimeSinceStartup;
        Repaint();
    }

    private void InitializeStyles()
    {
        if (stylesInitialized) return;

        headerStyle = new GUIStyle(EditorStyles.helpBox)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 20,
            richText = true,
            padding = new RectOffset(15, 15, 12, 12)
        };

        buttonStyle = new GUIStyle(EditorStyles.miniButton)
        {
            padding = new RectOffset(10, 10, 5, 5),
            margin = new RectOffset(2, 2, 2, 2)
        };

        headerTexture = CreateGradient(new Color(0.1f, 0.1f, 0.3f), new Color(0.3f, 0.3f, 0.6f));
        headerStyle.normal.background = headerTexture;

        rainbowColors = new string[60];
        for (int i = 0; i < rainbowColors.Length; i++)
            rainbowColors[i] = ColorUtility.ToHtmlStringRGB(CalculateRainbow((float)i / rainbowColors.Length));

        stylesInitialized = true;
    }

    private void OnGUI()
    {
        InitializeStyles();
        DrawHeader();
        DrawDragDropSection();
        DrawListControls();
        DrawObjectList();
        DrawActionButtons();
    }

    private void DrawHeader()
    {
        GUILayout.Label(GenerateAnimatedHeader("Shadow Tool"), headerStyle);
        EditorGUILayout.Space(10);
    }

    private Texture2D CreateGradient(Color start, Color end)
    {
        var tex = new Texture2D(1, 64) { hideFlags = HideFlags.HideAndDontSave };
        var cols = new Color[64];
        for (int i = 0; i < 64; i++) cols[i] = Color.Lerp(start, end, i / 64f);
        tex.SetPixels(cols);
        tex.Apply();
        return tex;
    }

    private static Color CalculateRainbow(float t) => new(
        Mathf.Sin(t * 2 * Mathf.PI) * 0.5f + 0.5f,
        Mathf.Sin((t + 0.33f) * 2 * Mathf.PI) * 0.5f + 0.5f,
        Mathf.Sin((t + 0.67f) * 2 * Mathf.PI) * 0.5f + 0.5f
    );

    private void DrawDragDropSection()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        var rect = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(rect, "Drag GameObjects Here");

        if (rect.Contains(Event.current.mousePosition) &&
           (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (Event.current.type == EventType.DragPerform)
            {
                AddUniqueObjects(DragAndDrop.objectReferences.OfType<GameObject>().Where(g => g));
                Event.current.Use();
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);
    }

    private void DrawListControls()
    {
        EditorGUILayout.BeginHorizontal();

        searchFilter = EditorGUILayout.TextField("Search:", searchFilter, GUILayout.Width(200));

        GUILayout.Space(5);
        sortByName = GUILayout.Toggle(sortByName, "Sort A-Z",
            GUI.skin.button,
            GUILayout.Width(80),
            GUILayout.Height(24));

        GUILayout.Space(5);
        if (GUILayout.Button("Clear List",
            GUI.skin.button,
            GUILayout.Width(80),
            GUILayout.Height(24)))
        {
            targetObjects.Clear();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawObjectList()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        var filtered = new List<GameObject>(targetObjects.Count);
        foreach (var go in targetObjects)
        {
            if (go && go.name.IndexOf(searchFilter, System.StringComparison.OrdinalIgnoreCase) >= 0)
                filtered.Add(go);
        }

        if (sortByName)
            filtered.Sort((a, b) => string.Compare(a.name, b.name, System.StringComparison.Ordinal));

        foreach (var go in filtered)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(go, typeof(GameObject), true);
            if (GUILayout.Button("×", GUILayout.Width(20)))
                targetObjects.Remove(go);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawActionButtons()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Process Objects", GUILayout.Height(40)))
            ProcessObjects();

        if (GUILayout.Button("Validate List", GUILayout.Height(40)))
            targetObjects = targetObjects.Where(g => g).Distinct().ToList();

        EditorGUILayout.EndHorizontal();
    }

    private string GenerateAnimatedHeader(string text)
    {
        var sb = new System.Text.StringBuilder();
        float time = (float)EditorApplication.timeSinceStartup * ANIM_SPEED;

        for (int i = 0; i < text.Length; i++)
        {
            int colorIndex = (int)((time + i * 0.5f) * 10) % rainbowColors.Length;
            sb.Append($"<color=#{rainbowColors[colorIndex]}>{text[i]}</color>");
        }
        return sb.ToString();
    }

    private void AddUniqueObjects(IEnumerable<GameObject> objects)
    {
        var unique = objects.Where(g => !targetObjects.Contains(g));
        targetObjects.AddRange(unique);
        if (sortByName) targetObjects = targetObjects.OrderBy(g => g.name).ToList();
    }

    private void ProcessObjects()
    {
        if (targetObjects.Count == 0)
        {
            Debug.LogWarning("No objects to process");
            return;
        }

        var renderers = new List<Renderer>(1024);
        foreach (var go in targetObjects.Where(g => g))
            renderers.AddRange(go.GetComponentsInChildren<Renderer>(true)
                .Where(r => r is MeshRenderer || r is SkinnedMeshRenderer));

        if (renderers.Count == 0)
        {
            Debug.LogWarning("No valid renderers found");
            return;
        }

        Undo.RecordObjects(renderers.Cast<Object>().ToArray(), "Disable Shadows");
        foreach (var r in renderers)
        {
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows = false;
        }

        Debug.Log($"Processed {renderers.Count} renderers");
    }
}
#endif