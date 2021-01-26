using GuildMaster.Characters;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class BasicCharacterStatView : MonoBehaviour, IValueView<Character.CharacterStat>
    {
        [SerializeField] private ObjectWith<IValueView<int>> _atkView;
        [SerializeField] private ObjectWith<IValueView<int>> _defView;
        [SerializeField] private ObjectWith<IValueView<int>> _agiView;
        [SerializeField] private ObjectWith<IValueView<int>> _intView;
        public void SetValue(Character.CharacterStat stat)
        {
            _atkView.Object.SetValue(stat.Atk);
            _defView.Object.SetValue(stat.Def);
            _agiView.Object.SetValue(stat.Agi);
            _intView.Object.SetValue(stat.Int);
        }
    }
}