using GuildMaster.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace GuildMaster.Data
{
    public class Inventory
    {
        public Inventory(int size, bool isStacked)
        {
            this.IsStacked = isStacked;
            _itemStackList = new ItemStack[size];
        }

        public readonly bool IsStacked;
        public int Size => _itemStackList.Length;
        public IReadOnlyList<ItemStack> ItemStackList => _itemStackList;
        public event Action Changed;

        [Obsolete]
        public void ChangeItemIndex(int index1, int index2)
        {
            Swap(index1, index2);
        }

        public void Swap(int index1, int index2)
        {
            var temp = _itemStackList[index1];
            _itemStackList[index1] = _itemStackList[index2];
            _itemStackList[index2] = temp;
            Changed?.Invoke();
        }

        public ItemStack TryGetItemStack(int index)
        {
            return _itemStackList.ElementAtOrDefault(index);
        }


        /// <summary>
        /// 지정한 아이템을 지정한 숫자만큼 인벤토리에 넣습니다. <br/>
        /// 넣는 데에 성공한 개수를 반환합니다.
        /// </summary>
        /// <returns> 인벤토리에 실제로 추가된 아이템 숫자</returns>
        public int TryAddItem(Item item, int number)
        {
            if (item == null) throw new ArgumentException("Inventory: Cannot add null item");

            var itemStaticData = item.StaticData;
            int maxStackSize = this.IsStacked ? itemStaticData.MaxStack : int.MaxValue;

            int remaining = number;

            // 이미 이 아이템이 지정된 스택이 있다면 그곳에 넣기.
            for (int index = 0; index < Size && remaining > 0; index++)
            {
                ItemStack stack = _itemStackList[index];
                if (!item.Equals(stack.Item)) continue;

                int prevNum = stack.ItemNum;
                int afterNum = Math.Min(prevNum + remaining, maxStackSize);

                remaining -= (afterNum - prevNum);
                _itemStackList[index] = new ItemStack(item, afterNum);
            }


            // 남은 건 빈 곳을 찾아 앞부터 넣기.
            for (int index = 0; index < Size && remaining > 0; index++)
            {
                ItemStack stack = _itemStackList[index];
                if (stack.Item != null) continue;
                int puttingNum = Math.Min(remaining, maxStackSize);

                remaining -= puttingNum;
                _itemStackList[index] = new ItemStack(puttingNum <= 0 ? null : item, puttingNum);
            }

            Assert.IsTrue(remaining >= 0);
            Changed?.Invoke();
            return number - remaining;
        }

        /// <summary>
        /// 지정한 아이템을 지정한 숫자만큼 제거합니다. <br/>
        /// 실제로 제거된 아이템 개수를 반환합니다.
        /// </summary>
        /// <param name="item"> 제거할 아이템 </param>
        /// <param name="number"> 실제로 제거된 아이템 개수 </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public int TryRemoveItem(Item item, int number)
        {
            if (item == null) throw new ArgumentException("Inventory: Cannot remove null item");

            int remaining = number;

            for (int index = Size - 1; index >= 0 && remaining > 0; index--)
            {
                ItemStack stack = _itemStackList[index];
                if (!item.Equals(stack.Item)) continue;

                int prevNum = stack.ItemNum;
                int afterNum = Math.Max(prevNum - remaining, 0);

                remaining -= (prevNum - afterNum);
                _itemStackList[index] = new ItemStack(afterNum <= 0 ? null : item, afterNum);
            }

            Assert.IsTrue(remaining >= 0);
            Changed?.Invoke();
            return number - remaining;
        }

        [Obsolete]
        public int TryDeleteItem(Item item, int number)
        {
            return TryRemoveItem(item, number);
        }


        /// <summary>
        /// 지정한 만큼의 아이템을 넣을 자리가 인벤토리에 있는지 반환
        /// </summary>
        /// <param name="item"> 아이템 </param>
        /// <param name="number"> 숫자 </param>
        /// <returns></returns>
        public bool CanAddItem(Item item, int number)
        {
            if (item == null) return false;
            if (IsStacked)
                return Array.Exists(_itemStackList, stack => stack.Item == null || stack.Item.Equals(item));

            var itemStaticData = item.StaticData;

            int cntSpace = 0;

            // 이미 이 아이템이 지정된 스택이 있다면 그곳에 넣기.
            foreach (ItemStack stack in _itemStackList)
            {
                if (stack.Item == null)
                    cntSpace += itemStaticData.MaxStack;
                else if (stack.Item.Equals(item))
                    cntSpace += itemStaticData.MaxStack - stack.ItemNum;
            }

            return cntSpace > 0;
        }

        /// <summary>
        /// 지정한 아이템의 개수를 반환.
        /// </summary>
        /// <param name="item"> 개수를 셀 아이템 </param>
        /// <returns> 아이템 개수 </returns>
        public int CountItem(Item item)
        {
            if (item == null) throw new ArgumentException("Inventory: Cannot count null item");
            int cnt = 0;
            foreach (ItemStack stack in _itemStackList)
            {
                if (item.Equals(stack.Item))
                    cnt += stack.ItemNum;
            }

            return cnt;
        }

        public void AddItem(Item item, int number)
        {
            if (!CanAddItem(item, number)) throw new InvalidOperationException("Inventory: Cannot add all items");
            var cnt = TryAddItem(item, number);
            Assert.IsTrue(cnt == number);
        }

        public void RemoveItem(Item item, int number)
        {
            int cnt = CountItem(item);
            if (cnt < number)
                throw new InvalidOperationException(
                    $"Inventory: Item count({cnt}) is less than removing number({number})");
            var removed = TryRemoveItem(item, number);
            Assert.IsTrue(removed == number);
        }

        private readonly ItemStack[] _itemStackList;
    }
}