using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

[Serializable]
public class SystemLocalization : MonoBehaviour
{
    [Serializable]
    private struct LocalizationItem
    {
        public string key;
        public string ru;
        public string en;
        public string tr;
    }

    [SerializeField] private LocalizationItem[] _phrases;

    private Dictionary<string, Dictionary<string, string>> _phrasesDict;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        _phrasesDict = new Dictionary<string, Dictionary<string, string>>();

        foreach (var item in _phrases)
        {
            var translations = new Dictionary<string, string>
            {
                { "ru", item.ru },
                { "en", item.en },
                { "tr", item.tr }
            };

            _phrasesDict[item.key] = translations;
        }
    }

    public string GetPhrase(string key, params object[] args)
    {
        if (_phrasesDict == null) InitializeDictionary();

        string lang = YandexGame.EnvironmentData.language;
        if (string.IsNullOrEmpty(lang)) lang = "en";

        if (!_phrasesDict.TryGetValue(key, out var translations))
        {
            Debug.LogError($"Localization key not found: {key}");
            return $"#{key}#";
        }

        if (!translations.TryGetValue(lang, out var phrase))
        {
            phrase = translations["en"];
        }

        return args.Length > 0 ? string.Format(phrase, args) : phrase;
    }
}