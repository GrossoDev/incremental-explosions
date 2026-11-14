using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneField
{
    [SerializeField]
    private Object m_SceneAsset;

    [SerializeField]
    private string m_SceneName = "";

    public string SceneName => m_SceneName;

    public static implicit operator string(SceneField sceneField)
    {
        return sceneField.SceneName;
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (m_SceneAsset != null)
        {
            var path = AssetDatabase.GetAssetPath(m_SceneAsset);
            m_SceneName = System.IO.Path.GetFileNameWithoutExtension(path);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var sceneAssetProp = property.FindPropertyRelative("m_SceneAsset");
        var sceneNameProp = property.FindPropertyRelative("m_SceneName");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        EditorGUI.BeginChangeCheck();
        Object value = EditorGUI.ObjectField(position, sceneAssetProp.objectReferenceValue, typeof(SceneAsset), false);
        if (EditorGUI.EndChangeCheck())
        {
            sceneAssetProp.objectReferenceValue = value;
            if (value != null)
            {
                var path = AssetDatabase.GetAssetPath(value);
                sceneNameProp.stringValue = System.IO.Path.GetFileNameWithoutExtension(path);
            }
            else
            {
                sceneNameProp.stringValue = "";
            }
        }
        EditorGUI.EndProperty();
    }
}
#endif
