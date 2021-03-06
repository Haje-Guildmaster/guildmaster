using GuildMaster.Databases;
using GuildMaster.Items;
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

        public int Open(Item item)
        {
            gameObject.SetActive(true);
            JumpToMouse();
            var staticData = item.StaticData;
            itemImage.sprite = staticData.ItemImage;
            nameLabel.text = staticData.ItemName;
            descriptionLabel.text = staticData.ItemDescription;

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