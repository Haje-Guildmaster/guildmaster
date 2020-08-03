using System;
using GuildMaster.Databases;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;



namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;
    
    [RequireComponent(typeof(SpriteRenderer))]
    public class LocationButton: ColorTintButton<LocationButton>, INodeRepresentative<MapNode>
    {
        [SerializeField] private Sprite _baseLocationSprite;
        [SerializeField] private Sprite _normalLocationSprite;
        protected override LocationButton EventArgument => this;
        
        
        public void SetNode( MapNode node)
        {
            Assert.IsNull(Node);
            Node = node;
            _here = ExplorationLocationDatabase.Get(Node.Content.LocationCode);
            
            switch (_here.LocationType)
            {
                case Location.Type.Base:
                    _spriteRenderer.sprite = _baseLocationSprite;
                    break;
                case Location.Type.Normal:
                    _spriteRenderer.sprite = _normalLocationSprite;
                    break;
                default:
                    throw new Exception($"Couldn't process the Location type {_here.LocationType}");
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public MapNode Node { get; private set; }

        private SpriteRenderer _spriteRenderer;
        private Location _here;
    }
}