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
        private int reducedmaxhp;           //부상으로 줄어든 최대체력
        [SerializeField] private int maxhp;
        [SerializeField] private bool spIsMp;
        [SerializeField] private int sp;
        [SerializeField] private int maxsp;

        public int HP
        {
            get { return hp; }
            set
            {
                if (reducedmaxhp < value) { hp = reducedmaxhp; }
                else if (value < 0) { hp = 0; }
                else { hp = value; }
            }
        }
        public int ReducedMaxHP
        {
            get { return reducedmaxhp; }
            set
            {
                if (maxhp < value) { reducedmaxhp = maxhp; }
                else if (value < 0) { reducedmaxhp = 0; }
                else { reducedmaxhp = value; }
            }
        }
        public int MaxHP
        {
            get { return maxhp; }
            set { maxhp = value; }
        }
        public int SP
        {
            get { return sp; }
            set
            {
                if (maxsp < value) { sp = maxsp; }
                else if (value < 0) { sp = 0; }
                else { sp = value; }
            }
        }
        public int MaxSP
        {
            get { return maxsp; }
            set { maxsp = value; }
        }
        public bool SpIsMp => spIsMp;

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