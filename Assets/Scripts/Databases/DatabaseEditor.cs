using System.IO;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    public abstract class DatabaseEditor<TDb, TElement, TIndex> : 
        Editor where TDb : UnityEditableDatabase<TDb, TElement, TIndex> where TIndex: Database<TDb, TElement>.Index, new()
    {
        private SerializedProperty _dataList;
        private SerializedProperty _serializedIndex;


        private void OnEnable()
        {
            _dataList = serializedObject.FindProperty("dataList");
            _serializedIndex = serializedObject.FindProperty("currentEditingIndex");
            // var i = 1;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label(GetType().Name, EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("list size:");
                _dataList.arraySize = EditorGUILayout.IntField(_dataList.arraySize);
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Json: ");
                if (GUILayout.Button("Save"))
                {
                    var fp = EditorUtility.SaveFilePanel("저장 위치", "Assets/Json", $"{typeof(TDb).Name}", "json");
                    SaveToJson(fp);
                }

                if (GUILayout.Button("Load"))
                {
                    var fp = EditorUtility.OpenFilePanel("DB 파일", "Assets/Json", "json");
                    LoadFromJson(fp);
                }
            }
            GUILayout.EndHorizontal();

            
            GUILayout.Space(40);
            
            _serializedIndex.isExpanded = true;
            EditorGUILayout.PropertyField(_serializedIndex);

            var index = _serializedIndex.FindPropertyRelative("Value").intValue;
            var validIndex = 0 <= index && index < _dataList.arraySize;
            var currentItem = (validIndex)
                    ? _dataList.GetArrayElementAtIndex(index) : null;
            
            EditorGUI.indentLevel++;
            if (currentItem != null)
                CurrentItemField(currentItem);
            else
            {
                GUILayout.Label("Null");
            }

            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        protected virtual void CurrentItemField(SerializedProperty currentItem)
        {
            currentItem.isExpanded = true;
            EditorGUILayout.PropertyField(currentItem, new GUIContent());
        }

        private void SaveToJson(string filepath)
        {
            var str = JsonUtility.ToJson(serializedObject.targetObject);
            File.WriteAllText(filepath, str);
        }

        private void LoadFromJson(string filepath)
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(filepath), serializedObject.targetObject);
            serializedObject.Update();
        }
    }
}