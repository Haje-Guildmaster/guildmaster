using System;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    [Serializable]
    public class Place
    {
        public string PlaceName;
        public Sprite BackgroundSprite;
        public List<MoveButton> MoveButtons;
        
        [Serializable]
        public struct MoveButton
        {
            public Sprite ButtonSprite;
            public Vector2 Position, Direction;
            public int ConnectedPlaceIndex;
        }
    }
}