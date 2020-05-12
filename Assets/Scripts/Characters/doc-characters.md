#Characters
(last edited: 2020-05-12) \
게임의 캐릭터들에 대한 객체를 둡니다.

```
 Character
   +                      CharacterDatabase
   +---+CharacterCode +------------------------>  CharacterStaticData
   |                                                +
   +-+usingNameIndex, Hp, Sp, injury 등 현재상태.    +----+CharacterBasicData
   |                                                |        +
   +-+ CharacterAlignmentData                       |        +-+ 이름, 이미지 등 기본 정보
         +                                          |
         |                                          +----+CharacterBattleStatData
         +-+ 현재 성향치                             |        +
                                                    |        |
                                                    |        +-+ 공격력, 체력과 같은 스탯들
                                                    |
                                                    |
                                                    +----+CharacterAlignmentData defaultAlignment
                                                             +
                                                             |
                                                             +-+ 기본 성향치

```