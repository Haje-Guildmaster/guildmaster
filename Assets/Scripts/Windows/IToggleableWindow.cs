namespace GuildMaster.Windows
{
    public interface IToggleableWindow
    {
        void Open();
    }
    
    public static class ToggleExtension
    {
        public static void Toggle<T>(this T window) where T: Window, IToggleableWindow
        {
            if (window.IsOpen)
                window.Close();
            else
                window.Open();
        }
    }   
}