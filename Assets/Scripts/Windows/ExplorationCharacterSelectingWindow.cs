using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Databases;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class ExplorationCharacterSelectingWindow : DraggableWindow
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
        
        public class Response
        {
            public enum ActionEnum
            {
                Cancel,
                GoNext
            }

            public ActionEnum NextAction;
            public List<Character> SelectedCharacters;
        }
        
        public void OpenNext()
        {
            _responseTaskCompletionSource.TrySetResult(
                new Response
                {
                    NextAction = Response.ActionEnum.GoNext,
                    SelectedCharacters = exploreCharacterList.ToList(),
                });
            Close();
        }

        protected override void OnClose()
        {
            _responseTaskCompletionSource.TrySetResult(
                new Response
                {
                    NextAction = Response.ActionEnum.Cancel,
                    SelectedCharacters = null,
                });
        }
        
        public async Task<Response> GetResponse()
        {
            // 마지막 GetResponse가 안 끝났으면 취소시키기
            _responseTaskCompletionSource.TrySetResult(
                new Response
                {
                    NextAction = Response.ActionEnum.Cancel,
                    SelectedCharacters = null,
                });
            
            // 윈도우 초기 화면
            base.OpenWindow();
            Set_allCharacters();
            RefreshList();
            
            // 입력 기다림.
            _responseTaskCompletionSource = new TaskCompletionSource<Response>(); 
            return await _responseTaskCompletionSource.Task;
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
            maxSpLeftLabel.text = (sd.BattleStatData.SpIsMp ? "MP" : "DP") + ":";
            maxSpValueLabel.text = $"{_currentCharacter.Sp}/{_currentCharacter.MaxSp}";
            atkLabel.text = _currentCharacter.Atk.ToString();
            defLabel.text = _currentCharacter.Def.ToString();
            agiLabel.text = _currentCharacter.Agi.ToString();
            intLabel.text = _currentCharacter.Int.ToString();
        }

        private void Set_allCharacters()
        {
            _allCharacters = Player.Instance.PlayerGuild._guildMembers.GuildMemberList.ToList();
        }

        private Character _currentCharacter;
        private List<Character> _allCharacters;
        private TaskCompletionSource<Response> _responseTaskCompletionSource = new TaskCompletionSource<Response>();
    }
}