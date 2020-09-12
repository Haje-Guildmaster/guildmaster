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

        //재우 코드 인용
        //public static StatusToolTip Instance => _instance != null ? _instance : (_instance = FindObjectOfType<StatusToolTip>());

        public Text nameLabel;
        public Text descriptionLabel;

        /*private string[] statusName = new string[] { "이름", "충성도", "HP", "DP", "ATK", "DEF", "AGI", "INT" };
        private string[] statusInfo = new string[]
        {
            null,
            "길드에 충성하는 정도를 나타냅니다.",
            "길드원의 남은 체력을 나타냅니다. 0이 되면 사망합니다.",
            "스테미너",
            "공격 시 데미지 수치",
            "피격 시 방어 수치",
            "피격 시 회피 및 공격 시 적중 수치, 전투 이외 이벤트 대처에 영향",
            "마법 공격력 수치, 전투 이외 이벤트 대처에 영향"
        };*/
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
        /*public static void showToolTipStatic(string tooltipname, string tooltipstring)
        {
            _instance.showToolTip(tooltipname, tooltipstring);
        }
        public static void hideToolTipStatic()
        {
            _instance.hideToolTip();
        }*/
        /*public int Open(ItemCode itemCode)
        {
            gameObject.SetActive(true);
            JumpToMouse();
            //var itemData = ItemDatabase.Get(itemCode);
            nameLabel.text = itemData.ItemName;
            descriptionLabel.text = itemData.ItemDescription;

            return ++_currentRequestId;
        }

        public void Close(int requestId)
        {
            if (requestId == _currentRequestId)
                gameObject.SetActive(false);
        }
        */
        private void JumpToMouse()
        {
            transform.position = Input.mousePosition;
        }
        private int _currentRequestId;

    }
}