using System;

namespace GuildMaster.GuildFacility
{
    public class GuildFacility
    {
        // public
        public string name;

        // private
        private IGuildFacilityComponent component;

        public GuildFacility(IGuildFacilityComponent component)
        {
            this.component = component;
        }

    }
}