using System;
using GuildMaster.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    [RequireComponent(typeof(Text))]
    public class ValueViewLabel: MonoBehaviour, IValueView<object>
    {
        private void Awake()
        {
            _label = GetComponent<Text>();
        }

        public void SetValue(object obj)
        {
            _label.text = obj?.ToString() ?? "";
        }

        private Text _label;
    }
}