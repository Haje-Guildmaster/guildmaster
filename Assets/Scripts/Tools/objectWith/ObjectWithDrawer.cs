using GuildMaster.Windows;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Tools
{
    [CustomPropertyDrawer(typeof(ObjectWith<>))]
    public class ObjectWithDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetObject = property.FindPropertyRelative("_innerValue");
            var targetType = fieldInfo.FieldType.GetGenericArguments()[0];
            // label.text += " " + targetType.FullName;
            targetObject.objectReferenceValue =
                EditorGUI.ObjectField(position, label, targetObject.objectReferenceValue, targetType, true);
        }
    }
}