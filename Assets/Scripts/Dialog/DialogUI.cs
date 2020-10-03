using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Dialog;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace GuildMaster.Windows
{
    public class DialogUI : Window
    {
        [SerializeField] private TextEffect dialogText;
        [SerializeField] private AsyncButton dialogButton; // to skip to next line.
        public void Open()
        {
            base.OpenWindow();
        }
        public async Task WaitNextLineTriggered()// -> 중간에 진행이 멈출 수 있다.
        {
            await dialogButton.WaitForClick();
        }
        public void Printtext(string dial)
        {
            dialogText.SetMsg(dial);
        }
        
    }
}
