# TownRoam
(last edited: 2020-02-08)

마을 맵을 불러오고 맵에서 돌아다니게 해 주는 코드들, 맵에 있는 객체들(npc, 건물 등),
그리고 그 객체들과 상호작용하게 해 주는 코드들입니다.

## 외부
외부코드에서는 밑의 두 클래스만 알면 되도록 했습니다.

#### TownRefs
유니티쪽 데이터를 C#으로 가져오고 또 외부에 노출시키기 위한 싱글톤 컴포넌트입니다.
여러 Town들의 Prefab을 담고 있습니다.

#### TownLoadManager
마을 맵(TownRoamScene)을 열 때 사용하는 static 클래스입니다.
유니티 `SceneManager.LoadScene`과 비슷하게 `LoadTownScene(Town town)` 함수를 제공합니다.
옵션을 같이 넣는 `LoadTownScene<T, T2>(T town, Option<T2> option) where T: T2 where T2: Town`
함수도 있으나 현재까지는 사용되지 않습니다.\
유니티 GUILayout의 함수 구조랑 비슷하게 해 보려고 했습니다. 

    TownLoadManager.LoadTownScene(TownRefs.TestTown, TownLoadManager.UseDefault());
처럼 사용하게 하는 것이 목표입니다.

## 내부
### 장소 저장
#### Place
장소를 나타내는 컴포넌트입니다. 이름과 배경을 그리는 SpriteRenderer가 존재하고 사이즈와 위치를 제공합니다.
#### PlaceButton
어떤 장소로 이동하는 버튼이라는 것을 나타내는 컴포넌트. 클릭했을 시 이어진 PlaceButton을 인수로
ClickedEvent를 발생시킴. 이 이벤트에 PlaceMoveButton이 리스너를 붙이게 됩니다.

#### Town
Town을 상속한 자녀들이 각각 하나의 맵을 나타내는 컴포넌트가 되며, 그 맵에 존재하는 Place들에 접근할 수 있게 해 줍니다.\
Place간의 연결 정보는 따로 저장하지 않습니다.

### 맵 안에서의 이동/상호작용
#### PlaceViewer
카메라와 같은 오브젝트에 붙어 카메라를 움직이고 크기를 조절하고 또한 레이어를 이용해 다른 Place가 보이지
않도록 합니다. 맵에 있는 클릭 가능한 요소들의 클릭도 처리합니다.

### 맵 로딩
맵 로딩의 순서:
1. 어딘가에서 `TownLoadManager.LoadTownScene` 호출.
2. `TownLoadManager`가 로딩을 요청받은 `Town`과 `Option`을 저장하고 TownRoamScene을 염.
3. TownRoamScene에 있는 `TownLoaderComponent`가 `Start` 이벤트에 `TownLoadManager`에 저장된 `Town`을 받아 옴.
4. `TownLoaderComponent`가 `Town`종류와 `Option`에 따라 적용해야 할 `TownModifiers`를 결정. 
5. `TownLoaderComponent`가 static 클래스 `TownObjectLoader`가 제공하는 함수를 이용해
prefab형태의 맵을 실제 scene에 `Instantiate`하고 `TownModifiers`를 적용

`TownModifier`는 그저 특정한 `Town` 을 받아 그걸 변형시키는 함수 `Modify` 가 있는 객체입니다.