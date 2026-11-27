using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Shared.View
{
    [CreateAssetMenu(fileName = "Localization", menuName = "Localization/Localization Settings")]
    public class Localization : ScriptableObject
    {
        private const string ResourceAssetPath = "Localization";
        private const string TablesFolder = "Localization";
        public const string UnlocalizedFallback = "[unlocalized]";

        private static Localization _instance;
        public static Localization Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<Localization>(ResourceAssetPath);
                    if (_instance != null && Application.isPlaying)
                    {
                        _instance.Initialize();
                    }
                }
                return _instance;
            }
        }

        [Header("Configuration")]
        [SerializeField] private string defaultLocale = "en";
        [SerializeField] private TextAsset defaultTableJson;

        [Header("Keys")]
        [SerializeField] private List<LocalizationKey> keys = new();

        private readonly List<LocalizationTable> _parsedTables = new();
        private LocalizationTable _defaultTable;
        private LLMProvider translator;
        private bool _isInitialized;

        public string CurrentLocale { get; private set; }
        public IReadOnlyList<LocalizationKey> Keys => keys;

        public Localization()
        {
            _instance = this;
            translator = LLMProvider.Instance;
        }

        private void OnEnable()
        {
            _isInitialized = false;
            _parsedTables.Clear();
            _defaultTable = null;
        }

        public void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            LoadDefaultTable();
            CurrentLocale = defaultLocale;
        }

        private void LoadDefaultTable()
        {
            if (defaultTableJson == null)
            {
                Debug.LogError("[Localization] Default table JSON is not assigned in Localization settings.");
                return;
            }

            var data = JsonUtility.FromJson<LocalizationTableData>(defaultTableJson.text);
            _defaultTable = new LocalizationTable(defaultLocale, data);

            if (!HasParsedTable(defaultLocale))
                _parsedTables.Add(_defaultTable);
        }

        public async Task SetLanguageAsync(string locale)
        {
            if (!_isInitialized && Application.isPlaying)
                Initialize();

            if (string.IsNullOrWhiteSpace(locale))
            {
                Debug.LogWarning("[Localization] Attempted to set an empty locale.");
                return;
            }

            CurrentLocale = locale;

            // 1. Already parsed in memory
            if (HasParsedTable(locale))
                return;

            // 2. Try loading from Resources/Localization/{locale}
            TextAsset jsonAsset = Resources.Load<TextAsset>($"{TablesFolder}/{locale}");
            if (jsonAsset != null)
            {
                var data = JsonUtility.FromJson<LocalizationTableData>(jsonAsset.text);
                _parsedTables.Add(new LocalizationTable(locale, data));
                return;
            }

            // 3. Not found in Resources -> translate default table using Gemini
            if (translator == null)
            {
                Debug.LogError($"[Localization] No JSON found for '{locale}' and no LLMTranslator is assigned.");
                return;
            }

            if (_defaultTable == null)
            {
                Debug.LogError("[Localization] Cannot translate because the default table failed to load.");
                return;
            }

            Debug.Log($"[Localization] Translating default table to '{locale}' via Gemini...");
            LocalizationTable translatedTable = await translator.TranslateAsync(_defaultTable, keys, locale);

            if (translatedTable != null && !HasParsedTable(locale))
            {
                _parsedTables.Add(translatedTable);
            }
        }

        public string GetString(string key)
        {
            if (!_isInitialized && Application.isPlaying)
                Initialize();

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("[Localization] Requested localization with an empty key.");
                return UnlocalizedFallback;
            }

            // 1. Current language table
            LocalizationTable currentTable = GetParsedTable(CurrentLocale);
            if (currentTable != null && currentTable.TryGetValue(key, out string localizedValue))
            {
                return localizedValue;
            }

            // 2. Fallback to default table
            if (_defaultTable != null && _defaultTable.TryGetValue(key, out string defaultValue))
            {
                return defaultValue;
            }

            // 3. Missing in both
            Debug.LogError($"[Localization] Missing localization key '{key}' in locale '{CurrentLocale}' and default '{defaultLocale}'.");
            return UnlocalizedFallback;
        }

        private bool HasParsedTable(string locale) => GetParsedTable(locale) != null;

        private LocalizationTable GetParsedTable(string locale)
        {
            for (int i = 0; i < _parsedTables.Count; i++)
            {
                if (string.Equals(_parsedTables[i].Locale, locale, StringComparison.OrdinalIgnoreCase))
                    return _parsedTables[i];
            }
            return null;
        }
    }
}