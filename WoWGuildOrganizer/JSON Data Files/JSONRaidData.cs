using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWGuildOrganizer
{
    class JSONRaidData
    {
        public IList<JSONZoneData> Zones { get; set; }
    }

    class JSONZoneData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ExpansionId { get; set; }
        public bool IsRaid { get; set; }
        public IList<JSONRaidBossData> Bosses { get; set; }
    }

    class JSONRaidBossData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
