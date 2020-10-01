
namespace GuildMaster.Windows
{
    public class TextureWindow : DraggableWindow, IToggleableWindow
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