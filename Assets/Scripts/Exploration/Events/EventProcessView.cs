using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GuildMaster.Characters;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 이벤트 처리를 유저에게 보여주는 역할의 유니티 오브젝트입니다.
    /// </summary>
    public class EventProcessView : MonoBehaviour
    {
        [SerializeField] private ScrollPicker _decisionSelector;
        [SerializeField] private CharacterSelectHelper _characterSelectHelperPrefab;
        [SerializeField] private ChoiceSelectorElement _choiceSelectorElementPrefab;
        [SerializeField] private Transform _characterSelectHelperParent;
        [SerializeField] private EventDescriptionLabel _eventDescriptionLabel;
        [SerializeField] private Transform CharacterSelectorYIndicator; // CharacterSelector 가 생성될 y 위치 지정. 


        private void Awake()
        {
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            _characterSelectHelperParent.gameObject.SetActive(active);
            _eventDescriptionLabel.gameObject.SetActive(active);
            _decisionSelector.gameObject.SetActive(active);
        }

        /// <summary>
        /// 선택지 하나를 유저에게 보여주기 위해 필요한 모든 데이터를 담은 클래스.
        /// </summary>
        public class ChoiceVisualData
        {
            public string Description;
            public List<(Character character, bool activated, string description)> CharacterSelectHelperStrings;
        }

        /// <summary>
        /// 이벤트에서 선택 가능한 선택지들을 보여주고 유저의 선택을 기다려 반환합니다.
        /// </summary>
        /// <note>
        /// 한번에 2개 이상 실행되면 안됩니다. (딱히 가드나 체크를 두지는 않았습니다)
        /// </note>
        /// <param name="characterSprites"> 캐릭터 오브젝트들 </param>
        /// <param name="choices"> 선택지 목록 </param>
        /// <param name="descriptionString"> 이벤트의 설명 텍스트 </param>
        /// <param name="initialChoiceIndex"> 맨 처음 선택되어 있어야 하는 </param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// Todo: SetChoices랑 WaitDecision으로 나누기 고려
        /// Todo: 함수 이름 수정 고려.
        public async Task<(int choiceIndex, Character selectedCharacter)> WaitUserDecision(
            IEnumerable<CharacterSprite> characterSprites, List<ChoiceVisualData> choices, string descriptionString,
            int initialChoiceIndex = 0)
        {
            Assert.IsTrue(choices.Count > 0); // 적어도 하나의 선택지는 있어야 함.
            // 시간복잡도는 무시한 코드이므로 속도 문제가 있으면 수정할 것.

            var cnt = 0;
            // 초기화.
            foreach (Transform child in _decisionSelector.transform)
            {
                cnt++;
                Destroy(child.gameObject);
            }

            // 설명 설정.
            _eventDescriptionLabel.Text = descriptionString;

            // 선택 버튼 생성.
            foreach (var (cvd, i) in choices.Select((val, i) => (val, i)))
            {
                var made = Instantiate(_choiceSelectorElementPrefab, _decisionSelector.transform);
                Assert.IsTrue(made.transform.GetSiblingIndex() == i + cnt);
                made.DescriptionLabel.text = cvd.Description;
            }


            // 이벤트 끝내기 함수 설정. (캐릭터를 선택하였을 경우 불리는 놈)
            var tcs = new TaskCompletionSource<(int choiceIndex, Character selectedCharacter)>();

            void EventEndSignal(int choiceIndex, Character character)
            {
                tcs.TrySetResult((choiceIndex, character));
            }

            // 캐릭터 선택 도우미(캐릭터 머리 위에 뜨는 그거) 생성
            var characterSelectors = new List<(CharacterSprite sprite, CharacterSelectHelper selectHelper)>();


            foreach (var cs in characterSprites)
            {
                var made = Instantiate(_characterSelectHelperPrefab, _characterSelectHelperParent);
                var csCapture = cs;
                made.OnClick.AddListener(() =>
                    EventEndSignal(_decisionSelector.SelectedIndex,
                        csCapture.Character)); // 클릭했을 시 이벤트 종료 신호를 보냄. 캐릭터 반환.
                made.transform.position =
                    new Vector3(cs.transform.position.x, CharacterSelectorYIndicator.position.y); // 위치 조정.
                characterSelectors.Add((cs, made));
            }

            // 현재 올려둔 선택지가 바뀌었을 때 캐릭터 선택 도우미의 모습 업데이트.
            void UpdateCharacterSelectHelpers(int i)
            {
                Assert.IsTrue(choices.Count > i);
                foreach (var (sprite, selectHelper) in characterSelectors)
                {
                    var (_, activated, description) = choices[i].CharacterSelectHelperStrings
                        .Find(tup => tup.character == sprite.Character);
                    selectHelper.ButtonEnabled = activated;
                    selectHelper.Text = activated ? description : "";
                }
            }
            
            
            _decisionSelector.SelectingChange += UpdateCharacterSelectHelpers;
            _decisionSelector.SetSelectedIndex(initialChoiceIndex, false);
            
            // 선택기 선택된 것 초기화. 동시에 UpdateCharacterSelectHelpers가 불리게 될 것.
            var result = await tcs.Task; // 캐릭터를 눌러서 이벤트가 종료되기를 기다림.

            _decisionSelector.SelectingChange -= UpdateCharacterSelectHelpers;

            // 나가기 전 정리.
            foreach (Transform child in _decisionSelector.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var selector in characterSelectors)
            {
                Destroy(selector.selectHelper.gameObject);
            }

            return result;
        }

        private List<CharacterSelectHelper> _selectorList = new List<CharacterSelectHelper>();
    }
}