using System;
using GuildMaster.Databases;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;
    
    /// <summary>
    /// 지도에서 어떤 한 위치를 선택할 수 있도록, 한 위치를 나타내는 오브젝트
    /// 비UI쪽에서 UI쪽으로 바꾸면서 약간 코드가 꼬였습니다. 재사용이 많지 않을 것 같아 굳이 개선하지 않고 둡니다. 
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(BasicMapLocationSprite))]
    public class LocationButton: MonoBehaviour, INodeRepresentative<MapNode>
    {
        public event Action<LocationButton> Clicked;
        
        [SerializeField] private Sprite _baseLocationSprite;
        [SerializeField] private Sprite _normalLocationSprite;

        public void SetNode( MapNode node)
        {
            _basicMapLocationSprite.SetNode(node);
        }

        public void SetColor((Color normalColor, Color mouseOnColor, Color pressedColor) colorSet)
        {
            var cb = _selfButton.colors;
            cb.normalColor = cb.selectedColor = colorSet.normalColor;
            cb.highlightedColor = colorSet.mouseOnColor;
            cb.pressedColor = colorSet.pressedColor;
            _selfButton.colors = cb;
        }

        private void Awake()
        {
            _basicMapLocationSprite = GetComponent<BasicMapLocationSprite>();
            _selfButton = GetComponent<Button>();
            _selfButton.onClick.AddListener( () => Clicked?.Invoke(this));
        }

        public MapNode Node => _basicMapLocationSprite.Node;

        private BasicMapLocationSprite _basicMapLocationSprite;
        private Button _selfButton;
        private Location _here;
    }
}