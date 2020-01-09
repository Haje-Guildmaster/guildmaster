using UnityEngine;

namespace GuildMaster.TownRoam
{
    public class TownEditData: ScriptableObject
    {
        private class TownLocation
        {
            public Town town;
            public Vector2 location;
        }
    }
}