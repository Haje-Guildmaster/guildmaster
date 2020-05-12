#Data
(last edited: 2020-05-12)

#### Player
게임을 플레이하는 사람의 모든 정보, 즉 세이브를 했을 때 저장되는 모든 데이터를 담으며, 그 데이터들을
편집하거나 받아올 수 있는 함수들을 제공합니다.
(퀘스트 클리어 정보, 길드원들, 레벨, 장비, etc...)\
싱글톤 오브젝트로 `Player.Instance`로 접근할 수 있습니다.\
`QuestManager`, `Inventory`, `InGameEventManager`를 포함합니다.\
조건을 체크할 수 있는 `public bool CheckCondition(Condition condition)`
을 제공합니다.

#### Inventory
플레이어가 소유한 아이템에 관한 정보를 담습니다.

#### Condition
조건을 나타내는 클래스입니다.
- Always(bool isTrue): 항상 참이거나 항상 거짓.
- GuildLevelOver(int level): 길드 레벨이 몇 이상.
- CompletedQuest(QuestData quest): 퀘스트를 클리어 했는지.
- And(params Condition[] list): 리스트의 모든 값이 참이면 참.
- Or(params Condition[] list): 리스트의 하나라도 참이면 참.
