using UnityEngine;
using System;

namespace GuildMaster.Characters
{
    [Serializable]
    public struct CharacterBasicData
    {
        public string usingName;        //현재 이름
        public string realName;         //실제 이름
        public string fakeName;         //숨겨진 이름 (숨겨진 이름이 있다면 ""로)  <- 이름 관련된건 기획 나오는거 보고 수정
        public bool knowUseRealName;    //현재 실제 이름을 사용중인지
        public int HP;                  //체력
        public int MaxHP;               //최대체력
        public int MP;                  //마나
        public int MaxMP;               //최대마나
        public int DP;                  //신성력
        public int MaxDP;               //최대신성력
    }
}