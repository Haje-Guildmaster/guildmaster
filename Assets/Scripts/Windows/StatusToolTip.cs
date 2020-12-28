using GuildMaster.Databases;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class StatusToolTip : MonoBehaviour
    {
        public static StatusToolTip Instance
        {
            get;
            private set;
        }

        public Text nameLabel;
        public Text descriptionLabel;
        private void Update()
        {
            JumpToMouse();
        }
        private void Awake()
        {
            //toDo
            Instance = this;
            gameObject.SetActive(false);
        }
        public void showToolTip(string tooltipname,string tooltipstring)
        {
            gameObject.SetActive(true);

            nameLabel.text = tooltipname;
            descriptionLabel.text = tooltipstring;

        }

        public void hideToolTip()
        {
            gameObject.SetActive(false);
        }
        private void JumpToMouse()
        {
            transform.position = Input.mousePosition;
        }
        private int _currentRequestId;

    }
}