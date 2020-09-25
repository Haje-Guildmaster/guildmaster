using System.Collections.Generic;
using GuildMaster.Items;

namespace GuildMaster.Exploration
{
    public class ExplorationLog
    {
        public Dictionary<Item, int> AcquiredItems = new Dictionary<Item, int>();
        public Dictionary<Item, int> UsedItems = new Dictionary<Item, int>();
    }
}