using System;
using System.Linq;
using GuildMaster.Characters;
using GuildMaster.Databases;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 캐릭터 한 명의 정보를 보여줍니다.
    /// Todo: Footer에 종속된 클래스가 아니므로 이름에서 Footer빼기
    /// </summary>
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
                
                string TraitString(Character character)
                {
                    return string.Join(" ", character.ActiveTraits
                        .Select(TraitDatabase.Get)
                        .Select(tsd => $"[{tsd.Name}]"));
                }
                _characteristicLabel.text = TraitString(_character);
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
                _characteristicLabel.text = "";
            }
        }

        private Character _character;
    }
}