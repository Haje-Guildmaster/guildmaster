using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Characters;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 탐색 씬 밑부분의, 캐릭터 상태를 보여주는 공간의 유니티 오브젝트입니다.
    /// </summary>
    public class Footer: MonoBehaviour
    {
        private void Awake()
        {
            _footerCharacterInfos = GetComponentsInChildren<FooterCharacterInfo>().ToList();
        }

        
        /// <summary>
        /// 보여줄 캐릭터들을 설정합니다. 한번 Setup이 불리고 나면 Character 안의 값이 바뀔 때
        /// 자동적으로 footer ui에도 반영이 될 것입니다.
        /// </summary>
        /// <param name="characters"> 캐릭터 목록 </param>
        public void Setup(IEnumerable<Character> characters)
        {
            foreach (var fci in _footerCharacterInfos)
                fci.SetCharacter(null);
            foreach (var (ch, i) in characters.Select((c,i)=>(c,i)))
            {
                _footerCharacterInfos[i].SetCharacter(ch);
            }
        }
        
        
        public IEnumerable<FooterCharacterInfo> FooterCharacterInfos => _footerCharacterInfos;
        private List<FooterCharacterInfo> _footerCharacterInfos;
    }
}