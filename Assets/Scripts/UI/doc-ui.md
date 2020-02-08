# UI
(last edited: 2020-02-08)\
게임의 UI를 처리하는 코드들을 넣습니다. 현재는 다수의 Window들만을 이용해 UI가 구성됩니다.

##외부
###UIWindowsManager
다른 코드에서 어떤 창을 열기 위해 접근하는 싱글톤 객체입니다.
UIWindowsManager.Instance로 접근하여 제공하는 함수들을 이용해 여러 창들을 열거나 닫을 수 있습니다.
현재 제공되는 함수들은
- **OpenNpcInteractWindow**\
npc를 클릭하였을 시 나오는, npc와 여러 상호작용이 가능한 창입니다.
- **OpenQuestSuggestWindow**\
npc가 퀘스트를 부탁할 시에 뜨는 퀘스트의 이름과 간단한 설명,
그리고 수락/거절 버튼이 있는 창입니다.
- **OpenQuestListWindow**\
퀘스트 목록 창입니다.
- **ToggleQuestListWindow**\
퀘스트 목록 창을 토글(열려있으면 닫고 닫혀있으면 열기)합니다.

##내부

모든 창의 부모인 Window가 있으며, 이를 상속한 DraggableWindow가 있습니다.
실제 창들은 모두 이 두 클래스 중 하나를 상속해서 만듭니다.
### Window
Open, Close, Toggle이라는 세 함수를 제공합니다. 작동 방식은 단순히 GameObject를 활성화/비활성화
하는 방식을 사용합니다. Toggle은 퍼블릭 변수인 enableToggle이 참일 때만 사용할 수 있으며,
창이 열려 있으면 Close를, 닫혀 있으면 Open을 호출합니다.\
Window의 자식은 OnOpen과 OnClose이라는 이벤트 함수를 오버라이드할 수 있습니다. 불리는 때는 이름 그대로.\
또한 IPointerDownHandler을 상속하여 마우스 클릭 이벤트를 받고, 클릭 시에 자신을 부모의 제일 마지막 
자식으로 설정합니다. 따라서 UI draw 순서가 밀려 형제들 중 자신이 제일 위에 보이게 합니다.

### DraggableWindow
말 그대로 드래그 가능한 창입니다.

### 기타 클래스들
다른 파일들은 각각 위의 두 클래스들을 상속해 만든 창들이거나,
그런 창들에서 쓰이는 요소(버튼이나 리스트의 항목)들입니다.\
