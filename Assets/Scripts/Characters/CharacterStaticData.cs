using System;

namespace GuildMaster.Characters
{
    [Serializable]
    public class CharacterStaticData
    {
        // 일단 이벤트에서 스테이터스 바뀌는거 만들어달라고 해서 임시로 대충 만들었습니다 추후 많이 바꿀 예정
        // 기획적인 것들이 좀더 나와야 될듯
        public CharacterBasicData basicData;
        public CharacterBattleStatData battleStatData;
        public CharacterAlignmentData defaultAlignment; //성향치
    }
}