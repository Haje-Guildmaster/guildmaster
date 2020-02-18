# Quests
(last edited: 2020-02-18)\
퀘스트 데이터를 저장하고 처리하는 객체들입니다. 이쪽 구조는 별로 마음에 안 들지만
마음에 들 때까지 할려면 한세월 걸릴 것 같으니 문제가 되면 그때 바꾸겠습니다.

QuestData는 유니티 ScriptableObject으로 퀘스트 자체에 대한 정보를 저장합니다.\
Quest는 받은  퀘스트 하나의 진행상황을 저장합니다. 일종의 QuestData의 Iterator입니다.

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
반환합니다.
- `public List<QuestData> GetAvailableQuestsFrom(IEnumerable<QuestData> quests)`\
주어진 리스트에서 지금 받을 수 있는 퀘스트들만 골라 반환합니다. 이것과 위의 것은 모두
npc쪽 코드에서 퀘스트 처리를 위해 사용합니다.

또한 QuestManager는 다음 이벤트들을 구독하고 있습니다.
- `NpcInteractWindow.QuestScriptPlayEnd`

마지막으로, 현재 퀘스트 정보가 바뀌었을 경우 Changed 이벤트를 일으킵니다.


#### QuestData
어떤 한 퀘스트의 (정적인) 정보를 저장합니다. 이에는 이름, 설명, 받을 수 있는 상태가 되는 조건,
그리고 퀘스트의 단계들이 포함됩니다. 퀘스트 단계는 QuestStep 객체를 이용해 표현됩니다.
##### QuestStep
퀘스트의 한 단계로, 조건(Condition 클래스)과 미션(말 걸기, 몬스터 n마리 잡기 등등)으로 이루어져 있습니다. 다만 지금은 조건을 만족해야
미션을 수행할 수 있는 걸로 했는데 생각해 보니 동시에 하도록 바꿔야겠네요.
##### StepMission
어떤 일을 수행하라는 미션을 나타내는 클래스입니다. 현재는 TalkMission밖에 없습니다.

#### Quest
플레이어가 받은 퀘스트입니다. QuestData, 현재 Step 번호와 미션 진행도를 저장합니다.