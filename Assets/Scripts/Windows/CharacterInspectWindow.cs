using System.Linq;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Databases;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class CharacterInspectWindow : DraggableWindow, IToggleableWindow
    {
        [SerializeField] private Transform characterListParent;
        [SerializeField] private Toggle characterSelectTogglePrefab;
        [SerializeField] private ToggleGroup characterSelectToggleGroup;
        [SerializeField] private Image characterIllustration;
        [SerializeField] private Text nameLabel;
        [SerializeField] private Text loyaltyLabel;
        [SerializeField] private Text CharacteristicLabel;
        [SerializeField] private Text HpLabel;
        [SerializeField] private Text StaminaLabel;
        [SerializeField] private Text StrengthLabel;
        [SerializeField] private Text TrickLabel;
        [SerializeField] private Text WisdomLabel;


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
                made.GetComponentInChildren<Text>().text = ch.UsingName;    //Todo: GetComponent 대체.
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
            Unsubscribe();
            _currentCharacter = character;
            if(_currentCharacter != null)
                _currentCharacter.Changed += Refresh;
            Refresh();
        }

        private void Unsubscribe()
        {
            if (_currentCharacter != null)
                _currentCharacter.Changed -= Refresh;
        }

        private void OnDestroy() => Unsubscribe();

        private void Refresh()
        {
            if (_currentCharacter == null)
            {
                // Todo: 뭔가 선택된 캐릭터가 없는 경우의 화면 보여주기.
                nameLabel.text = "선택된 캐릭터 없음.";
                return;
            }

            var sd = _currentCharacter.StaticData;
            characterIllustration.sprite = sd.BasicData.Illustration;
            nameLabel.text = _currentCharacter.UsingName;
            loyaltyLabel.text = _currentCharacter.Loyalty.ToString();
            
            string TraitText(Character character)
            {
                return string.Join("\n", character.ActiveTraits
                    .Select(TraitDatabase.Get)
                    .Select(tsd => $"[{tsd.Name}]\n{tsd.Description}"));
            }
            CharacteristicLabel.text = TraitText(_currentCharacter);
            HpLabel.text = $"{_currentCharacter.Hp}/{_currentCharacter.MaxHp}";
            StaminaLabel.text = $"{_currentCharacter.Stamina}/{_currentCharacter.MaxStamina}";
            StrengthLabel.text = _currentCharacter.Strength.ToString();
            TrickLabel.text = _currentCharacter.Trick.ToString();
            WisdomLabel.text = _currentCharacter.Wisdom.ToString();
        }

        private Character _currentCharacter;
    }
}