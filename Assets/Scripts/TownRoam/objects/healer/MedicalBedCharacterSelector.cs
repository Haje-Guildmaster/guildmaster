using System.Collections.Generic;
using System.Linq;
using GuildMaster.Characters;
using GuildMaster.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GuildMaster.TownRoam
{
    public class MedicalBedCharacterSelector : MonoBehaviour
    {
        [SerializeField] private int _bedIndex;
        [SerializeField] private Dropdown _dropdown;

        private void OnEnable()
        {
            _dropdown.onValueChanged.AddListener(SetCharacter);
        }

        private void OnDisable()
        {
            _dropdown.onValueChanged.RemoveListener(SetCharacter);
        }


        public void OpenCharacterSelectDropdown(Vector2 position)
        {
            var medicalBed = Player.Instance.MedicalBed;
            _playerListCapture = new List<Character>(GetCharacterCandidates());
            _waitingSelect = true;

            _dropdown.options = _playerListCapture.Select(ch => new Dropdown.OptionData(ch.UsingName)).ToList();
            var initialIndex = _playerListCapture.FindIndex(ch => ch == medicalBed.OnBeds[_bedIndex]);
            if (initialIndex < 0)
            {
                _dropdown.options.Add(new Dropdown.OptionData("None"));
                var size = _dropdown.options.Count;
                _dropdown.SetValueWithoutNotify(size - 1);
                _dropdown.options.RemoveAt(size - 1);
            }
            else
            {
                _dropdown.SetValueWithoutNotify(initialIndex);
            }

            Assert.IsTrue(Camera.main != null);
            var prevPos = _dropdown.template.position;
            _dropdown.template.position =
                Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, prevPos.z));
            _dropdown.Show();
        }

        private void SetCharacter(int index)
        {
            if (!_waitingSelect) return;
            _waitingSelect = false;

            var selectedCharacter = _playerListCapture[index];
            if (selectedCharacter != GetCharacterCandidates()[index])
            {
                Debug.LogWarning($"이 드롭다운을 열었을 때와 플레이어 목록이 달라진 것으로 보입니다. 이 경우 이 클래스는 아무 행동도 하지 않습니다.");
                return;
            }

            Player.Instance.MedicalBed.PutCharacter(_bedIndex, selectedCharacter);
        }

        private List<Character> GetCharacterCandidates()
        {
            return Player.Instance.PlayerGuild._guildMembers.GuildMemberList;
        }

        private bool _waitingSelect = false;
        private List<Character> _playerListCapture;
    }
}