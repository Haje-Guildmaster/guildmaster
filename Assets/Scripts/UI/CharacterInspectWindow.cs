using System.Collections.Generic;
using System.Linq;
using GuildMaster.Characters;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class CharacterInspectWindow: DraggableWindow
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
        
        // for debugging
        [SerializeField] private List<CharacterData> data;

        protected override void OnOpen()
        {
            SetCharacter(null);
            foreach (Transform t in characterListParent)
                Destroy(t.gameObject);
            foreach (var (cd,i) in data.Select((i,j)=>(i,j)))
            {
                var made = Instantiate(characterSelectTogglePrefab, characterListParent);
                made.group = characterSelectToggleGroup;
                made.GetComponentInChildren<Text>().text = cd.basicData.UsingName;
                var capture = cd;
                made.onValueChanged.AddListener(b => {if (b) SetCharacter(capture); });
                if (i == 0)
                    made.isOn = true;    //위의 AddListener와 순서 주의.
            }
        }

        private void SetCharacter(CharacterData character)
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
            // characterIllustration.sprite = ???
            var bd = _currentCharacter.basicData;
            nameLabel.text = bd.UsingName;
            loyaltyLabel.text = bd.Loyalty.ToString();
            maxHpLabel.text = $"{bd.HP}/{bd.MaxHP}";
            maxSpLeftLabel.text = (bd.SpIsMp ? "MP" : "DP") + ":";
            maxSpValueLabel.text = $"{bd.SP}/{bd.MaxSP}";
            // atkLabel.text = _currentCharacter.battleStatData.ATK private이네요?
        }

        private CharacterData _currentCharacter;
    }
}