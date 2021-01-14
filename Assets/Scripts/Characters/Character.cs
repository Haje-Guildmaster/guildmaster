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
            _hp.Value = StatMaxHpList[1];
            _stamina.Value = StatMaxStaminaList[1];
        }
        //보정값 안 붙는 애들
        public string UsingName => NameList[_usingNameIndex];                       // 현재 이름
        public string RealName => StaticData.BasicData.RealName;                    // 실제 이름
        public bool KnowUseRealName => _usingNameIndex == NameList.Count;           // 현재 실제 이름을 사용중인지
        public CharacterStaticData StaticData => CharacterDatabase.Get(_code);      // 캐릭터 코드
        public List<Trait> ActiveTraits => StaticData.BasicData.ActiveTraits;       // 캐릭터 특성
        public int MaxLoyalty => StaticData.StatData.MaxLoyalty;                    // 충성도 최대 값 (내부적으로 100)
        public int CurrentLevelupXP => LevelupXP[_level];
        //보정값 안 붙는 애들 private
        private readonly CharacterCode _code;
        private readonly ChangeTrackedValue<int> _usingNameIndex = new ChangeTrackedValue<int>(0); // 현재 나타날 이름의 index
        private readonly ChangeTrackedValue<int> _hp = new ChangeTrackedValue<int>();
        private readonly ChangeTrackedValue<int> _stamina = new ChangeTrackedValue<int>();
        private readonly ChangeTrackedValue<int> _level = new ChangeTrackedValue<int>(1);
        private readonly ChangeTrackedValue<int> _xp = new ChangeTrackedValue<int>(0);
        private readonly ChangeTrackedValue<int> _loyalty = new ChangeTrackedValue<int>(0);
        private ReadOnlyCollection<string> NameList => StaticData.BasicData.NameList.AsReadOnly();
        private ReadOnlyCollection<int> LevelupXP => StaticData.StatData.LevelupXP.AsReadOnly();
        //보정값 붙는 애들 (보정값 붙기 전)
        private ReadOnlyCollection<int> StatMaxHpList => StaticData.StatData.MaxHp.AsReadOnly();
        private ReadOnlyCollection<int> StatMaxStaminaList => StaticData.StatData.MaxStamina.AsReadOnly();
        private ReadOnlyCollection<int> StatStrengthList => StaticData.StatData.Strength.AsReadOnly();
        private ReadOnlyCollection<int> StatTrickList => StaticData.StatData.Trick.AsReadOnly();
        private ReadOnlyCollection<int> StatWisdomList => StaticData.StatData.Wisdom.AsReadOnly();
        //보정값
        private readonly ChangeTrackedValue<int> _hpChange = new ChangeTrackedValue<int>(0);
        private readonly ChangeTrackedValue<int> _staminaChange = new ChangeTrackedValue<int>(0);
        private readonly ChangeTrackedValue<int> _strengthChange = new ChangeTrackedValue<int>(0);
        private readonly ChangeTrackedValue<int> _trickChange = new ChangeTrackedValue<int>(0);
        private readonly ChangeTrackedValue<int> _wisdomChange = new ChangeTrackedValue<int>(0);
        
        public int Hp
        {
            get => _hp.Value; 
            set => _hp.Value = Math.Min(MaxHp, Math.Max(value, 0));
        }
        public int HpChange
        {
            get => _hpChange.Value;
            set => _hpChange.Value = value;
        }
        public int MaxHp => Math.Max(1, StatMaxHpList[_level] + _hpChange.Value);
        public int Stamina
        {
            get => _stamina.Value;
            set => _stamina.Value = Math.Min(MaxStamina, Math.Max(value, 0));
        }
        public int StaminaChange
        {
            get => _staminaChange.Value;
            set => _staminaChange.Value = value;
        }
        public int MaxStamina => Math.Max(1, StatMaxStaminaList[_level] + _staminaChange.Value);

        public int Strength => Math.Max(StrengthChange + StatStrengthList[_level], 0);
        public int StrengthChange
        {
            get => _strengthChange.Value;
            set => _strengthChange.Value = value;
        }
        public int Trick => Math.Max(TrickChange + StatTrickList[_level], 0);
        public int TrickChange
        {
            get => _trickChange.Value;
            set => _trickChange.Value = value;
        }
        public int Wisdom => Math.Max(WisdomChange + StatWisdomList[_level], 0);
        public int WisdomChange
        {
            get => _wisdomChange.Value;
            set => _wisdomChange.Value = value;
        }
        public int Level
        {
            get => _level.Value;
            private set
            {
                if (value < 1) return;
                _level.Value = value;                
            }
        }
        public int XP
        {
            get => _xp.Value;
            set
            {
                if (value >= LevelupXP[_level])
                {
                    _xp.Value = value - LevelupXP[_level];
                    Level += 1;
                }
                else _xp.Value = Math.Min(0, value);
            }
        }
        public int Loyalty  
        {
            get => (_loyalty.Value + 9) / 10;
            set => _loyalty.Value = Math.Min(MaxLoyalty, Math.Max(value, 0));
        }
    }
}