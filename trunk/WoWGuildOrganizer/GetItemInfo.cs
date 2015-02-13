﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;



namespace WoWGuildOrganizer
{
    /// <summary>
    /// 
    /// </summary>
    class GetItemInfo
    {
        /// <summary>
        /// 
        /// </summary>
        private ItemInfo item;

        /// <summary>
        /// 
        /// </summary>
        public ItemInfo Item
        {
            set 
            { 
                item = value; 
            }

            get 
            { 
                return item; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string availableContexts;

        /// <summary>
        /// 
        /// </summary>
        public GetItemInfo()
        {
            Item = new ItemInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CollectData(int id, string context)
        {
            bool success = false;
                        
            // First check with no context
            //  Some items are giving a context, but do not have the context web site available
            //  So, check first.  Disavantage of checking site twice is ok since once it's in the cache, 
            //  a new check won't be needed again
            success = CollectData(@"http://us.battle.net/api/wow/item/" + id.ToString());

            // If failure and a context has been provided, then check the site with the context
            if (!success && context != string.Empty)
            {
                if (context == "vendor")
                {
                    // vendor doesn't work for anything know... 
                    // instead check the parsed web site and see if there is only 1 context available
                    if (this.availableContexts.Contains("availableContexts"))
                    {
                        string search = @"{id:(?<id>\d+),availableContexts:\[(?<context>[a-zA-Z-]+)\]}";
                        Regex test = new Regex(search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
                        string newData = this.availableContexts.Replace("\"", "");
                        
                        // See how many matches there are
                        MatchCollection matches = test.Matches(newData);

                        // Get the item info
                        if (matches.Count == 1 && matches[0].Groups["context"].Success)
                        {
                            context = matches[0].Groups["context"].Value.ToString();
                        }
                    }
                }

                // URL = Host + "/api/wow/item/" + ItemId + / + Context => "hasSockets":false,
                success = CollectData(@"http://us.battle.net/api/wow/item/" + id.ToString() + "/" + context);
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WebPage"></param>
        /// <returns></returns>
        public bool CollectData(string WebPage)
        {
            bool success = false;

            try
            {                
                GetWebSiteData getSiteData = new GetWebSiteData();

                if (getSiteData.Parse(WebPage))
                {
                    // now parse the data
                    ItemInfo[] results = ParseOutItems(getSiteData.Data);

                    if (results.Length > 0)
                    {
                        // Put the first result in the usual place
                        Item = results[0];

                        this.availableContexts = string.Empty;

                        success = true;
                    }
                    else
                    {
                        this.availableContexts = getSiteData.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Debug(String.Format("ERROR: {0} in CollectData() in GetItemInfo.cs", ex.Message));
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ItemInfo[] ParseOutItems(string data)
        {
            ItemInfo[] foundItems = null;

            #region " Example "
            /* Example:
                    * http://us.battle.net/wow/en/item/38661 (no sockets)
                    * http://us.battle.net/api/wow/item/38661 (no sockets)
                    * 
                    {
                    "id":38661,
                    "disenchantingSkillRank":225,
                    "description":"",
                    "name":"Greathelm of the Scourge Champion",
                    "icon":"inv_helmet_06",
                    "stackable":1,
                    "itemBind":1,
                    "bonusStats":[
                    {
                        "stat":4,
                        "amount":27,
                        "reforged":false
                    },
                    {
                        "stat":32,
                        "amount":20,
                        "reforged":false
                    },
                    {
                        "stat":7,
                        "amount":19,
                        "reforged":false
                    }
                    ],
                    "itemSpells":[],
                    "buyPrice":74,
                    "itemClass":4,
                    "itemSubClass":4,
                    "containerSlots":0,
                    "inventoryType":1,
                    "equippable":true,
                    "itemLevel":70,
                    "maxCount":0,
                    "maxDurability":80,
                    "minFactionId":0,
                    "minReputation":0,
                    "quality":3,
                    "sellPrice":14,
                    "requiredSkill":0,
                    "requiredLevel":0,
                    "requiredSkillRank":0,
                    "itemSource":{
                    "sourceId":12779,
                    "sourceType":"REWARD_FOR_QUEST"
                    },
                    "baseArmor":514,
                    "hasSockets":false,
                    "isAuctionable":false
                }
                    * http://us.battle.net/wow/en/item/67143 (sockets)
                    * http://us.battle.net/api/wow/item/67143 (sockets)
                    * 
                {
                    "id":67143,
                    "disenchantingSkillRank":475,
                    "description":"",
                    "name":"Icebone Hauberk",
                    "icon":"inv_chest_plate_raiddeathknight_i_01",
                    "stackable":1,
                    "itemBind":2,
                    "bonusStats":[
                    {
                        "stat":4,
                        "amount":239,
                        "reforged":false
                    },
                    {
                        "stat":31,
                        "amount":173,
                        "reforged":false
                    },
                    {
                        "stat":14,
                        "amount":281,
                        "reforged":false
                    },
                    {
                        "stat":7,
                        "amount":512,
                        "reforged":false
                    }
                    ],
                    "itemSpells":[],
                    "buyPrice":1242148,
                    "itemClass":4,
                    "itemSubClass":4,
                    "containerSlots":0,
                    "inventoryType":5,
                    "equippable":true,
                    "itemLevel":359,
                    "maxCount":0,
                    "maxDurability":165,
                    "minFactionId":0,
                    "minReputation":0,
                    "quality":4,
                    "sellPrice":248429,
                    "requiredSkill":0,
                    "requiredLevel":85,
                    "requiredSkillRank":0,
                    "socketInfo":{
                    "sockets":[
                        {
                        "type":"YELLOW"
                        },
                        {
                        "type":"YELLOW"
                        }
                    ]
                    },
                    "itemSource":{
                    "sourceId":50005,
                    "sourceType":"CREATURE_DROP"
                    },
                    "baseArmor":3426,
                    "hasSockets":true,
                    "isAuctionable":true
                }
                */

            #endregion

            try
            {
                string search = @"id:(?<id>\d+),.*?name:(?<name>[A-Za-z '""\\.,:-]+),icon.*?bonusStats:\[(?<stats>.*?)\],itemSpells:\[(?<itemspells>.*?)\],.*itemClass:(?<itemClass>\d+?),itemSubClass:(?<itemSubClass>\d+?),.*?inventoryType:(?<inventoryType>\d+?),.*?itemLevel:(?<itemLevel>\d+?),.*?quality:(?<quality>\d+?),.*?hasSockets:(?<hasSockets>\w+?),.*?(socketInfo:{(?<socketInfo>.*?)},)?";
                Regex test = new Regex(search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
                string newData = data.Replace("\"", "");

                // See how many matches there are
                MatchCollection matches = test.Matches(newData);
                foundItems = new ItemInfo[matches.Count];
                int count = 0;

                // Get the item info
                foreach (Match result in matches)
                {
                    ItemInfo item = new ItemInfo();

                    // First get the slot, if no slot then it's an missing item
                    if (result.Groups["id"].Success)
                    {
                        item.Id = Convert.ToInt32(result.Groups["id"].Value);
                    }

                    if (result.Groups["name"].Success)
                    {
                        item.Name = result.Groups["name"].Value.ToString().Replace("\\", "\"");
                    }

                    if (result.Groups["stats"].Success)
                    {
                        item.Stats = result.Groups["stats"].Value;
                    }

                    if (result.Groups["itemspells"].Success)
                    {
                        item.Spells = result.Groups["itemspells"].Value;
                    }

                    if (result.Groups["itemClass"].Success)
                    {
                        item.ItemClass = Convert.ToInt32(result.Groups["itemClass"].Value);
                    }

                    if (result.Groups["itemSubClass"].Success)
                    {
                        item.ItemSubClass = Convert.ToInt32(result.Groups["itemSubClass"].Value);
                    }

                    if (result.Groups["inventoryType"].Success)
                    {
                        item.InventoryType = Convert.ToInt32(result.Groups["inventoryType"].Value);

                        // Find if the Item is enchantable
                        if (DetermineIfCanEnchantItem(item.InventoryType))
                        {
                            item.CanEnchant = true;
                        }
                    }

                    if (result.Groups["itemLevel"].Success)
                    {
                        item.ItemLevel = Convert.ToInt32(result.Groups["itemLevel"].Value);
                    }

                    if (result.Groups["quality"].Success)
                    {
                        item.Quality = Convert.ToInt32(result.Groups["quality"].Value);
                    }

                    if (result.Groups["hasSockets"].Success)
                    {
                        if (result.Groups["hasSockets"].Value.ToUpper() == "TRUE")
                        {
                            item.CanSocket = true;

                            // Capture the number of sockets
                            int i = 0;

                            while ((i = data.IndexOf(@"""type"":""", i + 1)) != -1)
                            {
                                item.SocketCount += 1;
                            }
                        }
                    }

                    // Get the armor if it exists:
                    item.Armor = 0;
                    string searchArmorString = "\"armor\":";
                    int locBegin = data.IndexOf(searchArmorString);

                    if (locBegin > -1)
                    {
                        try
                        {
                            int locEnd = data.IndexOf(",", locBegin + searchArmorString.Length);

                            // armor has been found, grab it
                            string testArmor = data.Substring(locBegin + searchArmorString.Length, locEnd - (locBegin + searchArmorString.Length));

                            // Convert armor to a int
                            item.Armor = Convert.ToInt32(testArmor);
                        }
                        catch (Exception ex)
                        {
                            Logging.DisplayError(ex.Message);
                        }
                    }

                    foundItems[count++] = item;
                }
            }
            catch (Exception ex)
            {
                Logging.Error(string.Format("{0}", ex.Message));
            }

            return foundItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool DetermineIfCanEnchantItem(int type)
        {
            bool enchantable = false;

            /*
             * It has to be one of these...
                "itemClass":4,
                "itemSubClass":4,
                "containerSlots":0,
                "inventoryType":5,
            */

            switch (type)
            {
                // main hand
                case 0:
                    enchantable = true;
                    break;
                // neck
                case 2:
                    enchantable = true;
                    break;
                // finger
                case 11:
                    enchantable = true;
                    break;
                // one-hand weapon
                case 13:
                    enchantable = true;
                    break;
                // back
                case 16:
                    enchantable = true;
                    break;
                // two handed weapon
                case 17:                    
                    enchantable = true;
                    break;
                // main hand weapon
                case 21:
                    enchantable = true;
                    break;
                // off hand weapon
                case 22:
                    enchantable = true;
                    break;
                default:
                    enchantable = false;
                    break;
            }

            return enchantable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] CollectContexts(int id)
        {
            string[] results = null;

            // Should always return a failure when a context has not been provided, so then check for all the available contexts
            if (!this.CollectData(@"http://us.battle.net/api/wow/item/" + id.ToString()))
            {
                // instead check the parsed web site and see if there is only 1 context available
                if (this.availableContexts.Contains("availableContexts"))
                {
                    string search = @"{id:(?<id>\d+),availableContexts:\[(?<context>[a-zA-Z-,]+)\]}";
                    Regex test = new Regex(search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
                    string newData = this.availableContexts.Replace("\"", "");

                    // See how many matches there are
                    MatchCollection matches = test.Matches(newData);

                    // Get the item info
                    if (matches.Count == 1 && matches[0].Groups["context"].Success)
                    {
                        string contexts = matches[0].Groups["context"].Value.ToString();
                        
                        results = contexts.Split(',');
                    }
                }
            }

            return results;
        }
    }
}
