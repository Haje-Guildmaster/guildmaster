using System.Threading.Tasks;
using GuildMaster.Tools;
using UnityEngine;
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
