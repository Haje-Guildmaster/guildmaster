namespace GuildMaster.Windows
{
    public class TownMapWindow: Window, IToggleableWindow
    {
        public void Open()
        {
            base.OpenWindow();
        }
        private void Awake()
        {
            foreach (var epb in GetComponentsInChildren<ExplorationPrepareButton>())
                epb.BeforeAction += base.Close;
            foreach (var mpb in GetComponentsInChildren<MovePlaceButton>())
                mpb.BeforeMoving += base.Close;
        }
    }
}