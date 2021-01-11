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
            void InvokeChanged() => Changed?.Invoke();
            _loyalty.Changed += InvokeChanged;
            _hp.Changed += InvokeChanged;
            _stamina.Changed += InvokeChanged;
            _usingNameIndex.Changed += InvokeChanged;    
            
            _code = code;
            _hp.Value = MaxHp;
            _stamina.Value = MaxStamina;
            _maxhp.Value = StatMaxHp;
            _maxstamina.Value = StatMaxStamina;
            Alignment = StaticData.DefaultAlignment;
        }
        //보정값 안 붙는 애들
        public string UsingName => NameList[_usingNameIndex];                //현재 이름
        public string RealName => StaticData.BasicData.RealName;             //실제 이름
        public bool KnowUseRealName => _usingNameIndex == NameList.Count;    //현재 실제 이름을 사용중인지
        public CharacterStaticData StaticData => CharacterDatabase.Get(_code);
        public CharacterAlignmentData Alignment;
        public List<Trait> ActiveTraits => StaticData.BasicData.ActiveTraits;
        public const int MaxLoyalty = 10; //최대 충성도
        //보정값 붙는 애들 (보정값 붙기 전)
        public int StatMaxHp => StaticData.StatData.MaxHp;     // MaxSp, MaxSp, Hp 등은 후에 캐릭터 종류에 종속된 값이 아니게 될 것이라 판단하여 Character에 넣습니다.
        public int StatMaxStamina => StaticData.StatData.MaxStamina;
        //보정값 붙는 애들 (보정값 붙은 후)
        private ChangeTrackedValue<int> _maxhp = new ChangeTrackedValue<int>();
        private ChangeTrackedValue<int> _maxstamina = new ChangeTrackedValue<int>();
        //private (대부분 현재 값. ex : 현재 체력)
        private readonly CharacterCode _code;
        private readonly ChangeTrackedValue<int> _usingNameIndex = new ChangeTrackedValue<int>(0); // 현재 나타날 이름의 index
        private readonly ChangeTrackedValue<int> _hp = new ChangeTrackedValue<int>();
        private readonly ChangeTrackedValue<int> _stamina = new ChangeTrackedValue<int>();
        private readonly ChangeTrackedValue<int> _loyalty = new ChangeTrackedValue<int>(0);
        private ReadOnlyCollection<string> NameList => StaticData.BasicData.NameList.AsReadOnly();
        //보정값
        private readonly ChangeTrackedValue<int> _hpchange = new ChangeTrackedValue<int>(0);
        private readonly ChangeTrackedValue<int> _staminachange = new ChangeTrackedValue<int>(0);

        public int Hp
        {
            get => _hp; 
            set => _hp.Value = Math.Min(MaxHp, Math.Max(value, 0));
        }
        public int HpChange
        {
            get => _hpchange;
            set 
            {
                _hpchange.Value += value;
                MaxHp = Math.Max(1, StatMaxHp + _hpchange);
            }
        }
        public int MaxHp
        {
            get => _maxhp;
            private set
            {
                _maxhp.Value = value;
            }
        }
        public int Stamina
        {
            get => _stamina;
            set => _stamina.Value = Math.Min(MaxStamina, Math.Max(value, 0));
        }
        public int StaminaChange
        {
            get => _staminachange;
            set
            {
                _staminachange.Value += value;
                Stamina = Stamina;
            }
        }
        public int MaxStamina
        {
            get => _maxstamina;
            private set
            {
                _maxstamina.Value = value;
            }

        }
        public int Loyalty
        {
            get => _loyalty;
            set => _loyalty.Value = Math.Min(MaxLoyalty, Math.Max(value, 0));
        }
    }
}