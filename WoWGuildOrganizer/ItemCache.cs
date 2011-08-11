using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace WoWGuildOrganizer
{
    [Serializable]
    public class ItemCache
    {
        public ArrayList Items = null;

        public ItemCache()
        {
            Items = new ArrayList();
        }

    }
}
