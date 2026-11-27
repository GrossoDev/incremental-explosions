using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.View
{
    [Serializable]
    public class LocalizationEntry
    {
        public string key;
        public string value;
        public string context;
    }

    [Serializable]
    public class LocalizationTable
    {
        public string locale;
        public List<LocalizationEntry> entries = new();

        [NonSerialized] private Dictionary<string, string> _lookup;

        public bool TryGetValue(string key, out string value)
        {
            if (_lookup == null)
            {
                _lookup = new Dictionary<string, string>();
                if (entries != null)
                {
                    foreach (var entry in entries)
                    {
                        if (entry != null && !string.IsNullOrEmpty(entry.key))
                            _lookup[entry.key] = entry.value;
                    }
                }
            }

            return _lookup.TryGetValue(key, out value);
        }
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
    }
}