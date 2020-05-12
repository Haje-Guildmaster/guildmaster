# UI
(last edited: 2020-05-12)\
게임의 UI를 처리하는 코드들을 넣습니다. 현재는 다수의 Window들만을 이용해 UI가 구성됩니다.

class hierachy
```
Window
        DraggableWindow
                CharacterInspectWindow
                GuildInspectWindow
                InGameEventWindow
                InventoryWindow
                MessageBox
                QuestInspectWindow
                QuestListWindow
                QuestSuggestWindow
        NpcInteractWindow
```
#### Window
모든 윈도우들의 부모입니다. 이 클래스를 상속한 자식은 `protected void OpenWindow()`를 통해 자기
자신을 열 수 있습니다. `public void Close()`를 제공하며 윈도우를 클릭할 시 transform의 형제들 중 제일 앞에 보이도록 합니다.

#### DraggableWindow
드래그 가능한 윈도우입니다.

#### IToggleableWindow
이 인터페이스를 상속한 윈도우가 `void Open()`을 구현하면 그 윈도우에 대해 `public void Toggle()`을 사용할 수 있습니다.

#### UiWindowManager
여러 UI요소에 접근할 수 있도록 해 주는 싱글톤 오브젝트입니다.

#### etc
나머지는 실제 윈도우를 표현하는 클래스이거나 그의 구성요소입니다. 대부분 코드에서 얻은 정보를 실제 게임에 보여주는
비슷비슷한 구조를 가지므로 자세한 설명은 생략하겠습니다.\
새로운 윈도우를 만들고 싶다면 일관성을 위해 `public void Open(...)`을 구현해 주세요.
