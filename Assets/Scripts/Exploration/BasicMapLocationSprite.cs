using System;
using GuildMaster.Databases;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;
    
    [RequireComponent(typeof(Image))]
    public class BasicMapLocationSprite: MonoBehaviour, INodeRepresentative<MapNode>
    {
        [SerializeField] private Sprite _baseLocationSprite;
        [SerializeField] private Sprite _normalLocationSprite;
        
        public void SetNode(MapNode node)
        {
            Assert.IsTrue(Node == null);
            Node = node;
            
            var here = ExplorationLocationDatabase.Get(Node.Content.LocationCode);
            
            switch (here.LocationType)
            {
                case Location.Type.Base:
                    _selfImage.sprite = _baseLocationSprite;
                    break;
                case Location.Type.Normal:
                    _selfImage.sprite = _normalLocationSprite;
                    break;
                default:
                    throw new Exception($"Couldn't process the Location type {here.LocationType}");
            }
        }
        private void Awake()
        {
            _selfImage = GetComponent<Image>();
        }

        private Image _selfImage;
        public MapNode Node { get; private set; }
    }
}