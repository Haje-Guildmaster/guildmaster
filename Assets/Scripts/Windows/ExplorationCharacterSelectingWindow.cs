using System.Collections.Generic;
using System.Linq;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Databases;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class ExplorationCharacterSelectingWindow : DraggableWindow, IToggleableWindow
    {
        [SerializeField] private Transform characterSelectingListParent;
        [SerializeField] private Toggle characterSelectingTogglePrefab;
        [SerializeField] private ToggleGroup characterSelectingToggleGroup;
        [SerializeField] private Transform characterSelectedListParent;
        [SerializeField] private Toggle characterSelectedTogglePrefab;
        [SerializeField] private ToggleGroup characterSelectedToggleGroup;
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
        [SerializeField] private Text CharacteristicLabel;

        public List<Character> exploreCharacterList = new List<Character>();

        public void OpenNext()
        {
            base.Close();
            UiWindowsManager.Instance.ExplorationItemSelectingWindow.Open();
            //ExplorationLoader.Instance.Load(exploreCharacterList); //Asd가 구현중인 기능 에러 뜨는게 정상
        }

        public void Open()
        {
            base.OpenWindow();
            if (first) Set_allCharacters();
            RefreshList();
        }
        public void SwitchList()
        {
            if (_allCharacters.Contains(_currentCharacter))
            {
                if (exploreCharacterList.Count == 4) return;
                _allCharacters.Remove(_currentCharacter);
                exploreCharacterList.Add(_currentCharacter);
                RefreshList();
            }
            else if (exploreCharacterList.Contains(_currentCharacter))
            {
                exploreCharacterList.Remove(_currentCharacter);
                _allCharacters.Add(_currentCharacter);
                RefreshList();
            }
        }

        private void RefreshList()
        {
            SetCharacter(null);
            foreach (Transform t in characterSelectingListParent)
                Destroy(t.gameObject);
            foreach (var (ch, i) in _allCharacters.Select((i, j) =>
                (i, j)))
            {
                var made = Instantiate(characterSelectingTogglePrefab, characterSelectingListParent);
                made.group = characterSelectingToggleGroup;
                made.GetComponentInChildren<Text>().text = ch.UsingName;
                var capture = ch;
                made.onValueChanged.AddListener(b =>
                {
                    if (b) SetCharacter(capture);
                });
                if (i == 0)
                    made.isOn = false; //위의 AddListener와 순서 주의.
            }
            foreach (Transform t in characterSelectedListParent)
                Destroy(t.gameObject);
            foreach (var (ch, i) in exploreCharacterList.Select((i, j) =>
                (i, j)))
            {
                var made = Instantiate(characterSelectedTogglePrefab, characterSelectedListParent);
                made.group = characterSelectedToggleGroup;
                made.GetComponentInChildren<Text>().text = ch.UsingName;
                var capture = ch;
                made.onValueChanged.AddListener(b =>
                {
                    if (b) SetCharacter(capture);
                });
                if (i == 0)
                    made.isOn = false; //위의 AddListener와 순서 주의.
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
            maxHpLabel.text = $"{_currentCharacter.Hp}/{_currentCharacter.MaxHp}";
            maxSpValueLabel.text = $"{_currentCharacter.Stamina}/{_currentCharacter.MaxStamina}";
            atkLabel.text = _currentCharacter.Strength.ToString();
            defLabel.text = _currentCharacter.Strength.ToString();
            agiLabel.text = _currentCharacter.Trick.ToString();
            intLabel.text = _currentCharacter.Wisdom.ToString();
            //유니티 상에서의 수정 필요
        }

        private void Set_allCharacters()
        {
            first = false;
            foreach (var (ch, i) in Player.Instance.PlayerGuild._guildMembers.GuildMemberList.Select((i, j) =>
                (i, j)))
            {
                _allCharacters.Add(ch);
            }
        }

        private Character _currentCharacter;
        private bool first = true;
        private List<Character> _allCharacters = new List<Character>();
    }
}