using System.Xml.Schema;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GuildMaster.Items
{
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor: Editor
    {
        private SerializedProperty _itemStaticDataMap;
        private Item.ItemCode _itemCode;
        // private SerializedObject _serializedItemCode;

        void OnEnable()
        {
            _itemStaticDataMap = serializedObject.FindProperty("itemStaticDataMap");
            // _serializedItemCode = new UnityEditor.SerializedObject(_itemCode);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Item Database");
            _itemCode = (Item.ItemCode) EditorGUILayout.EnumPopup(_itemCode);
            
            var itemStaticData = _itemStaticDataMap.GetArrayElementAtIndex((int) _itemCode);

            EditorGUI.indentLevel++;
            DrawItemStaticDataInspector(itemStaticData);
            EditorGUI.indentLevel--;
            
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private bool _isConsumable;
        private void DrawItemStaticDataInspector(SerializedProperty itemStaticData)
        {
            var itemName = itemStaticData.FindPropertyRelative("itemName");
            var isConsumable = itemStaticData.FindPropertyRelative("isConsumable");
            var consumptionEffect = itemStaticData.FindPropertyRelative("consumptionEffect");
            var maxStack = itemStaticData.FindPropertyRelative("maxStack");
            var itemImage = itemStaticData.FindPropertyRelative("itemImage");
            var isEquipable = itemStaticData.FindPropertyRelative("isEquipable");
            var defaultEquipmentStatsRef = itemStaticData.FindPropertyRelative("defaultEquipmentStatsRef");
            var isImportant = itemStaticData.FindPropertyRelative("isImportant");

            EditorGUILayout.PropertyField(itemName);
            EditorGUILayout.PropertyField(itemImage);
            EditorGUILayout.PropertyField(maxStack);

            EditorGUILayout.PropertyField(isConsumable);
            if (isConsumable.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(consumptionEffect);
                EditorGUI.indentLevel--;
            }
            else
            {
                consumptionEffect.managedReferenceValue = null;
            }
            
            EditorGUILayout.PropertyField(isEquipable);
            if (isEquipable.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(defaultEquipmentStatsRef);
                EditorGUI.indentLevel--;
            }
            else
            {
                defaultEquipmentStatsRef.managedReferenceValue = null;
            }
            
            EditorGUILayout.PropertyField(isImportant);
        }
    }
}