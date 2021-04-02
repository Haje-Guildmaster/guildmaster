using System;
using GuildMaster.Characters;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class AutoRefreshedCharacterView : MonoBehaviour
    {
        [SerializeField] private ObjectWith<IValueView<string>> _nameView;
        [SerializeField] private ObjectWith<IValueView<Tuple<int, int>>> _hpPairView;   // (hp, maxHp)
        [SerializeField] private ObjectWith<IValueView<Tuple<int, int>>> _staminaPairView; // (stamina, maxStamina)
        [SerializeField] private ObjectWith<IValueView<Character.CharacterStat>> _statView;
        [SerializeField] private ObjectWith<IValueView<Sprite>> _illustView;
        [SerializeField] private ObjectWith<IValueView<Box<int>>> _loyaltyView;

        private void OnDestroy()
        {
            if (_character != null)
                _character.Changed -= Refresh;
        }

        public void SetCharacter(Character character)
        {
            var prevCharacter = _character;
            _character = character;

            if (prevCharacter != null)
                prevCharacter.Changed -= Refresh;
            if (_character != null)
                _character.Changed += Refresh;
            Refresh();
        }
        
        private void Refresh()
        {
            _nameView.Object?.SetValue(_character?.UsingName ?? "");
            _hpPairView.Object?.SetValue(new Tuple<int, int>(_character?.Hp ?? 0, _character?.MaxHp ?? 0));
            _staminaPairView.Object?.SetValue(new Tuple<int, int>(_character?.Stamina ?? 0, _character?.MaxStamina ?? 0));
            _statView.Object?.SetValue(_character?.GetStat());
            _illustView.Object?.SetValue(_character?.StaticData.BasicData.Illustration);
            _loyaltyView.Object?.SetValue(_character?.Loyalty ?? 0);
        }


        private Character _character;
    }
}