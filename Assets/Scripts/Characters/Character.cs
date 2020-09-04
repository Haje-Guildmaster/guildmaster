using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using GuildMaster.Databases;
using GuildMaster.Tools;

namespace GuildMaster.Characters
{
    public class Character
    {
        public event Action Changed; 
        
        //코드에 표시되어 있지 않다면 수의 하한선은 0입니다.
        public Character(CharacterCode code)
        {
            _injury.Changed += Changed;
            _loyalty.Changed += Changed;
            _hp.Changed += Changed;
            _sp.Changed += Changed;
            _stamina.Changed += Changed;
            _usingNameIndex.Changed += Changed;
            _characteristicIndex.Changed += Changed;
            
            _code = code;
            _hp.Value = MaxHp;
            _sp.Value = MaxSp;
            _stamina.Value = MaxStamina;
            Alignment = StaticData.DefaultAlignment;
        }
        
        public const int MaxLoyalty = 100;

        
        public string UsingName => NameList[_usingNameIndex];                //현재 이름
        public string RealName => StaticData.BasicData.RealName;             //실제 이름
        public bool KnowUseRealName => _usingNameIndex == NameList.Count;    //현재 실제 이름을 사용중인지
        public string CharacteristicName;
        public string CharacteristicContents;
        public int MaxSp => StaticData.BattleStatData.MaxSp;
        public int MaxHp => StaticData.BattleStatData.MaxHp;     // MaxSp, MaxSp, Hp 등은 후에 캐릭터 종류에 종속된 값이 아니게 될 것이라 판단하여 Character에 넣습니다.
        public int MaxStamina => StaticData.BattleStatData.MaxStamina;

        public string TraitText()
        {
            string _str = "";
            List<CharacterTraitData.Trait> _activeTraits = StaticData.BasicData.ActiveTraits;
            for(int i = 0; i < _activeTraits.Count-1; i++)
            {
                _str += StaticData.TraitData.GetName(_activeTraits[i]);
                _str += "\n";
                _str += StaticData.TraitData.GetDescription(_activeTraits[i]);
            }
            return _str;
        }

        public float Injury                        // 현재 부상 정도(0~1). 퍼센트로 최대 체력이 깎인다.
        {
            get => _injury;
            set
            {
                _injury.Value = Math.Min(1, Math.Max(value, 0));
                Hp = Hp;
            }
        }

        public int Hp
        {
            get => _hp; 
            set => _hp.Value = Math.Min(CurrentMaxHp, Math.Max(value, 0));
        }
        public int Sp
        {
            get => _sp;
            set => _sp.Value = Math.Min(MaxSp, Math.Max(value, 0));
        }

        public int Stamina
        {
            get => _stamina;
            set => _stamina.Value = Math.Min(MaxSp, Math.Max(value, 0));
        }
        
        public int Loyalty
        {
            get => _loyalty;
            set => _loyalty.Value = Math.Min(MaxLoyalty, Math.Max(value, 0));
        }

        public int CurrentMaxHp => (int)(MaxHp*(1-Injury));

        public int Agi => StaticData.BattleStatData.BaseAgi;
        public int Atk => StaticData.BattleStatData.BaseAtk;
        public int Def => StaticData.BattleStatData.BaseDef;
        public int Int => StaticData.BattleStatData.BaseInt;

        public CharacterAlignmentData Alignment;

        private readonly CharacterCode _code;
        private readonly ChangeTrackedValue<float> _injury = new ChangeTrackedValue<float>(0);
        private readonly ChangeTrackedValue<int> _loyalty = new ChangeTrackedValue<int>(0);
        private readonly ChangeTrackedValue<int> _usingNameIndex = new ChangeTrackedValue<int>(0);                      // 현재 나타날 이름의 index
        private readonly ChangeTrackedValue<int> _hp = new ChangeTrackedValue<int>();
        private readonly ChangeTrackedValue<int> _sp = new ChangeTrackedValue<int>();
        private readonly ChangeTrackedValue<int> _stamina = new ChangeTrackedValue<int>();
        private readonly ChangeTrackedValue<int> _characteristicIndex = new ChangeTrackedValue<int>(0);                      // 현재 나타날 이름의 index
        private ReadOnlyCollection<string> NameList => StaticData.BasicData.NameList.AsReadOnly();
        public CharacterStaticData StaticData => CharacterDatabase.Get(_code);
    }
}