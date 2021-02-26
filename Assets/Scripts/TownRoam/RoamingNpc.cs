using GuildMaster.Databases;
using GuildMaster.Npcs;
using GuildMaster.Tools;
using GuildMaster.Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GuildMaster.TownRoam
{
    /// <summary>
    /// 마을에서 보이는 npc 입니다. 클릭당하면 npcInteractWindow를 엽니다.
    /// </summary>
    public class RoamingNpc: ClickableComponent
    {
        [SerializeField] private NpcCode _npcCode;
        
        protected override void OnClick()
        {
            UiWindowsManager.Instance.npcInteractWindow.Open(_npcCode);
        }
    }
}