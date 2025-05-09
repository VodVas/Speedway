//#if UNITY_EDITOR
//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine.UI;
//using System.Linq;
//using System;

//namespace Apocalypse.Tools
//{
//    public class TextFinderWindow : EditorWindow
//    {
//        private List<TextObjectInfo> _textObjects = new List<TextObjectInfo>();
//        private Vector2 _scrollPosition;
//        private string _searchFilter = string.Empty;
//        private bool _showLegacyText = true;
//        private bool _showTMP = true;
//        private bool _showTMPUI = true;
//        private bool _isSearching = false;
//        private int _totalCount = 0;
//        private int _legacyTextCount = 0;
//        private int _tmpTextCount = 0;
//        private int _tmpUITextCount = 0;
//        private Dictionary<GameObject, string> _hierarchyPathCache = new Dictionary<GameObject, string>();

//        [MenuItem("Tools/Apocalypse/Text Finder")]
//        public static void ShowWindow()
//        {
//            GetWindow<TextFinderWindow>("Text Finder");
//        }

//        private void OnGUI()
//        {
//            DrawHeader();
//            DrawControls();
//            DrawTextObjectList();
//            DrawFooter();
//        }

//        private void DrawHeader()
//        {
//            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//            GUILayout.Label("Text Finder", EditorStyles.boldLabel);
//            EditorGUILayout.LabelField("Find all text elements in the current scene.", EditorStyles.wordWrappedLabel);
//            EditorGUILayout.EndVertical();
//            EditorGUILayout.Space();
//        }

//        private void DrawControls()
//        {
//            EditorGUILayout.BeginHorizontal();

//            EditorGUI.BeginDisabledGroup(_isSearching);
//            if (GUILayout.Button("START", GUILayout.Height(30)))
//            {
//                FindAllTextObjects();
//            }
//            EditorGUI.EndDisabledGroup();

//            if (GUILayout.Button("Clear", GUILayout.Height(30)))
//            {
//                _textObjects.Clear();
//                _hierarchyPathCache.Clear();
//                _totalCount = 0;
//                _legacyTextCount = 0;
//                _tmpTextCount = 0;
//                _tmpUITextCount = 0;
//            }

//            EditorGUILayout.EndHorizontal();
//            EditorGUILayout.Space();
//            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//            EditorGUILayout.LabelField("Filters", EditorStyles.boldLabel);

//            _searchFilter = EditorGUILayout.TextField("Search:", _searchFilter);

//            EditorGUILayout.BeginHorizontal();
//            _showLegacyText = EditorGUILayout.ToggleLeft("Legacy UI Text", _showLegacyText, GUILayout.Width(120));
//            _showTMP = EditorGUILayout.ToggleLeft("TextMeshPro", _showTMP, GUILayout.Width(120));
//            _showTMPUI = EditorGUILayout.ToggleLeft("TMP UI", _showTMPUI, GUILayout.Width(120));
//            EditorGUILayout.EndHorizontal();

//            EditorGUILayout.EndVertical();

//            if (_totalCount > 0)
//            {
//                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//                EditorGUILayout.LabelField("Statistics", EditorStyles.boldLabel);
//                EditorGUILayout.LabelField($"Total Text Objects: {_totalCount}");

//                if (_legacyTextCount > 0)
//                    EditorGUILayout.LabelField($"Legacy UI Text: {_legacyTextCount}");
//                if (_tmpTextCount > 0)
//                    EditorGUILayout.LabelField($"TextMeshPro: {_tmpTextCount}");
//                if (_tmpUITextCount > 0)
//                    EditorGUILayout.LabelField($"TextMeshProUGUI: {_tmpUITextCount}");

//                EditorGUILayout.EndVertical();
//            }

//            EditorGUILayout.Space();
//        }

//        private void DrawTextObjectList()
//        {
//            if (_isSearching)
//            {
//                EditorGUILayout.HelpBox("Searching for text objects...", MessageType.Info);
//                return;
//            }

//            if (_textObjects.Count == 0)
//            {
//                EditorGUILayout.HelpBox("No text objects found. Click START to find text objects in the scene.", MessageType.Info);
//                return;
//            }

//            EditorGUILayout.LabelField("Text Objects:", EditorStyles.boldLabel);

//            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

//            var filteredObjects = _textObjects.Where(obj =>
//                (!string.IsNullOrEmpty(_searchFilter) ?
//                    (obj.GameObject.name.IndexOf(_searchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
//                     obj.Content.IndexOf(_searchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
//                     GetHierarchyPath(obj.GameObject).IndexOf(_searchFilter, StringComparison.OrdinalIgnoreCase) >= 0) :
//                    true) &&
//                ((obj.Type == TextObjectType.LegacyText && _showLegacyText) ||
//                 (obj.Type == TextObjectType.TextMeshPro && _showTMP) ||
//                 (obj.Type == TextObjectType.TextMeshProUGUI && _showTMPUI))
//            ).ToList();

//            foreach (var textObj in filteredObjects)
//            {
//                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//                EditorGUILayout.BeginHorizontal();
//                GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);

//                switch (textObj.Type)
//                {
//                    case TextObjectType.LegacyText:
//                        labelStyle.normal.textColor = new Color(0.9f, 0.4f, 0.3f);
//                        break;
//                    case TextObjectType.TextMeshPro:
//                        labelStyle.normal.textColor = new Color(0.3f, 0.6f, 0.9f);
//                        break;
//                    case TextObjectType.TextMeshProUGUI:
//                        labelStyle.normal.textColor = new Color(0.3f, 0.9f, 0.4f);
//                        break;
//                }

//                EditorGUILayout.LabelField($"{textObj.GameObject.name} ({textObj.Type})", labelStyle);

//                if (GUILayout.Button("Select", GUILayout.Width(60)))
//                {
//                    Selection.activeGameObject = textObj.GameObject;
//                    EditorGUIUtility.PingObject(textObj.GameObject);
//                }

//                EditorGUILayout.EndHorizontal();
//                EditorGUILayout.LabelField("Path:", EditorStyles.miniBoldLabel);
//                EditorGUILayout.LabelField(GetHierarchyPath(textObj.GameObject), EditorStyles.miniLabel);
//                EditorGUILayout.LabelField("Content:", EditorStyles.miniBoldLabel);
//                EditorGUILayout.LabelField(textObj.Content, EditorStyles.wordWrappedLabel);
//                EditorGUILayout.EndVertical();
//                EditorGUILayout.Space(2);
//            }

//            EditorGUILayout.EndScrollView();
//        }

//        private void DrawFooter()
//        {
//            EditorGUILayout.Space();
//            EditorGUILayout.LabelField("Tip: Click on an object to select it in the hierarchy.", EditorStyles.miniLabel);
//        }

//        private void FindAllTextObjects()
//        {
//            _isSearching = true;
//            _textObjects.Clear();
//            _hierarchyPathCache.Clear();
//            _totalCount = 0;
//            _legacyTextCount = 0;
//            _tmpTextCount = 0;
//            _tmpUITextCount = 0;

//            if (_showLegacyText)
//            {
//                Text[] legacyTexts = FindObjectsOfType<Text>();
//                foreach (var text in legacyTexts)
//                {
//                    if (text.gameObject.activeInHierarchy)
//                    {
//                        _textObjects.Add(new TextObjectInfo(text.gameObject, TextObjectType.LegacyText, text.text));
//                        _legacyTextCount++;
//                    }
//                }
//            }

//            if (_showTMP)
//            {
//                TextMeshPro[] tmpTexts = FindObjectsOfType<TextMeshPro>();
//                foreach (var text in tmpTexts)
//                {
//                    if (text.gameObject.activeInHierarchy)
//                    {
//                        _textObjects.Add(new TextObjectInfo(text.gameObject, TextObjectType.TextMeshPro, text.text));
//                        _tmpTextCount++;
//                    }
//                }
//            }

//            if (_showTMPUI)
//            {
//                TextMeshProUGUI[] tmpUITexts = FindObjectsOfType<TextMeshProUGUI>();
//                foreach (var text in tmpUITexts)
//                {
//                    if (text.gameObject.activeInHierarchy)
//                    {
//                        _textObjects.Add(new TextObjectInfo(text.gameObject, TextObjectType.TextMeshProUGUI, text.text));
//                        _tmpUITextCount++;
//                    }
//                }
//            }

//            _totalCount = _textObjects.Count;
//            _isSearching = false;

//            Repaint();
//        }

//        private string GetHierarchyPath(GameObject obj)
//        {
//            if (_hierarchyPathCache.TryGetValue(obj, out string cachedPath))
//                return cachedPath;

//            string path = obj.name;
//            Transform parent = obj.transform.parent;

//            while (parent != null)
//            {
//                path = parent.name + "/" + path;
//                parent = parent.parent;
//            }

//            _hierarchyPathCache[obj] = path;
//            return path;
//        }
//    }

//    public enum TextObjectType
//    {
//        LegacyText,
//        TextMeshPro,
//        TextMeshProUGUI
//    }

//    public class TextObjectInfo
//    {
//        public GameObject GameObject { get; }
//        public TextObjectType Type { get; }
//        public string Content { get; }

//        public TextObjectInfo(GameObject gameObject, TextObjectType type, string content)
//        {
//            GameObject = gameObject;
//            Type = type;
//            Content = content;
//        }
//    }
//}
//#endif