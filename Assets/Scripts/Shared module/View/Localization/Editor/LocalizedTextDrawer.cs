using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Shared.View
{
    [CustomPropertyDrawer(typeof(LocalizedText))]
    public class LocalizedTextDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty keyProp = property.FindPropertyRelative("key");
            Rect controlRect = EditorGUI.PrefixLabel(position, label);

            Localization settings = Localization.Instance;
            if (settings == null)
            {
                EditorGUI.HelpBox(controlRect, "Missing 'Resources/Localization.asset'", MessageType.Warning);
                EditorGUI.EndProperty();
                return;
            }

            string currentKey = keyProp.stringValue;
            bool keyExists = KeyExists(settings, currentKey);

            Color prevColor = GUI.color;
            if (!string.IsNullOrEmpty(currentKey) && !keyExists)
                GUI.color = new Color(1f, 0.55f, 0.55f);

            string buttonText = string.IsNullOrEmpty(currentKey)
                ? "<Select Localization Key>"
                : (keyExists ? currentKey : $"{currentKey} (Missing)");

            GUIContent dropdownContent = new GUIContent(buttonText, GetContextTooltip(settings, currentKey));

            if (EditorGUI.DropdownButton(controlRect, dropdownContent, FocusType.Keyboard))
            {
                SerializedObject targetObj = property.serializedObject;
                string propPath = keyProp.propertyPath;

                var dropdown = new LocalizationKeyDropdown(
                    new AdvancedDropdownState(),
                    settings.Keys,
                    selectedKey =>
                    {
                        targetObj.Update();
                        SerializedProperty p = targetObj.FindProperty(propPath);
                        if (p != null)
                        {
                            p.stringValue = selectedKey;
                            targetObj.ApplyModifiedProperties();
                        }
                    });

                dropdown.Show(controlRect);
            }

            GUI.color = prevColor;
            EditorGUI.EndProperty();
        }

        private static bool KeyExists(Localization settings, string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            foreach (var k in settings.Keys)
            {
                if (k.key == key) return true;
            }
            return false;
        }

        private static string GetContextTooltip(Localization settings, string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            foreach (var k in settings.Keys)
            {
                if (k.key == key) return k.context;
            }
            return "Key not found in Localization settings.";
        }
    }

    internal class LocalizationKeyDropdown : AdvancedDropdown
    {
        private class KeyItem : AdvancedDropdownItem
        {
            public string FullKey { get; }
            public KeyItem(string displayName, string fullKey) : base(displayName)
            {
                FullKey = fullKey;
            }
        }

        private readonly IReadOnlyList<LocalizationKey> _keys;
        private readonly Action<string> _onSelected;

        public LocalizationKeyDropdown(
            AdvancedDropdownState state,
            IReadOnlyList<LocalizationKey> keys,
            Action<string> onSelected) : base(state)
        {
            _keys = keys;
            _onSelected = onSelected;
            minimumSize = new Vector2(260f, 320f);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("Localization Keys");
            var folderCache = new Dictionary<string, AdvancedDropdownItem>();

            root.AddChild(new KeyItem("<None>", string.Empty));

            foreach (var entry in _keys)
            {
                if (string.IsNullOrEmpty(entry.key)) continue;

                string[] parts = entry.key.Split('/');
                AdvancedDropdownItem parent = root;
                string pathAccumulator = "";

                for (int i = 0; i < parts.Length - 1; i++)
                {
                    pathAccumulator = i == 0 ? parts[i] : $"{pathAccumulator}/{parts[i]}";
                    if (!folderCache.TryGetValue(pathAccumulator, out var folder))
                    {
                        folder = new AdvancedDropdownItem(parts[i]);
                        parent.AddChild(folder);
                        folderCache[pathAccumulator] = folder;
                    }
                    parent = folder;
                }

                parent.AddChild(new KeyItem(parts[^1], entry.key));
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is KeyItem keyItem)
                _onSelected?.Invoke(keyItem.FullKey);
        }
    }
}