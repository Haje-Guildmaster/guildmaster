using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Characters;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Exploration
{
    public class Footer: MonoBehaviour
    {
        private void Awake()
        {
            _footerCharacterInfos = GetComponentsInChildren<FooterCharacterInfo>().ToList();
        }

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