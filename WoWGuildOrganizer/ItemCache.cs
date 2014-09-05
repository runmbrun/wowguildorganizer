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
        /// <summary>
        /// Dictionary that contains all the items
        /// </summary>
        Dictionary<Int32, ItemInfo> Items = null;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemCache()
        {
            Items = new Dictionary<Int32, ItemInfo>();
        }

        /// <summary>
        /// Checks to see if the Dictionary has the item
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Boolean Contains(Int32 i)
        {
            return Items.ContainsKey(i);
        }

        /// <summary>
        /// Gets the item from the Dictionary.
        /// If the Dictionary doesn't contain the item,
        /// then get it and add in the new item.
        /// There is no item number 0, so ignore it
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ItemInfo GetItem(Int32 i)
        {
            ItemInfo item = new ItemInfo();

            // Does the current Dictionary have this item?
            if (Items.ContainsKey(i))
            {
                // Yes, return the item requested
                item = Items[i];
            }
            else if (i != 0)
            {
                // Now, the item doesn't exist in the Dictionary                
                GetItemInfo getNewItem = new GetItemInfo();

                // Fetch the data
                if (getNewItem.CollectData(i))
                {
                    ItemInfo newItem = getNewItem.Item;

                    // Now add the new item to the Dictionary
                    this.AddItem(newItem);
                }
                else
                {
                    WoWGuildOrganizer.Logging.Log(string.Format("Error: Can't retrieve information about item {0}", i));
                }
            }

            return item;
        }

        /// <summary>
        /// Add a new item to the dictionary
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Boolean AddItem(ItemInfo i)
        {
            Boolean Success = false;

            // Add the new item to the Dictionary
            Items.Add(i.Id, i);

            return Success;
        }

        /// <summary>
        /// Return the number of items currently stored in the dictionary
        /// </summary>
        /// <returns></returns>
        public Int32 GetCount()
        {   
            return Items.Count;
        }

        public Dictionary<Int32, ItemInfo> GetData()
        {
            return Items;
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /*public List<int> Search(int ident)
        {
            //List<int> tests = Items.Keys.ToList<int>();
            List<int> results = null; // tests.FindAll(ident);

            results = Items.Keys.ToList<int>().FindAll(
                delegate(int bk)
                {
                    return Items.Keys.Any(ident);
                }
                );

            return results;
        }*/
    }
}
