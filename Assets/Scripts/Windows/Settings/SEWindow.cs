
using GuildMaster.Windows;

namespace GuildMaster.Windows
{
    public class SEWindow : DraggableWindow, IToggleableWindow
    {
        public void Open()
        {
            base.OpenWindow();
            Refresh();
        }

        // Update is called once per frame
        void Refresh()
        {
            
        }
    }
}