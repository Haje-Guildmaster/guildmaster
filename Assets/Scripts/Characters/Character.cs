using System;
using System.Collections.ObjectModel;
using GuildMaster.Databases;

namespace GuildMaster.Characters
{
    public class Character
    {
        //코드에 표시되어 있지 않다면 수의 하한선은 0입니다.
        public Character(CharacterCode code)
        {
            _code = code;
            _hp = MaxHp;
            _sp = MaxSp;
            Alignment = StaticData.defaultAlignment;
        }
        
        public const int MaxLoyalty = 100;
        
        public string UsingName => NameList[_usingNameIndex];                //현재 이름
        public string RealName => StaticData.basicData.RealName;             //실제 이름
        public bool KnowUseRealName => _usingNameIndex == NameList.Count;    //현재 실제 이름을 사용중인지
        public int MaxSp => StaticData.battleStatData.maxSp;
        public int MaxHp => StaticData.battleStatData.maxHp;     // MaxSp, MaxSp, Hp 등은 후에 캐릭터 종류에 종속된 값이 아니게 될 것이라 판단하여 Character에 넣습니다.


        public float Injury                        // 현재 부상 정도(0~1). 퍼센트로 최대 체력이 깎인다.
        {
            get => _injury;
            set
            {
                _injury = Math.Min(1, Math.Max(value, 0));
                Hp = Hp;
            }
        }

        public int Hp
        {
            get => _hp; 
            set => _hp = Math.Min(CurrentMaxHp, Math.Max(value, 0));
        }
        public int Sp
        {
            get => _sp;
            set => _sp = Math.Min(MaxSp, Math.Max(value, 0));
        }
        public int Loyalty
        {
            get => _loyalty;
            set => _loyalty = Math.Min(MaxLoyalty, Math.Max(value, 0));
        }
        public int CurrentMaxHp => (int)(MaxHp*(1-Injury));

        public int Agi => StaticData.battleStatData.baseAgi;
        public int Atk => StaticData.battleStatData.baseAtk;
        public int Def => StaticData.battleStatData.baseDef;
        public int Int => StaticData.battleStatData.baseInt;

        public CharacterAlignmentData Alignment;
        
        private float _injury = 0;
        private readonly CharacterCode _code;
        private int _loyalty = 0;
        private int _usingNameIndex = 0;                      // 현재 나타날 이름의 index
        private int _hp;
        private int _sp;
        private ReadOnlyCollection<string> NameList => StaticData.basicData.nameList.AsReadOnly(); 
        public CharacterStaticData StaticData => CharacterDatabase.Get(_code);
    }
}