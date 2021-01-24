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
    public class RoamingNpc: ClickableComponent
    {
        [SerializeField] private NpcCode _npcCode;
        
        protected override void OnClick()
        {
            UiWindowsManager.Instance.npcInteractWindow.Open(_npcCode);
        }
    }
}