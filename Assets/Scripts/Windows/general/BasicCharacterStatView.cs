using GuildMaster.Characters;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace GuildMaster.Windows
{
    public class BasicCharacterStatView : MonoBehaviour, IValueView<Character.CharacterStat>
    {
        [FormerlySerializedAs("_atkView")] [SerializeField] private ObjectWith<IValueView<Box<int>>> _strengthView;
        [FormerlySerializedAs("_defView")] [SerializeField] private ObjectWith<IValueView<Box<int>>> _trickView;
        [FormerlySerializedAs("_agiView")] [SerializeField] private ObjectWith<IValueView<Box<int>>> _wisdomView;
        public void SetValue(Character.CharacterStat stat)
        {
            _strengthView.Object?.SetValue(stat?.Strength ?? 0);
            _trickView.Object?.SetValue(stat?.Trick ?? 0);
            _wisdomView.Object?.SetValue(stat?.Wisdom ?? 0);
        }
    }
}