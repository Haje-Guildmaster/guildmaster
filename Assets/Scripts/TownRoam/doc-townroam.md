# TownRoam
(last edited: 2021-01-11)

기존 코드가 너무 장황해 갈아엎었습니다.

TownRoamManager가 마을 탐색을 총괄합니다.

Town 안에 여러 Place들이 있고, 각각의 Place들은 또 여러 Room 들을 가지고 있습니다. \
(Place가 따로 클래스로 있지는 않습니다. enum PlaceName으로 구분만 할 뿐입니다) \
Place에는 각각 Entrance Room이 있습니다.. 그 Place를 갈 때 Entrance에 맨 처음 가게 됩니다.

RoomViewer가 현재 플레이어가 있는 Room을 비춥니다. 동시에 레이어를 조정하여 다른 Room들이 보이지 않게 합니다.

