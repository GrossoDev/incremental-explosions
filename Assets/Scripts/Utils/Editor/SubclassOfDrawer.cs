using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SubclassOfAttribute))]
public class SubclassOfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var baseType = ((SubclassOfAttribute)attribute).BaseType;
        var nameProp = property.FindPropertyRelative(nameof(SerializableType.assemblyQualifiedName));

        // Gather all concrete subclasses
        var types = TypeCache.GetTypesDerivedFrom(baseType)
            .Where(t => !t.IsAbstract && !t.IsGenericType)
            .OrderBy(t => t.Name)
            .ToList();

        // Build dropdown options
        var options = new string[types.Count + 1];
        options[0] = "None";
        for (int i = 0; i < types.Count; i++)
        {
            options[i + 1] = types[i].Name;
        }

        // Determine currently selected index
        int currentIndex = 0;
        if (!string.IsNullOrEmpty(nameProp.stringValue))
        {
            Type currentType = Type.GetType(nameProp.stringValue);
            int foundIndex = types.IndexOf(currentType);
            if (foundIndex >= 0) currentIndex = foundIndex + 1;
        }

        // Draw dropdown and update serialized string
        EditorGUI.BeginProperty(position, label, property);
        int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, options);

        if (selectedIndex != currentIndex)
        {
            nameProp.stringValue = selectedIndex == 0 
                ? string.Empty 
                : types[selectedIndex - 1].AssemblyQualifiedName;
        }
        EditorGUI.EndProperty();
    }
}
