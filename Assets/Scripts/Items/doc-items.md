#Items
(last edited: 2020-05-12) \
아이템 정보에 관한 클래스들이 있습니다.\
아마 stat부분은 나중에 많이 수정하게 되지 않을까 합니다.

```
Item
  +                   ItemDatabase
  +-+ ItemCode +---------------------------> ItemStaticData
  |                                           +
  +-+ EquipmentStats: 아이템 스탯              +-+ 이름, 설명, 최대 스택 개수, 이미지, 중요물품인지 여부 등
                                              |
                                              +-+ ItemEffect: 사용시 효과
                                              |
                                              +-+ EquipmentStats defaultEquipmentStats: 아이템 종류에 따른 기본 스탯
```

#### ItemCode
아이템 종류의 고유 코드입니다.

#### Item
아이템 한 종류에 대한 정보를 저장합니다.

#### ItemEffect
아이템 사용시의 효과를 저장합니다.
- DoNothing: 아무것도 일어나지 않음.
