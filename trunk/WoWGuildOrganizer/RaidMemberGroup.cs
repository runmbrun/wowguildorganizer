using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace WoWGuildOrganizer
{
    [Serializable]
    class RaidMemberGroup
    {
        public ArrayList RaidGroup = null;

        public RaidMemberGroup()
        {
            RaidGroup = new ArrayList();
        }
    }
}
