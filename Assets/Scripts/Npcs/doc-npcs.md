# Npcs
(last edited: 2020-05-12)\
Npc에 대한 정보를 저장합니다.
```
                   NpcDatabase
NpcCode +--------------------------------> NpcStaticData
        |                                    +
        |                                    +-+ NpcBasicData
        |                                    |     +
        |                                    |     +-+ 이름, 이미지 등
        |                                    |
        |                                    +-+ NpcQuestData
        |                                    |     +
        |                                    |     +-+ 이 Npc가 주는 퀘스트 목록
        |                                    |
        |                                    +-+ NpcRoamData
        |                                          +
        |                                          +-+ Npc가 Town에서 어떻게 움직일지, 어디서 나타날지를 결정하는데 필요한 데이터
        |                                              후에 Npc가 출몰/움직이는 규칙이 정해지면 채울 예정.
        |
        |
        |      PlayerData.GetNpcStatus
        +--------------------------------> NpcStatus
                                             +
                                             +-+ Affinity(친밀도)


```


