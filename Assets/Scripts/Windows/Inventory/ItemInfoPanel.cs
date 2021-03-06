using GuildMaster.Databases;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows.Inven
{
    public class ItemInfoPanel : MonoBehaviour
    {
        public Image itemImage;
        public Text nameLabel;
        public Text descriptionLabel;

        private void Update()
        {
            JumpToMouse();
        }

        public int Open(ItemCode itemCode)
        {
            gameObject.SetActive(true);
            JumpToMouse();
            var itemData = ItemDatabase.Get(itemCode);
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