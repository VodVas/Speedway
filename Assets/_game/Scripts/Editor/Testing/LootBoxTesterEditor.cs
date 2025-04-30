#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LootBoxTester))]
public class LootBoxTesterEditor : Editor
{
    private static GUIStyle _titleStyle;
    private static GUIStyle TitleStyle => _titleStyle ??= CreateTitleStyle();

    private static GUIStyle CreateTitleStyle()
    {
        var style = new GUIStyle(EditorStyles.boldLabel)
        {
            richText = true,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 16,
            normal = { textColor = Color.HSVToRGB(0.11f, 0.8f, 1f) }
        };
        return style;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("<b>APOCALYPSE SPEEDWAY</b>", TitleStyle);

        if (GUILayout.Button("RUN LOOT TEST", GUILayout.Height(30)))
        {
            RunTestLogic();
        }

        DrawDefaultInspector();
    }

    [MenuItem("Tools/Apocalypse/Tests/Run LootBox Chance Test")]
    private static void RunTestMenu() => RunTestLogic();

    private static void RunTestLogic()
    {
        var tester = FindFirstObjectByType<LootBoxTester>();
        if (!tester)
        {
            Debug.LogError("No LootBoxTester found in scene!");
            return;
        }

        tester.RunBalanceTest();
        EditorApplication.QueuePlayerLoopUpdate();
    }
}
#endif