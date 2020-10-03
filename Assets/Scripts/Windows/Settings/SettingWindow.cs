
namespace GuildMaster.Windows
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
