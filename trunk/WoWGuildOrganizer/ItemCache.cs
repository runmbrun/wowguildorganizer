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
        Dictionary<Int32, ItemInfo> Items = null;

        public ItemCache()
        {
            Items = new Dictionary<Int32, ItemInfo>();
        }

        public Boolean Contains(Int32 i)
        {
            return Items.ContainsKey(i);
        }

        public ItemInfo GetItem(Int32 i)
        {
            ItemInfo item = new ItemInfo();


            if (Items.ContainsKey(i))
            {
                item = Items[i];
            }

            return item;
        }

        public Boolean AddItem(ItemInfo i)
        {
            Boolean Success = false;


            Items.Add(i.Id, i);

            return Success;
        }

        public Int32 GetCount()
        {   
            return Items.Count;
        }

    }
}
