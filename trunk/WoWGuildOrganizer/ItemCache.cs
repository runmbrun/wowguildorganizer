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
        /// <param name="itemId"></param>
        /// <returns></returns>
        public ItemInfo GetItem(int itemId, string context)
        {
            ItemInfo item = new ItemInfo();

            // First check to make sure that it's a valid item id
            // Then check to see if it's in the cache
            // if it is, return the item,
            // but if not, then add the new item to the cache
            if (itemId != 0)
            {
                if (this.Contains(itemId, context))
                {
                    // Yes, return the item requested
                    DataRow[] rows = null;

                    // Check to see if a context has been provided
                    if (context == null || context == string.Empty)
                    {
                        // no context
                        rows = items.Select(string.Format("id = {0}", itemId));
                    }
                    else
                    {
                        // context has been provided
                        rows = items.Select(string.Format("id = {0} and context = '{1}'", itemId, context));
                    }

                    item = (ItemInfo)rows[0]["iteminfo"];
                }
                else
                {
                    // Now, the item doesn't exist in the Dictionary                
                    GetItemInfo getNewItem = new GetItemInfo();

                    // Fetch the data
                    if (getNewItem.CollectData(itemId, context))
                    {
                        ItemInfo newItem = getNewItem.Item;

                        // Now add the new item to the Dictionary
                        this.AddItem(newItem, context);

                        item = newItem;
                    }
                    else
                    {
                        Logging.Error(string.Format("Can't retrieve information about item: {0} with context: {1}", itemId, context));
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// Add a new item to the dictionary
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(ItemInfo item, string context)
        {
            bool success = false;

            // First check to make sure that it's a valid item id
            // Then check to see if it's in the cache
            // if it is, return the item,
            // but if not, then add the new item to the cache
            if (item.Id == 0)
            {
                // This is not a valid item id
                success = false;
            }
            else if (!this.Contains(item.Id, context))
            {
                try
                {
                    DataRow row = items.NewRow();

                    row["id"] = item.Id;
                    row["name"] = item.Name;
                    row["context"] = context;
                    row["iteminfo"] = item;

                    items.Rows.Add(row);

                    success = true;
                }
                catch (Exception ex)
                {
                    WoWGuildOrganizer.Logging.Error(string.Format("Can't add information to cache about item: {0} with context: {1}. {2}", item, context, ex.Message));
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
        public int GetCount()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public string[] GetAvailableContexts(int itemId)
        {
            string[] results = null;

            /*
            DataRow[] rows = null;

            try
            {
                // Check to see what contexts are currently cached
                rows = items.Select(string.Format("id = {0}", itemId));

                if (rows.Length > 0)
                {
                    int count = 0;
                    results = string[rows.Length];

                    for (int i=0; i<rows.Length; i++)
                    {
                        results[count] = rows[count]["context"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {             
            }*/

            GetItemInfo getNewItem = new GetItemInfo();

            //
            results = getNewItem.CollectContexts(itemId);

            if (results == null)
            {
                Logging.Error(string.Format("Can't retrieve contexts for item [{0}]: ", itemId));
            }

            return results;
        }
    }
}
