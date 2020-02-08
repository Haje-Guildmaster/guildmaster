#Data
(last edited: 2020-02-08)

#### PlayerData
게임을 플레이하는 사람의 모든 정보, 즉 세이브를 했을 때 저장되는 모든 데이터를 담으며, 그 데이터들을
편집하거나 받아올 수 있는 함수들을 제공합니다.
(퀘스트 클리어 정보, 길드원들, 레벨, 장비, etc...)\
싱글톤 오브젝트로 `PlayerData.Instance`로 접근할 수 있습니다.\
`QuestManager`를 포함합니다.\
조건을 체크할 수 있는 `public bool CheckCondition(Condition condition)`
을 제공합니다.
