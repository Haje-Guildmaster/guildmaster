using System.Collections.Generic;
using System.Linq;
using GuildMaster.Characters;
using GuildMaster.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class CharacterInspectWindow : DraggableWindow, IToggleableWindow
    {
        [SerializeField] private Transform characterListParent;
        [SerializeField] private Toggle characterSelectTogglePrefab;
        [SerializeField] private ToggleGroup characterSelectToggleGroup;
        [SerializeField] private Image characterIllustration;
        [SerializeField] private Text nameLabel;
        [SerializeField] private Text loyaltyLabel;
        [SerializeField] private Text maxHpLabel;
        [SerializeField] private Text maxSpLeftLabel;
        [SerializeField] private Text maxSpValueLabel;
        [SerializeField] private Text atkLabel;
        [SerializeField] private Text defLabel;
        [SerializeField] private Text agiLabel;
        [SerializeField] private Text intLabel;


        public void Open()
        {
            base.OpenWindow();

            SetCharacter(null);
            foreach (Transform t in characterListParent)
                Destroy(t.gameObject);
            foreach (var (ch, i) in Player.Instance.PlayerGuild._guildMembers.GuildMemberList.Select((i, j) =>
                (i, j)))
            {
                var made = Instantiate(characterSelectTogglePrefab, characterListParent);
                made.group = characterSelectToggleGroup;
                made.GetComponentInChildren<Text>().text = ch.UsingName;
                var capture = ch;
                made.onValueChanged.AddListener(b =>
                {
                    if (b) SetCharacter(capture);
                });
                if (i == 0)
                    made.isOn = true; //위의 AddListener와 순서 주의.
            }
        }

        private void SetCharacter(Character character)
        {
            _currentCharacter = character;
            Refresh();
        }

        private void Refresh()
        {
            if (_currentCharacter == null)
            {
                // Todo: 뭔가 선택된 캐릭터가 없는 경우의 화면 보여주기.
                nameLabel.text = "선택된 캐릭터 없음.";
                return;
            }

            var sd = _currentCharacter.StaticData;
            characterIllustration.sprite = sd.basicData.illustration;
            nameLabel.text = _currentCharacter.UsingName;
            loyaltyLabel.text = _currentCharacter.Loyalty.ToString();
            maxHpLabel.text = $"{_currentCharacter.Hp}/{_currentCharacter.MaxHp}";
            maxSpLeftLabel.text = (sd.battleStatData.spIsMp ? "MP" : "DP") + ":";
            maxSpValueLabel.text = $"{_currentCharacter.Sp}/{_currentCharacter.MaxSp}";
            atkLabel.text = _currentCharacter.Atk.ToString();
            defLabel.text = _currentCharacter.Def.ToString();
            agiLabel.text = _currentCharacter.Agi.ToString();
            intLabel.text = _currentCharacter.Int.ToString();
        }

        private Character _currentCharacter;
    }
}