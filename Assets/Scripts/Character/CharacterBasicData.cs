using System;
using System.Collections.Generic;
using UnityEngine;


namespace GuildMaster.Characters
{
    [Serializable]
    public struct CharacterBasicData
    {
        //코드에 표시되어 있지 않다면 수의 하한선은 0입니다.

        [SerializeField] private int usingNameIndex;    //현재 나타날 이름의 index
        [SerializeField] private List<string> nameList; //이름의 리스트 (가장 마지막 것이 진짜이름)

        public string UsingName => nameList[usingNameIndex];                //현재 이름
        public string RealName => nameList[nameList.Count - 1];             //실제 이름
        public bool KnowUseRealName => usingNameIndex == nameList.Count;    //현재 실제 이름을 사용중인지

        [SerializeField] private int hp;
        [SerializeField] private int maxhp;
        [SerializeField] private int mp;
        [SerializeField] private int maxmp;
        [SerializeField] private int dp;
        [SerializeField] private int maxdp;

        public int HP
        {
            get { return hp; }
            set
            {
                if (maxhp < value) { hp = maxhp; }
                else if (value < 0) { hp = 0; }
                else { hp = value; }
            }
        }
        public int MaxHP
        {
            get { return maxhp; }
            set { maxhp = value; }
        }
        public int MP
        {
            get { return mp; }
            set
            {
                if (maxmp < value) { mp = maxmp; }
                else if (value < 0) { mp = 0; }
                else { mp = value; }
            }
        }
        public int MaxMP
        {
            get { return maxmp; }
            set { maxmp = value; }
        }
        public int DP
        {
            get { return dp; }
            set
            {
                if (maxdp < value) { dp = maxdp; }
                else if (value < 0) { dp = 0; }
                else { dp = value; }
            }
        }
        public int MaxDP
        {
            get { return maxdp; }
            set { maxdp = value; }
        }

        [SerializeField] private int loyalty;
        [SerializeField] private const int maxLoyalty = 100;

        public int Loyalty
        {
            get { return loyalty; }
            set
            {
                if (value < 0) { loyalty = 0; }
                else if (maxLoyalty < value) { loyalty = maxLoyalty; }
                else { loyalty = value; }
            }
        }


    }
}