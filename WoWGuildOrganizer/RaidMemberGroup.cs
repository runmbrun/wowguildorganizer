using System;
using System.Collections;

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
