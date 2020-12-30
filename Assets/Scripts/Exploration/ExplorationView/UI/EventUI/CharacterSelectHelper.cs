using System;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 이벤트 처리시 캐릭터 머리 위에 떠서 선택의 결과를 알려주고 클릭해서 그 캐릭터를 선택할 수 있게 하는 오브젝트
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class CharacterSelectHelper : MonoBehaviour
    {
        [SerializeField] private Text _label;

        public Button.ButtonClickedEvent OnClick => _button.onClick;
        public string Text
        {
            get => _label.text;
            set => _label.text = value;
        }

        public bool ButtonEnabled
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }
        
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        
        private Button _button;
    }
}