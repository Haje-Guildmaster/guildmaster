using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public abstract class DragManager<TGiver, TReceiver> : MonoBehaviour
    {
        [SerializeField] private Transform DragGhostParent;
        
        public interface IDragFrom
        {
            event Action<PointerEventData> Dragged;
            event Action<PointerEventData> BeganDrag;
            event Action<PointerEventData> EndedDrag;

            void IndicateBeingDragged(bool doIndicate);

            TGiver Giver { get; }
        }

        public interface IDropIn
        {
            event Action<PointerEventData> PointerEntered;
            event Action<PointerEventData> PointerExited;

            void Highlight(bool doHighlight);
            TReceiver Receiver { get; }
        }

        private class DragFromEventListenerRecord
        {
            public Action<PointerEventData> DraggedListener;
            public Action<PointerEventData> BeganDragListener;
            public Action<PointerEventData> EndedDragListener;
        }

        private class DropInEventListenerRecord
        {
            public Action<PointerEventData> PointerEnteredListener;
            public Action<PointerEventData> PointerExitedListener;
        }

        /// <summary>
        /// 이 DragManager가 관리하게 되는, 드래그를 시작할 수 있는 모든 물체를 반환합니다.
        /// </summary>
        /// <returns></returns>
        protected abstract List<IDragFrom> GetAllDragFroms();

        /// <summary>
        /// 이 DragManager가 관리하게 되는, 드래그를 받을 수 있는 물체들을 반환합니다.
        /// </summary>
        /// <returns></returns>
        protected abstract List<IDropIn> GetAllDropIns();

        /// <summary>
        /// 드래그가 성공적으로 끝났을 때 그 결과를 처리합니다.
        /// </summary>
        /// <param name="dragStart"> 드래그가 시작된 DragFrom </param>
        /// <param name="dragEnd"> 드래그가 끝난 DropIn </param>
        protected abstract void ProcessDragResult(IDragFrom dragStart, IDropIn dragEnd);

        /// <summary>
        /// 지정한 dragFrom에서 dropIn까지 드래그가 가능한지 판단하는 predicate. 어떤 물체를 드래그하려 시도하려 할 때,
        /// 이 함수의 결과에 따라 각각이 dropIn들이 highlight되는지, 드래그를 받는지 결정됩니다.
        /// </summary>
        /// <param name="dragStart"></param>
        /// <param name="dragEnd"></param>
        /// <returns> </returns>
        protected abstract bool IsDragAble(IDragFrom dragStart, IDropIn dragEnd);
        
        protected virtual void Awake()
        {
        }

        protected virtual void OnEnable()
        {
            // 이벤트 구독.
            var dragFroms = GetAllDragFroms();
            _dragFromWithListenerList = dragFroms.Select(df =>
                {
                    var dfCapture = df;
                    var record = new DragFromEventListenerRecord
                    {
                        DraggedListener = ped => OnElementDrag(dfCapture, ped),
                        BeganDragListener = ped => OnBeginElementDrag(dfCapture, ped),
                        EndedDragListener = ped => OnEndElementDrag(dfCapture, ped),
                    };
                    df.Dragged += record.DraggedListener;
                    df.BeganDrag += record.BeganDragListener;
                    df.EndedDrag += record.EndedDragListener;
                    return (df, record);
                }
            ).ToList();

            var dropIns = GetAllDropIns();
            _dropInWithListenerList = dropIns.Select(di => {
                var diCapture = di;
                var record = new DropInEventListenerRecord
                {
                    PointerEnteredListener = ped => OnPointerEnterDropIn(diCapture, ped),
                    PointerExitedListener = ped => OnPointerExitDropIn(diCapture, ped),
                };
                di.PointerEntered += record.PointerEnteredListener;
                di.PointerExited -= record.PointerExitedListener;
                return (di, record);
            }).ToList();
        }

        protected virtual void OnDisable()
        {
            // 이벤트 구독 해제.
            foreach (var (df, record) in _dragFromWithListenerList)
            {
                df.Dragged -= record.DraggedListener;
                df.BeganDrag -= record.BeganDragListener;
                df.EndedDrag -= record.EndedDragListener;
            }

            foreach (var (di, record) in _dropInWithListenerList)
            {
                di.PointerEntered -= record.PointerEnteredListener;
                di.PointerExited -= record.PointerExitedListener;
            }
        }

        /// <summary>
        /// 마우스로 주어진 dragFrom으로부터 드래그를 시작했을 때 한 번 불리며, 드래그를 진행해야 하는지 여부를 반환합니다.
        /// false가 반환된다면 DragManager는 아무 일도 하지 않아 드래그가 진행되지 않습니다.
        /// </summary>
        /// <param name="dragFrom"> 현재 드래그가 시작된 dragFrom </param>
        /// <returns> 드래그를 진행해야 하는지 여부</returns>
        protected virtual bool CheckBeforeBeginDrag(IDragFrom dragFrom, PointerEventData pointerEventData)
        {
            return true;
        }

        private void OnBeginElementDrag(IDragFrom dragFrom, PointerEventData pointerEventData)
        {
            Assert.IsTrue(_currentDragging == null);
            if (!CheckBeforeBeginDrag(dragFrom, pointerEventData))
                return;
            
            dragFrom.IndicateBeingDragged(true);
            _currentDragIndicator = CreateDragGhost(dragFrom, pointerEventData);
            _currentDragIndicator.transform.SetParent(DragGhostParent, false);
            _currentDragIndicator.transform.position = pointerEventData.position;   
            
            foreach (var di in DropIns.Where(a => IsDragAble(dragFrom, a)))
            {
                di.Highlight(true);
            }
            
            _currentDragging = dragFrom;
        }

        private void OnElementDrag(IDragFrom dragFrom, PointerEventData pointerEventData)
        {
            if (_currentDragging != dragFrom) return;
            _currentDragIndicator.transform.position = pointerEventData.position;
        }

        private void OnEndElementDrag(IDragFrom dragFrom, PointerEventData pointerEventData)
        {
            if (_currentDragging != dragFrom) return;

            var dropIn = GetCurrentlySelectedDropIn();
            if (dropIn != null)
            {
                // drag&drop 성공시.

                ProcessDragResult(_currentDragging, dropIn);
            }

            // 후처리
            foreach (var di in DropIns)
            {
                di.Highlight(false);
            }
            _currentDragging.IndicateBeingDragged(false);
            _currentDragging = null;
            GameObject.Destroy(_currentDragIndicator);
            _currentDragIndicator = null;
        }

        private void OnPointerEnterDropIn(IDropIn dropIn, PointerEventData pointerEventData)
        {
            _dropInsUnderPointer.Add(dropIn);
        }

        private void OnPointerExitDropIn(IDropIn dropIn, PointerEventData pointerEventData)
        {
            _dropInsUnderPointer.RemoveAll(a => a == dropIn);
        }

        private IDropIn GetCurrentlySelectedDropIn()
        {
            if (_currentDragging == null) return null;
            return _dropInsUnderPointer.FindLast(di=>IsDragAble(_currentDragging, di));
        }
        
        /// <summary>
        /// 드래그 시 마우스를 따라가는 'ghost'를 생성함. raycast를 받지 않아야 함에 주의해 주세요. 
        /// </summary>
        /// <returns></returns>
        protected abstract GameObject CreateDragGhost(IDragFrom dragFrom, PointerEventData pointerEvent);


        private GameObject _currentDragIndicator; // 'ghost'
        private IDragFrom _currentDragging;
        private List<(IDragFrom, DragFromEventListenerRecord)> _dragFromWithListenerList;
        private IEnumerable<IDragFrom> DragFroms => _dragFromWithListenerList.Select(a => a.Item1);
        private List<(IDropIn, DropInEventListenerRecord)> _dropInWithListenerList;
        private IEnumerable<IDropIn> DropIns => _dropInWithListenerList.Select(a => a.Item1);
        private readonly List<IDropIn> _dropInsUnderPointer = new List<IDropIn>();
    }
}