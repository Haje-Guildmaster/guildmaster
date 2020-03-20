using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Items
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 0)]
    public class ItemDatabase: Database<Item.ItemCode, ItemStaticData> {}
}