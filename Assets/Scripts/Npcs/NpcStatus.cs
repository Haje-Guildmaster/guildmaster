using System;

namespace GuildMaster.Npcs
{
    public class NpcStatus
    {
        public event Action Changed;

        public int Affinity
        {
            get => _affinity;
            set
            {
                _affinity = value;
                Changed?.Invoke();
            }
        }

        private int _affinity;
    }
}