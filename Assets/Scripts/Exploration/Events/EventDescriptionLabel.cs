using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 이벤트 설명을 띄우는 오브젝트.
    /// </summary>
    public class EventDescriptionLabel: MonoBehaviour
    {
        [SerializeField] private Text _label;

        public string Text
        {
            get => _label.text;
            set => _label.text = value;
        }
    }
}