# Conditions
(last edited: 2020-02-08)

#### Condition
조건을 나타내는 클래스입니다.
- Always(bool isTrue): 항상 참이거나 항상 거짓.
- LevelOver(int level): 플레이어 레벨이 몇 이상.
- CompletedQuest(QuestData quest): 퀘스트를 클리어 했는지.
- And(params Condition[] list): 리스트의 모든 값이 참이면 참.
- Or(params Condition[] list): 리스트의 하나라도 참이면 참.

