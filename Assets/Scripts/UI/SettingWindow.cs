
using GuildMaster.UI;

namespace GuildMaster.UI
{
    public class SettingWindow : DraggableWindow, IToggleableWindow
    {
        public void Open()
        {
            base.OpenWindow();
            Refresh();
        }

        private void Refresh()
        {

        }
    }
}
