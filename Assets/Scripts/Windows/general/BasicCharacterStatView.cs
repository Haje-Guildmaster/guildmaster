using GuildMaster.Characters;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class BasicCharacterStatView : MonoBehaviour, IValueView<Character.CharacterStat>
    {
        [SerializeField] private ObjectWith<IValueView<Box<int>>> _atkView;
        [SerializeField] private ObjectWith<IValueView<Box<int>>> _defView;
        [SerializeField] private ObjectWith<IValueView<Box<int>>> _agiView;
        [SerializeField] private ObjectWith<IValueView<Box<int>>> _intView;
        public void SetValue(Character.CharacterStat stat)
        {
            _atkView.Object?.SetValue(stat?.Atk ?? 0);
            _defView.Object?.SetValue(stat?.Def ?? 0);
            _agiView.Object?.SetValue(stat?.Agi ?? 0);
            _intView.Object?.SetValue(stat?.Int ?? 0);
        }
    }
}