using System;
using UnityEngine;

namespace GuildMaster.Npcs
{
    [Serializable]
    public struct NpcBasicData
    {
        public string npcName;
        public Sprite illustration;
        [TextArea] public string greeting;
    }
}