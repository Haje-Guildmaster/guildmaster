using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Exploration.Events
{
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