using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.View
{
    [Serializable]
    public class LocalizationKey
    {
        [Tooltip("Use '/' for categories in the inspector dropdown, e.g. 'UI/Menu/Play'")]
        public string key;

        [TextArea(2, 4)]
        [Tooltip("Context provided to Gemini to improve translation accuracy.")]
        public string context;
    }

    [Serializable]
    public class LocalizationEntry
    {
        public string key;
        public string value;
    }

    [Serializable]
    public class LocalizationTableData
    {
        public string locale;
        public List<LocalizationEntry> entries = new();
    }

    public class LocalizationTable
    {
        public string Locale { get; }
        private readonly Dictionary<string, string> _lookup = new();

        public LocalizationTable(string locale, LocalizationTableData data)
        {
            Locale = locale;
            if (data?.entries == null) return;

            foreach (var entry in data.entries)
            {
                if (!string.IsNullOrEmpty(entry.key))
                    _lookup[entry.key] = entry.value;
            }
        }

        public bool TryGetValue(string key, out string value) => _lookup.TryGetValue(key, out value);

        public IReadOnlyDictionary<string, string> Entries => _lookup;
    }

    [Serializable]
    public struct LocalizedText
    {
        [SerializeField] private string key;

        public string Key => key;

        public LocalizedText(string key)
        {
            this.key = key;
        }

        public string Get()
        {
            return Localization.Instance.GetString(key);
        }

        public string GetFormatted(params object[] values)
        {
            string template = Localization.Instance.GetString(key);

            return string.Format(template, values);
        }

        public override string ToString() => Get();
    }
}