using System;
using UnityEngine;
using GuildMaster.Databases;

namespace GuildMaster.Npcs
{
    [Serializable]
    public struct NpcBasicData
    {
        public string npcName;
        public Sprite illustration;
        [TextArea] public string greeting;
        public DialogCode talk;
    }
}