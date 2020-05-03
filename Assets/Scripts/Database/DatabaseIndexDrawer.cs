using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Database
{
    public abstract class DatabaseIndexDrawer<TDb, TElement> : PropertyDrawer where TDb : Database<TDb, TElement>
    {
        [Pure]
        protected abstract string GetElementDescriptionWithIndex(int i, TElement element);
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            DrawButton(property, position);
            EditorGUI.EndProperty();
        }


        private void DrawButton(SerializedProperty property, Rect position)
        {
            var backgroundColor = new Color(0.85f, 0.184f, 0.97f, 0.67f);

            var buttonPosition = position;
            buttonPosition.x += EditorGUIUtility.labelWidth + 1 * EditorGUIUtility.standardVerticalSpacing;
            buttonPosition.width = position.width - EditorGUIUtility.labelWidth -
                                   1 * EditorGUIUtility.standardVerticalSpacing;
            buttonPosition.height = EditorGUIUtility.singleLineHeight;

            var storedIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var storedColor = GUI.backgroundColor;
            GUI.backgroundColor = backgroundColor;

            var intIndexProperty = property.FindPropertyRelative("Value");
            var currentIntValue = intIndexProperty.intValue;
            var currentElement = GetDatabase()._GetElement(new Database<TDb, TElement>.Index(currentIntValue));
            if (GUI.Button(buttonPosition,
                new GUIContent(currentElement != null
                    ? GetElementDescriptionWithIndex(currentIntValue, currentElement)
                    : "Null")))
            {
                OpenSelection(intIndexProperty);
            }

            GUI.backgroundColor = storedColor;
            EditorGUI.indentLevel = storedIndent;
        }

        private void OpenSelection(SerializedProperty intIndexProperty)
        {
            var context = new GenericMenu();
            FillContextMenu();
            context.ShowAsContext();

            void FillContextMenu()
            {
                var db = GetDatabase();
                foreach (var (i, elem) in db.GetAllElements())
                {
                    context.AddItem(new GUIContent(GetElementDescriptionWithIndex(i, elem)), false, SetIndex, i);
                }

                void SetIndex(object indexNumberObject)
                {
                    var ind = (int) indexNumberObject;
                    intIndexProperty.serializedObject.Update();
                    intIndexProperty.intValue = ind;
                    intIndexProperty.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
        }

        private TDb GetDatabase()
        {
            return Resources.FindObjectsOfTypeAll<TDb>()[0];
        }
    }
}