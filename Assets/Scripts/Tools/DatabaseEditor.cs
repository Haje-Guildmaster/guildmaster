using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

namespace GuildMaster.Tools
{
    public abstract class DatabaseEditor<TDb, TEnum, TElement>: Editor where TDb: Database<TEnum, TElement> where TEnum: Enum
    {
        private SerializedProperty _dataList;
        private TEnum _editingIndex; 
        
        private void OnEnable()
        {
            _dataList = serializedObject.FindProperty("dataList");
            _dataList.arraySize = Enum.GetValues(typeof(TEnum)).Cast<int>().Max()+1;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Label(GetType().Name, EditorStyles.boldLabel);
            
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

            _editingIndex = (TEnum) EditorGUILayout.EnumPopup(_editingIndex);
            var currentItem = _dataList.GetArrayElementAtIndex(Convert.ToInt32(_editingIndex));
            // if (currentItem == null) return;
            
            EditorGUI.indentLevel++;
            CurrentItemField(currentItem);
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