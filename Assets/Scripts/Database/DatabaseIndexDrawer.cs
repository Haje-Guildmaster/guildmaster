using GuildMaster.Items;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GuildMaster.Database
{
    // [CustomPropertyDrawer(typeof(DatabaseIndex<ItemDatabase, ItemStaticData>))]
    public class DatabaseIndexDrawer: PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var valueField = new PropertyField(property.FindPropertyRelative("Value"));

            // Add fields to the container.
            container.Add(valueField);
            return container;
        }
    }
}