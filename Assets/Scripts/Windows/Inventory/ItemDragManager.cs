using GuildMaster.Data;
using GuildMaster.Items;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public interface IItemGiver
    {
        /// <summary>
        /// 이 Giver가 주는 Item과 줄 수 있는 최대량을 의미하는 ItemStack을 반환
        /// </summary>
        /// <returns> giver로부터 가져갈 수 있는 최대량의 ItemStack </returns>
        ItemStack GetAvailable();
        /// <summary>
        /// Giver로부터 실제로 Item을 특정 개수만큼 가져가서 없앰. 항상 GetAvailable()의 반환값만큼은 가져갈 수 있어야 함.
        /// </summary>
        /// <param name="number"> 가져가는 개수 </param>
        /// <param name="item"> (확인용)가져가는 아이템 종류  </param>
        void TakeItem(int number, Item item);
    }

    public interface IItemReceiver
    {
        /// <summary>
        /// 인수로 주어진 아이템을 받는다. 받는 아이템 중 일부만 받을 수 있으며, 실제로 받은 개수를 반환함.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="number"></param>
        /// <returns> 실제로 받은 양. </returns>
        int GiveItem(Item item, int number);
    }

    public abstract class ItemDragManager : DragManager<IItemGiver, IItemReceiver>
    {
        [SerializeField] private ItemStackView _dragIndicatorPrefab;

        protected override bool CheckBeforeBeginDrag(IDragFrom dragFrom, PointerEventData pointerEventData)
        {
            _draggingItem = dragFrom.Giver.GetAvailable();
            return !_draggingItem.IsEmpty();
        }

        protected override GameObject CreateDragGhost(IDragFrom dragFrom, PointerEventData pointerEvent)
        {
            var made = Object.Instantiate(_dragIndicatorPrefab);
            made.ItemStack = dragFrom.Giver.GetAvailable();
            return made.gameObject;
        }
        protected void TransferItem(IItemGiver from, IItemReceiver to)
        {
            var givable = from.GetAvailable();
            
            // 드래그 시작시와 출발지 아이템이 바뀌었으면 아이템 이동 취소.
            if (givable != _draggingItem)
                return;
            _draggingItem = new ItemStack();

            Assert.IsTrue(!givable.IsEmpty());
            
            var givenCount = to.GiveItem(givable.Item, givable.ItemNum);
            from.TakeItem(givenCount, givable.Item);
        }

        private ItemStack _draggingItem;
    }
}