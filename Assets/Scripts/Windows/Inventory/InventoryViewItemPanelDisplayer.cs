using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class InventoryViewItemPanelDisplayer : MonoBehaviour
    {
        [SerializeField] private AutoRefreshedInventoryView _targetInventoryView;

        
        // Todo: InventoryView disabled 될 때 panel도 꺼지게 해야 하나, 현재 이 오브젝트도 같이 disabled되면서 CloseAllPanel이 불려 패널이 꺼지므로 냅둠.;
        private void OnEnable()
        {
            _targetInventoryView.PointerEnteredItemIcon += OnPointerEnteredItemIcon;
            _targetInventoryView.PointerExitedItemIcon += OnPointerExitedItemIcon;
        }

        private void OnDisable()
        {
            _targetInventoryView.PointerEnteredItemIcon -= OnPointerEnteredItemIcon;
            _targetInventoryView.PointerExitedItemIcon -= OnPointerExitedItemIcon;
            CloseAllPanel();
        }


        private void OnPointerEnteredItemIcon(int index, PointerEventData pointerEventData)
        {
            ClosePanel(index);
            var itemStack = _targetInventoryView.Inventory.GetItemStack(index);
            if (itemStack.Item == null) return;
            var panelRequestId = UiWindowsManager.Instance.itemInfoPanel.Open(itemStack.Item);
            _requestedPanelIdMap[index] = panelRequestId;
        }

        private void OnPointerExitedItemIcon(int index, PointerEventData pointerEventData)
        {
            ClosePanel(index);
        }

        private void ClosePanel(int itemIndex)
        {
            if (!_requestedPanelIdMap.ContainsKey(itemIndex)) return;
            UiWindowsManager.Instance.itemInfoPanel.Close(_requestedPanelIdMap[itemIndex]);
            _requestedPanelIdMap.Remove(itemIndex);
        }

        private void CloseAllPanel()
        {
            foreach (var requestId in _requestedPanelIdMap.Values)
                UiWindowsManager.Instance.itemInfoPanel.Close(requestId);
            _requestedPanelIdMap.Clear();
        }

        private readonly Dictionary<int, int>
            _requestedPanelIdMap = new Dictionary<int, int>(); // key: index, value: panelRequestId
    }
}