using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.MapRoam
{
    public class Place
    {
        public Sprite BackgroundSprite;
        public List<MoveButton> MoveButtons;
        
        public struct MoveButton
        {
            public Sprite ButtonSprite;
            public Vector2 Position, Direction;
            public Place ConnectedPlace;
        }
    }
}