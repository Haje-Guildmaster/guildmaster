using System;
using System.Linq;
using System.Xml.Schema;
using GuildMaster.Tools;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GuildMaster.Items
{
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor: DatabaseEditor<ItemDatabase, Item.ItemCode, ItemStaticData>
    {
        protected override void CurrentItemField(SerializedProperty itemStaticData)
        {
            var itemName = itemStaticData.FindPropertyRelative("itemName");
            var itemDescription = itemStaticData.FindPropertyRelative("itemDescription");
            var isConsumable = itemStaticData.FindPropertyRelative("isConsumable");
            var consumptionEffect = itemStaticData.FindPropertyRelative("consumptionEffect");
            var maxStack = itemStaticData.FindPropertyRelative("maxStack");
            var itemImage = itemStaticData.FindPropertyRelative("itemImage");
            var isEquipable = itemStaticData.FindPropertyRelative("isEquipable");
            var defaultEquipmentStats = itemStaticData.FindPropertyRelative("defaultEquipmentStats");
            var isImportant = itemStaticData.FindPropertyRelative("isImportant");

            EditorGUILayout.PropertyField(itemName);
            EditorGUILayout.PropertyField(itemDescription);
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
                EditorGUILayout.PropertyField(defaultEquipmentStats);
                EditorGUI.indentLevel--;
            }
            else
            {
                defaultEquipmentStats.managedReferenceValue = null;
            }
            
            EditorGUILayout.PropertyField(isImportant);
        }
    }
}