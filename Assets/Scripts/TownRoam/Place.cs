using System;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    [Serializable]
    public class Place
    {
        public const int Width = 600;
        public const int Height = 400;
        
        public string PlaceName;
        public Sprite BackgroundSprite;
        public List<MoveButton> MoveButtons;
        public GameObject objects;
        
        [Serializable]
        public struct MoveButton
        {
            public Sprite ButtonSprite;
            public Vector2 Position, Direction;
            public int ConnectedPlaceIndex;
        }
    }
}