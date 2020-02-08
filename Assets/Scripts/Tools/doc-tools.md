# Tools
(last edited: 2020-02-08)\
이곳저곳에서 쓰이는 객체들을 넣습니다.

#### GenericButton\<T\>
유니티 UI.Button과 비슷하게 클릭하면 이벤트를 발생시키는 컴포넌트이나,발생시키는 이벤트에 T 타입의
인수를 같이 보냅니다. 즉 `public UnityEvent<T> clicked`를 제공합니다.
이 클래스를 상속하는 객체들은 각각 `protected abstract T EventArgument { get; }`를 구현해 이벤트에
어떤 값을 보낼 지 결정합니다.

#### Persistent
이 컴포넌트가 든 오브젝트는 scene이 바뀌어도 사라지지 않습니다.
