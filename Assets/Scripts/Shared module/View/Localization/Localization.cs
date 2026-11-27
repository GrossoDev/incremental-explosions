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

        private readonly List<LocalizationKey> keys = new();
        private readonly List<LocalizationTable> _parsedTables = new();
        private LocalizationTable _defaultTable;
        private LLMTranslator translator;
        private bool _isInitialized;
        private bool _defaultTableLoadAttempted;
        private string _loadedDefaultTableJson;
        private string _loadedDefaultLocale;

        public string CurrentLocale { get; private set; }
        public IReadOnlyList<LocalizationKey> Keys
        {
            get
            {
                LoadDefaultTable();
                return keys;
            }
        }

        public Localization()
        {
            _instance = this;
            translator = new LLMTranslator();
        }

        private void OnEnable()
        {
            _isInitialized = false;
            _parsedTables.Clear();
            _defaultTable = null;
            keys.Clear();
            _defaultTableLoadAttempted = false;
            _loadedDefaultTableJson = null;
            _loadedDefaultLocale = null;
        }

        public void Initialize()
        {
            LoadDefaultTable();
            if (_isInitialized) return;
            CurrentLocale = defaultLocale;
            _isInitialized = true;
        }

        private void LoadDefaultTable()
        {
#if UNITY_EDITOR
            string tableJson = defaultTableJson == null ? null : defaultTableJson.text;
            if (_defaultTableLoadAttempted
                && string.Equals(tableJson, _loadedDefaultTableJson, StringComparison.Ordinal)
                && string.Equals(defaultLocale, _loadedDefaultLocale, StringComparison.Ordinal))
            {
                return;
            }
#else
            if (_defaultTableLoadAttempted) return;
            string tableJson = defaultTableJson == null ? null : defaultTableJson.text;
#endif

            bool isReload = _defaultTableLoadAttempted;
            string previousDefaultLocale = _loadedDefaultLocale;
            _defaultTableLoadAttempted = true;
            _loadedDefaultTableJson = tableJson;
            _loadedDefaultLocale = defaultLocale;

            keys.Clear();
            _defaultTable = null;

            if (isReload)
            {
                for (int i = _parsedTables.Count - 1; i >= 0; i--)
                {
                    if (string.Equals(_parsedTables[i].Locale, previousDefaultLocale, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(_parsedTables[i].Locale, defaultLocale, StringComparison.OrdinalIgnoreCase))
                    {
                        _parsedTables.RemoveAt(i);
                    }
                }
            }

            if (defaultTableJson == null)
            {
                Debug.LogError("[Localization] Default table JSON is not assigned in Localization settings.");
                return;
            }

            var data = JsonUtility.FromJson<LocalizationTableData>(tableJson);
            if (data?.entries == null)
            {
                Debug.LogError("[Localization] Default table JSON does not contain an entries list.");
                return;
            }

            _defaultTable = new LocalizationTable(defaultLocale, data);
            foreach (var entry in data.entries)
            {
                if (entry == null || string.IsNullOrEmpty(entry.key)) continue;
                keys.Add(new LocalizationKey { key = entry.key, context = entry.context });
            }

            int defaultTableIndex = _parsedTables.FindIndex(
                table => string.Equals(table.Locale, defaultLocale, StringComparison.OrdinalIgnoreCase));
            if (defaultTableIndex >= 0)
                _parsedTables[defaultTableIndex] = _defaultTable;
            else
                _parsedTables.Add(_defaultTable);
        }

        public async Task SetLanguageAsync(string locale)
        {
            LoadDefaultTable();

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
            else
                LoadDefaultTable();

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