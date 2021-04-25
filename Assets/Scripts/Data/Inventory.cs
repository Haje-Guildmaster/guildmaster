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

        public ItemStack GetItemStack(int index)
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

                remaining -= _TryAddItemToIndexWithoutOverflow(index, item, remaining);
            }


            // 남은 건 빈 곳을 찾아 앞부터 넣기.
            for (int index = 0; index < Size && remaining > 0; index++)
            {
                ItemStack stack = _itemStackList[index];
                if (!stack.IsEmpty()) continue;

                remaining -= _TryAddItemToIndexWithoutOverflow(index, item, remaining);
            }

            Assert.IsTrue(remaining >= 0);
            Changed?.Invoke();
            return number - remaining;
        }

        public int TryAddItem(ItemStack stack) => TryAddItem(stack.Item, stack.ItemNum);

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

        /// <summary>
        /// 지정된 index의 itemStack에서 일정 개수를 제거합니다. <br/>
        /// item은 기능에는 필요가 없는데, 실수로 다른 거 지우지 말라고 확인용으로 받습니다.
        /// 지정된 index에 주어진 item이 없거나, 충분한 양이 없으면 에러를 던집니다.
        /// </summary>
        /// <param name="targetIndex"></param>
        /// <param name="removingNumber"></param>
        /// <param name="targetItem"></param>
        public void RemoveItemFromIndex(int targetIndex, int removingNumber, Item targetItem)
        {
            var prev = _itemStackList[targetIndex];
            if (prev.IsEmpty())
                throw new InvalidOperationException($"지정한 인덱스 {targetIndex}에 아이템이 존재하지 않습니다.");
            if (!prev.Item.Equals(targetItem))
                throw new InvalidOperationException("item이 일치하지 않습니다.");
            if (prev.ItemNum < removingNumber)
                throw new InvalidCastException(
                    $"지정한 인덱스 {targetIndex}의 아이템 개수가 {prev.ItemNum}으로 제거하는 개수 {removingNumber}보다 많습니다.");

            var afterNum = prev.ItemNum - removingNumber;
            _itemStackList[targetIndex] = new ItemStack(afterNum <= 0 ? null : prev.Item, afterNum);
            Changed?.Invoke();
        }

        /// <summary>
        /// 지정한 index의 itemStack에 아이템을 추가합니다. 지정된 위치에 추가되지 못하는 양은 TryAddItem을 이용해 임의의 위치에
        /// 추가됩니다. 인벤토리가 꽉 차서 그조차 불가능하다면 추가 작업을 하지 않아 아이템이 남습니다. 실제로 추가된 아이템 개수를 반환합니다.
        /// </summary>
        /// <param name="targetIndex"></param>
        /// <param name="addingItem"></param>
        /// <param name="addingNumber"></param>
        /// <returns> 실제로 추가된 아이템 개수</returns>
        public int TryAddItemToIndex(int targetIndex, Item addingItem, int addingNumber)
        {
            int remaining = addingNumber;
            remaining -= _TryAddItemToIndexWithoutOverflow(targetIndex, addingItem, addingNumber);
            remaining -= TryAddItem(addingItem, remaining);
            // Changed?.Invoke  // TryAddItem에서 불림.
            return addingNumber - remaining;
        }

        private int _TryAddItemToIndexWithoutOverflow(int targetIndex, Item addingItem, int addingNumber)
        {
            var stack = _itemStackList[targetIndex];
            if (!stack.IsEmpty() && !stack.Item.Equals(addingItem))
                return 0;
            int maxStackSize = this.IsStacked ? addingItem.StaticData.MaxStack : int.MaxValue;
            
            int prevNum = stack.ItemNum;
            int afterNum = Math.Min(prevNum + addingNumber, maxStackSize);
            _itemStackList[targetIndex] = new ItemStack(addingItem, afterNum);
            return (afterNum - prevNum);
        }
        
        private readonly ItemStack[] _itemStackList;
    }
}