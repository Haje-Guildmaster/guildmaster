using System;
using GuildMaster.Characters;
using GuildMaster.Databases;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Exploration
{
    public class FooterCharacterInfo : MonoBehaviour
    {
        [SerializeField] private Image _characterIllust;
        [SerializeField] private Text _nameLabel;
        [SerializeField] private Text _hpLabel;
        [SerializeField] private Text _staminaLabel;

        [SerializeField] private Text _atkLabel;
        [SerializeField] private Text _defLabel;
        [SerializeField] private Text _agiLabel;
        [SerializeField] private Text _characteristicLabel;

        private void OnDestroy() => Unsubscribe();


        public void SetCharacter(Character character)
        {
            Unsubscribe();
            _character = character;
            if (_character != null)
                _character.Changed += UpdateAppearance;
            UpdateAppearance();
        }


        private void Unsubscribe()
        {
            if (_character != null)
                _character.Changed -= UpdateAppearance;
        }


        private void UpdateAppearance()
        {
            if (_character != null)
            {
                _characterIllust.sprite = _character.StaticData.BasicData.Illustration;
                _nameLabel.text = _character.UsingName;
                _hpLabel.text = $"HP: {_character.Hp}/{_character.MaxHp}";
                _staminaLabel.text = $"STM: {_character.Stamina}/{_character.MaxStamina}";
                _atkLabel.text = $"ATK: {_character.Atk}";
                _defLabel.text = $"DEF: {_character.Def}";
                _agiLabel.text = $"AGI: {_character.Agi}";
                _characteristicLabel.text = $"[{_character.CharacteristicName}]";
            }
            else
            {
                _characterIllust.sprite = null;
                _nameLabel.text = "";
                _hpLabel.text = "";
                _staminaLabel.text = "";
                _atkLabel.text = "";
                _defLabel.text = "";
                _agiLabel.text = "";
                //_characteristicLabel.text = "";
            }
        }

        private Character _character;
    }
}