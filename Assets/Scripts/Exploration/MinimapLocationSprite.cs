using System;
using GuildMaster.Databases;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;
    
    [RequireComponent(typeof(SpriteRenderer))]
    public class MinimapLocationSprite: MonoBehaviour, INodeRepresentative<MapNode>
    {
        [SerializeField] private Sprite _baseLocationSprite;
        [SerializeField] private Sprite _normalLocationSprite;
        
        public void SetNode(MapNode node)
        {
            Node = node;
            
            var here = ExplorationLocationDatabase.Get(Node.Content.LocationCode);
            
            switch (here.LocationType)
            {
                case Location.Type.Base:
                    _spriteRenderer.sprite = _baseLocationSprite;
                    break;
                case Location.Type.Normal:
                    _spriteRenderer.sprite = _normalLocationSprite;
                    break;
                default:
                    throw new Exception($"Couldn't process the Location type {here.LocationType}");
            }
        }
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private SpriteRenderer _spriteRenderer;
        public MapNode Node { get; private set; }
    }
}