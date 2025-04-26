#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class UIOptimizerEditor : EditorWindow
{
    [SerializeField] private List<GameObject> excludedObjects = new List<GameObject>();
    [SerializeField] private bool processText = true;
    [SerializeField] private bool disableTextMaskable = true;
    [SerializeField] private bool disableTextRaycast = true;
    [SerializeField] private bool processImages = true;
    [SerializeField] private bool disableImageMaskable = true;
    [SerializeField] private bool disableImageRaycast = true;
    [SerializeField] private bool processPanels = true;
    [SerializeField] private bool disablePanelMaskable = true;
    [SerializeField] private bool disablePanelRaycast = true;
    [SerializeField] private bool includeInactive = true;
    [SerializeField] private bool excludeButtons = true;

    private SerializedObject serializedObject;
    private Vector2 scrollPosition;
    private GUIStyle headerStyle, sectionStyle;
    private Texture2D headerTexture;
    private string[] rainbowColors;
    private bool stylesInitialized;
    private float lastRepaintTime;
    private const float ANIM_SPEED = 2.5f;
    private string searchFilter = "";

    [MenuItem("Tools/Apocalypse/UI Optimizer Pro")]
    public static void ShowWindow()
    {
        GetWindow<UIOptimizerEditor>("UI Optimizer Pro").minSize = new Vector2(500, 600);
    }

    private void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
        serializedObject = new SerializedObject(this);
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

        sectionStyle = new GUIStyle(EditorStyles.helpBox)
        {
            padding = new RectOffset(10, 10, 10, 10),
            margin = new RectOffset(5, 5, 5, 5)
        };

        headerTexture = CreateGradient(new Color(0.25f, 0.1f, 0.4f), new Color(0.45f, 0.2f, 0.65f));
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
        DrawConfiguration();
        DrawDragDropSection();
        DrawExcludedList();
        DrawActionButtons();
    }

    private void DrawHeader()
    {
        GUILayout.Label(GenerateAnimatedHeader("UI Optimizer Pro"), headerStyle);
        EditorGUILayout.Space(15);
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

    private void DrawConfiguration()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical(sectionStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Search Options", EditorStyles.boldLabel, GUILayout.Width(100));
        includeInactive = EditorGUILayout.ToggleLeft("Include Inactive", includeInactive);
        excludeButtons = EditorGUILayout.ToggleLeft("Exclude Buttons", excludeButtons);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        DrawComponentSettings("TextMeshPro", ref processText, ref disableTextMaskable, ref disableTextRaycast);
        DrawComponentSettings("Images", ref processImages, ref disableImageMaskable, ref disableImageRaycast);
        DrawComponentSettings("Panels", ref processPanels, ref disablePanelMaskable, ref disablePanelRaycast);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);
    }

    private void DrawComponentSettings(string label, ref bool process, ref bool disableMask, ref bool disableRaycast)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(80));
        process = EditorGUILayout.ToggleLeft("Enable", process, GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        if (process)
        {
            EditorGUI.indentLevel++;
            disableMask = EditorGUILayout.Toggle("Disable Maskable", disableMask);
            disableRaycast = EditorGUILayout.Toggle("Disable Raycast", disableRaycast);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    private void DrawDragDropSection()
    {
        EditorGUILayout.BeginVertical(sectionStyle);

        var rect = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(rect, "Drag Objects To Exclude", EditorStyles.centeredGreyMiniLabel);

        if (rect.Contains(Event.current.mousePosition))
            HandleDragAndDrop();

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
                AddUniqueObjects(DragAndDrop.objectReferences.OfType<GameObject>().Where(g => g));
                Event.current.Use();
            }
        }
    }

    private void DrawExcludedList()
    {
        EditorGUILayout.BeginVertical(sectionStyle);

        EditorGUILayout.BeginHorizontal();
        searchFilter = EditorGUILayout.TextField("Search Excluded:", searchFilter, GUILayout.Width(200));
        if (GUILayout.Button("Clear List", GUILayout.Width(100)))
        {
            Undo.RecordObject(this, "Clear Excluded List");
            excludedObjects.Clear();
        }
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        var filtered = new List<GameObject>(excludedObjects.Count);
        foreach (var go in excludedObjects)
        {
            if (go && go.name.IndexOf(searchFilter, System.StringComparison.OrdinalIgnoreCase) >= 0)
                filtered.Add(go);
        }

        foreach (var go in filtered)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(go, typeof(GameObject), true);
            if (GUILayout.Button("×", GUILayout.Width(20)))
            {
                Undo.RecordObject(this, "Remove Excluded Object");
                excludedObjects.Remove(go);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawActionButtons()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Process Scene", GUILayout.Height(40)))
            ProcessScene();

        if (GUILayout.Button("Validate Exclusions", GUILayout.Height(40)))
        {
            Undo.RecordObject(this, "Validate Exclusions");
            excludedObjects = excludedObjects.Where(g => g).Distinct().ToList();
        }

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
        Undo.RecordObject(this, "Add Excluded Objects");
        foreach (var go in objects)
        {
            if (!excludedObjects.Contains(go))
                excludedObjects.Add(go);
        }
    }

    private void ProcessScene()
    {
        if (!processText && !processImages && !processPanels)
        {
            EditorUtility.DisplayDialog("Warning", "No components selected for processing", "OK");
            return;
        }

        Undo.SetCurrentGroupName("UI Optimization");
        int group = Undo.GetCurrentGroup();

        var excludedSet = new HashSet<GameObject>(excludedObjects.Where(g => g));
        int processedText = 0, processedImages = 0, processedPanels = 0;

        if (processText)
        {
            var texts = Resources.FindObjectsOfTypeAll<TMP_Text>();
            foreach (var text in texts)
            {
                if (!ShouldProcess(text.gameObject, excludedSet)) continue;

                bool modified = false;
                Undo.RecordObject(text, "Modify Text");

                if (disableTextMaskable && text.maskable)
                {
                    text.maskable = false;
                    modified = true;
                }
                if (disableTextRaycast && text.raycastTarget)
                {
                    text.raycastTarget = false;
                    modified = true;
                }

                if (modified) processedText++;
            }
        }

        if (processImages)
        {
            var images = Resources.FindObjectsOfTypeAll<Image>();
            foreach (var image in images)
            {
                if (!ShouldProcess(image.gameObject, excludedSet)) continue;

                bool modified = false;
                Undo.RecordObject(image, "Modify Image");

                if (disableImageMaskable && image.maskable)
                {
                    image.maskable = false;
                    modified = true;
                }
                if (disableImageRaycast && image.raycastTarget)
                {
                    image.raycastTarget = false;
                    modified = true;
                }

                if (modified) processedImages++;
            }
        }

        if (processPanels)
        {
            var panels = Resources.FindObjectsOfTypeAll<RectTransform>();
            foreach (var panel in panels)
            {
                if (panel.GetComponent<Graphic>() != null) continue;
                if (!ShouldProcess(panel.gameObject, excludedSet)) continue;

                bool modified = false;
                Undo.RecordObject(panel.gameObject, "Modify Panel");

                if (disablePanelMaskable && panel.gameObject.layer != LayerMask.NameToLayer("UI"))
                {
                    panel.gameObject.layer = LayerMask.NameToLayer("UI");
                    modified = true;
                }
                if (disablePanelRaycast && !panel.gameObject.CompareTag("Untagged"))
                {
                    panel.gameObject.tag = "Untagged";
                    modified = true;
                }

                if (modified) processedPanels++;
            }
        }

        Undo.CollapseUndoOperations(group);
        Debug.Log($"Optimized: Text({processedText}), Images({processedImages}), Panels({processedPanels})");
        EditorUtility.DisplayDialog("Results",
            $"Actual changes:\nText: {processedText}\nImages: {processedImages}\nPanels: {processedPanels}",
            "OK");
    }

    private bool ShouldProcess(GameObject obj, HashSet<GameObject> excluded)
    {
        return !excluded.Contains(obj) &&
               !EditorUtility.IsPersistent(obj) &&
               obj.scene.IsValid() &&
               (includeInactive || obj.activeInHierarchy) &&
               (!excludeButtons || obj.GetComponent<Button>() == null);
    }
}
#endif