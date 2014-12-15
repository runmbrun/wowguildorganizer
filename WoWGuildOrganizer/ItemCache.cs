using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WoWGuildOrganizer
{
    [Serializable]
    public class ItemCache
    {
        /// <summary>
        /// DataTable that contains all the items
        /// </summary>
        DataTable items;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemCache()
        {
            // Create the datatable
            items = new DataTable();

            // Add the columns needed
            items.Columns.Add("id", typeof(int));
            items.Columns.Add("name", typeof(string));
            items.Columns.Add("context", typeof(string));
            items.Columns.Add("iteminfo", typeof(ItemInfo));
        }

        /// <summary>
        /// Checks to see if the Dictionary has the item
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool Contains(int i, string context)
        {
            bool found = false;
            DataRow[] rows = null;

            // Check to see if a context has been provided
            if (context == null || context == string.Empty)
            {
                // no context
                rows = items.Select(string.Format("id = {0}", i));                
            }
            else
            {
                // context has been provided
                rows = items.Select(string.Format("id = {0} and context = '{1}'", i, context));
            }

            // now see if the item was found
            if (rows != null && rows.Length > 0)
            {
                found = true;
            }

            return found;
        }

        /// <summary>
        /// Gets the item from the Dictionary.
        /// If the Dictionary doesn't contain the item,
        /// then get it and add in the new item.
        /// There is no item number 0, so ignore it
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ItemInfo GetItem(int i, string context)
        {
            ItemInfo item = new ItemInfo();

            // Does the current Dictionary have this item?
            if (this.Contains(i, context))
            {
                // Yes, return the item requested
                //item = Items[i];
                DataRow[] rows = null;

                // Check to see if a context has been provided
                if (context == null || context == string.Empty)
                {
                    // no context
                    rows = items.Select(string.Format("id = {0}", i));
                }
                else
                {
                    // context has been provided
                    rows = items.Select(string.Format("id = {0} and context = '{1}'", i, context));
                }

                item = (ItemInfo)rows[0]["iteminfo"];
            }
            else
            {
                // Now, the item doesn't exist in the Dictionary                
                GetItemInfo getNewItem = new GetItemInfo();
                
                // Fetch the data
                if (getNewItem.CollectData(i, context))
                {
                    ItemInfo newItem = getNewItem.Item;

                    // Now add the new item to the Dictionary
                    this.AddItem(newItem, context);

                    item = newItem;
                }
                else
                {
                    Logging.Error(string.Format("Can't retrieve information about item: {0} with context: {1}", i, context));
                }
            }

            return item;
        }

        /// <summary>
        /// Add a new item to the dictionary
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool AddItem(ItemInfo i, string context)
        {
            bool success = false;

            // Add the new item to the Dictionary
            if (!this.Contains(i.Id, context))
            {
                try
                {
                    DataRow row = items.NewRow();

                    row["id"] = i.Id;
                    row["name"] = i.Name;
                    row["context"] = context;
                    row["iteminfo"] = i;

                    items.Rows.Add(row);

                    success = true;
                }
                catch (Exception ex)
                {
                    WoWGuildOrganizer.Logging.Error(string.Format("Can't add information to cache about item: {0} with context: {1}. {2}", i, context, ex.Message));
                }
            }
            else
            {
                success = true;
            }

            return success;
        }

        /// <summary>
        /// Return the number of items currently stored in the dictionary
        /// </summary>
        /// <returns></returns>
        public Int32 GetCount()
        {   
            return items.Rows.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetData()
        {
            return items;
        }
    }
}
