using UnityEngine;
using UnityEngine.Assertions;


namespace GuildMaster.TownRoam
{
    /// <summary>
    /// 장소 하나를 나타내는 유니티 오브젝트.
    /// </summary>
    public class Room : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _backgroundRenderer;
        [field: SerializeField] public PlaceName BelongingPlace { get; private set; }

        public Vector2 Size => _backgroundRenderer.bounds.size;
        public Vector2 Center => _backgroundRenderer.transform.position;
        // 일단 임시로 backgroundRenderer에서 모든 값을 가져오도록 했습니다.
        // 추후에 직접 값 입력도 할 수 있도록 해야 하지 않을까 합니다.

        private void Awake()
        {
            Assert.IsTrue(BelongingPlace != PlaceName.None);
        }
    }
}