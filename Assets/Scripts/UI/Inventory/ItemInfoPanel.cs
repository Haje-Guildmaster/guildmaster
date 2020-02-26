using System;
using GuildMaster.Items;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI.Inventory
{
    public class ItemInfoPanel: MonoBehaviour
    {
        public Image itemImage;
        public Text nameLabel;
        public Text descriptionLabel;

        private void Update()
        {
            JumpToMouse();
        }
        
        public int Open(Item.ItemCode itemCode)
        {
            gameObject.SetActive(true);
            JumpToMouse();
            var itemData = ItemDatabase.Instance.GetItemStaticData(itemCode);
            itemImage.sprite = itemData.ItemImage;
            nameLabel.text = itemData.ItemName;
            descriptionLabel.text = itemData.ItemDescription;

            return ++_currentRequestId;
        }

        public void Close(int requestId)
        {
            if (requestId == _currentRequestId)
                gameObject.SetActive(false);
        }

        private void JumpToMouse()
        {
            transform.position = Input.mousePosition;
        }
        private int _currentRequestId;
    }
}