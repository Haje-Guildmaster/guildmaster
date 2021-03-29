using System;
using GuildMaster.Items;

namespace GuildMaster.Data
{
    public readonly struct ItemStack
    {
        public readonly Item Item;
        public readonly int ItemNum;

        public ItemStack(Item item, int itemNum)
        {
            if (item == null && itemNum != 0) throw new ArgumentException("ItemStack의 item이 null이면 itemNum은 0이어야 합니다.");
            if (item != null && itemNum == 0) throw new ArgumentException("ItemStack의 itemNum이 0이면 item은 null이어야 합니다.");
            if (itemNum < 0)
                throw new ArgumentException($"ItemStack: itemNum({itemNum}) should be equal to or higher than 0");
            Item = item;
            ItemNum = itemNum;
        }

        public bool IsEmpty()
        {
            return Item == null || ItemNum == 0;
        }
        
        public bool Equals(ItemStack other)
        {
            return Equals(Item, other.Item) && ItemNum == other.ItemNum;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemStack other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Item != null ? Item.GetHashCode() : 0) * 397) ^ ItemNum;
            }
        }

        public static bool operator ==(ItemStack left, ItemStack right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ItemStack left, ItemStack right)
        {
            return !left.Equals(right);
        }

    }
}