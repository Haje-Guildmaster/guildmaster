using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class FormattingValueView : MonoBehaviour, IValueView<ITuple>, IValueView<IEnumerable<object>>,
        IValueView<object>
    {
        [SerializeField] private ObjectWith<IValueView<string>> _outputView;
        [SerializeField] private string _formatString;

        public void SetValue(ITuple tuple)
        {
            SetValue(Enumerable.Range(0, tuple.Length).Select(i => tuple[i]));
        }


        public void SetValue(object value)
        {
            SetValue(new []{value});
        }

        public void SetValue(IEnumerable<object> enumerable)
        {
            string formattedString = String.Format(_formatString, enumerable.ToArray());
            _outputView.Object.SetValue(formattedString);
        }
    }
}