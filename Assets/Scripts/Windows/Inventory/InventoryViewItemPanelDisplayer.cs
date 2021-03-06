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