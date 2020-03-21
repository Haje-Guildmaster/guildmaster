using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

namespace GuildMaster.Database
{
    public abstract class DatabaseEditor<TDb, TIndex, TElement>: Editor where TDb: EditableDatabase<TIndex, TElement> where TIndex: DatabaseIndex, new()
    {
        private SerializedProperty _dataList;
        private SerializedProperty _serializedIndex;

        
        private void OnEnable()
        {
            _dataList = serializedObject.FindProperty("dataList");
            _serializedIndex = serializedObject.FindProperty("currentEditingIndex");
            // int i = 1;
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

            // _serializedIndex.FindPropertyRelative("Value").intValue = 1;
            EditorGUILayout.PropertyField(_serializedIndex);
            var currentItem = _dataList.GetArrayElementAtIndex(_serializedIndex.FindPropertyRelative("Value").intValue);
            // var currentItem = _dataList.GetArrayElementAtIndex(0);
            
            // if (currentItem == null) return;
            
            GUILayout.Space(20);
            GUILayout.Label("Current Item: ");
            EditorGUI.indentLevel++;
            if (currentItem!=null)
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
            EditorGUILayout.PropertyField(currentItem);
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