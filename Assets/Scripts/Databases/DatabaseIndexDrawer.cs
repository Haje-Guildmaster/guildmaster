using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    /// <summary>
    /// 데이터베이스 인덱스의 유니티 편집기(PropertyDrawer)입니다. 버튼 식으로 클릭하면 선택 가능한 인덱스 전부가 뜹니다.  
    /// </summary>
    /// <typeparam name="TDb"> DB의 타입 </typeparam>
    /// <typeparam name="TElement"> DB의 원소 타입 </typeparam>
    public abstract class DatabaseIndexDrawer<TDb, TElement> : PropertyDrawer where TDb : IndexDatabase<TDb, TElement>
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
            var currentElement = GetDatabase()._GetElement(new IndexDatabase<TDb, TElement>.Index(currentIntValue));
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
                    intIndexProperty.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private TDb GetDatabase()
        {
            if (_databaseCache != null) return _databaseCache;

            string path = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:"+ typeof(TDb).Name)[0]);
            return _databaseCache = AssetDatabase.LoadAssetAtPath<TDb>(path);
        }

        private TDb _databaseCache;
    }
}