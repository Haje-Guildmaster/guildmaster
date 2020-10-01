
namespace GuildMaster.Windows
{
    public class TextspeedWindow : DraggableWindow, IToggleableWindow
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