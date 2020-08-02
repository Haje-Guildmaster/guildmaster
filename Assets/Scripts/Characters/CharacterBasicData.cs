using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;


namespace GuildMaster.Characters
{
    [Serializable]
    public class CharacterBasicData
    {
        //코드에 표시되어 있지 않다면 수의 하한선은 0입니다.

        public Sprite illustration;
        public List<string> nameList; //이름의 리스트 (가장 마지막 것이 진짜이름)
        public string RealName => nameList.ElementAtOrDefault(nameList.Count - 1);
    }
}