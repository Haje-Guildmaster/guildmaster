# Quests
(last edited: 2020-02-18)\
퀘스트 데이터를 저장하고 처리하는 객체들입니다. 현재 문제가 되는 부분이 좀 있으므로 후에 본격적으로 퀘스트 관련 작업을 할 때 수정할 예정입니다.
```
Quest
  +                        QuestDatabase
  +-+ QuestCode+----------------------------------------->QuestStaticData
  |                                                       +
  +-+ NpcCode client                                      +-+ 이름, 설명  등.
  |                                                       |
  +-+ StepIndex: 퀘스트의 현재 단계.                        +-+ Condition activationCodintion: 퀘스트를 받을 수 있는 조건
  |                                                       |
  +-+ int[] _progresses: 현재 단계의 미션들의 진행도         +-+ List<Reward> rewards: 보상 목록.
                                                          |
                                                          +-+ List<QuestStep> steps: 퀘스트의 각 단계들
                                                                       +
                                                                       |
                                                                       |
       +---------------------------------------------------------------+
       |
       v
   QuestStep
     +
     +-+ Condition stepCondition: 단계를 완료하기 위한 조건
     |
     +-+ List<StepMission> _stepMissions
```
####Quest
받은  퀘스트 하나의 진행상황을 저장합니다. 일종의 QuestData의 Iterator입니다.

#### QuestManager
외부에서 퀘스트 정보를 받아오거나 수정하기 위해 접근하는 싱글톤 오브젝트입니다.
- `public bool ReceiveQuest(QuestData questData, NpcData client)`, `public bool AbandonQuest(ReadOnlyQuest quest)`\
퀘스트를 받거나 포기합니다.
- `public bool CompletedQuest(QuestData questData)`, `public bool DoingQuest(QuestData questData)`\
퀘스트를 진행중인지, 예전에 끝냈는지 확인합니다.
- `public ReadOnlyCollection<ReadonlyQuest> CurrentQuests()`\
현재 진행중인 퀘스트를 제공합니다.
- `public List<StepMission.TalkMission> GetCompletableTalkMissions(NpcData npcData)`\
현재 진행하고 있는 퀘스트 중에, `npcData`와 대화하는 미션이 수행 가능할 경우 경우 그런 퀘스트들을 모두 
반환합니다. Npc와 대화를 시도할 때 때 퀘스트 텍스트를 출력하여야 하는지 판단하기 위해 사용됩니다.
- `public List<QuestData> GetAvailableQuestsFrom(IEnumerable<QuestData> quests)`\
주어진 리스트에서 지금 받을 수 있는 퀘스트들만 골라 반환합니다. 
즉, npc가 주는 모든 퀘스트 목록을 입력하면 npc한테서 받을 수 있는 퀘스트 목록을 반환합니다.

또한 QuestManager는 다음 이벤트들을 구독하고 있습니다.
- `NpcInteractWindow.QuestScriptPlayEnd`\
대응하는 TalkMission의 진행도를 추가합니다.

마지막으로, 현재 퀘스트 정보가 바뀌었을 경우 Changed 이벤트를 일으킵니다.

##### QuestStep
퀘스트의 한 단계로, 조건(Condition 클래스)과 미션(말 걸기, 몬스터 n마리 잡기 등등)으로 이루어져 있습니다.

##### StepMission
어떤 일을 수행하라는 미션을 나타내는 클래스입니다. 
 - TalkMission\
 특정 npc와 대화하는 미션
 