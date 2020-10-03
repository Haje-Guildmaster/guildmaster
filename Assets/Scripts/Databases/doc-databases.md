#Database
(last edited: 2020-09-25) \
길마의 데이터들은 대체로 두 가지로 나눠서 관리합니다. 게임이 진행되는 동안 바뀔 수 있는 데이터들(현재 체력 등)과 바뀌지 않는 값들
(레벨에 따른 최대 체력 값 등). 이 중 뒤쪽을 ???StaticData(CharacterStaticData, NpcStaticData 등)라는 클래스에 저장하고 Database 클래스를 이용해 접근합니다.\
간단히 말하면 Database는 StaticData들의 배열이고, 싱글톤으로 접근할 수 있습니다.\
DB의 간편한 편집을 위해 ScriptableObject로 만들었으며 유니티 Inspector로 편집할 수 있습니다.

#### 사용
AADatabase의 값은 AADatabase.Index로 접근할 수 있습니다.
AADatabase.Get(index) <- 이렇게 값을 가지고 오면 됩니다.

#### 기타
EnumDatabase는 인덱스로 enum을 씁니다.


#### 목록
- CharacterDatabase
- ItemDatabase
- NpcDatabase
- QuestDatabase
- EventSeedDatabase
- ExplorationLocationDatabase
- TraitDatabase