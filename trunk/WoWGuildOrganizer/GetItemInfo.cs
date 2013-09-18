using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;



namespace WoWGuildOrganizer
{
    class GetItemInfo
    {
        private ItemInfo _item;
        public ItemInfo Item
        {
            set { _item = value; }
            get { return _item; }
        }


        /// <summary>
        /// 
        /// </summary>
        public GetItemInfo()
        {
            Item = new ItemInfo();
        }

        public Boolean CollectData(Int32 Id)
        {
            // URL = Host + "/api/wow/item/" + ItemId => "hasSockets":false,
            return CollectData(@"http://us.battle.net/api/wow/item/" + Id.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WebPage"></param>
        /// <returns></returns>
        public Boolean CollectData(String WebPage)
        {
            Boolean Success = false;

            try
            {                
                GetWebSiteData getSiteData = new GetWebSiteData();

                if (!getSiteData.Parse(WebPage))
                {
                    // error
                }
                else
                {
                    // now parse the data
                    ItemInfo[] results = ParseOutItems(getSiteData.Data);

                    // Put the first result in the usual place
                    Item = results[0];

                    Success = true;
                }
            }
            catch (Exception ex)
            {
                String Error = String.Format("ERROR: ", ex.Message);
            }

            return Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ItemInfo[] ParseOutItems(string data)
        {
            ItemInfo[] items;

            // piece in web site that is important
            // code to expresso => replace "" with "
            // expresso to code => replace " with ""

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

            //string DataString = getSiteData.Data;
            string Search = @"id"":(?<id>\d+),.*?name"":""(?<name>[A-Za-z ',:-]+)?"".*?bonusStats"":\[(?<stats>.*?)\],.*itemClass"":(?<itemClass>\d+?),""itemSubClass"":(?<itemSubClass>\d+?),.*?inventoryType"":(?<inventoryType>\d+?),.*?itemLevel"":(?<itemLevel>\d+?),.*?quality"":(?<quality>\d+?),.*?hasSockets"":(?<hasSockets>\w+?),.*?(socketInfo"":{(?<socketInfo>.*?)},)?";
            Regex test = new Regex(Search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

            // See how many matches there are
            MatchCollection matches = test.Matches(data);
            items = new ItemInfo[matches.Count];
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
                    item.Name = result.Groups["name"].Value;
                }

                if (result.Groups["stats"].Success)
                {
                    item.Stats = result.Groups["stats"].Value;
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

                        // TODO: is this the best way to do this?
                        //   capture the number of sockets
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
                        // TODO: how to handle this error?
                        string message = ex.Message;
                    }
                }

                items[count++] = item;
            }

            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Boolean DetermineIfCanEnchantItem(Int32 type)
        {
            Boolean CanEnchant = false;

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
                    CanEnchant = true;
                    break;
                // head
                case 1: 
                    CanEnchant = false;
                    break;
                // neck
                case 2:
                    CanEnchant = false;
                    break;
                // shoulder
                case 3:
                    CanEnchant = true;
                    break;
                // shirt
                case 4:
                    CanEnchant = false;
                    break; 
                // chest
                case 5:
                    CanEnchant = true;
                    break;
                // waist
                case 6:
                    CanEnchant = false;  // This is extraSocket, not an enchant
                    break;
                // legs
                case 7:
                    CanEnchant = true;
                    break;
                // feet
                case 8:
                    CanEnchant = true;
                    break;
                // wrist
                case 9:
                    CanEnchant = true;
                    break;
                // hands
                case 10:
                    CanEnchant = true;
                    break;
                // finger
                case 11:
                    CanEnchant = false;
                    break;
                // trinket
                case 12:
                    CanEnchant = false;
                    break;
                // one-hand weapon
                case 13:
                    CanEnchant = true;
                    break;
                // shield
                case 14:
                    CanEnchant = true;
                    break;
                // ranged
                case 15:
                    CanEnchant = false;
                    break;
                // back
                case 16:
                    CanEnchant = true;
                    break;
                // two handed weapon
                case 17:                    
                    CanEnchant = true;
                    break;
                // tabard
                case 19:
                    CanEnchant = false;
                    break;
                // chest - robe
                case 20:
                    CanEnchant = true;
                    break;
                // main hand weapon
                case 21:
                    CanEnchant = true;
                    break;
                // off hand weapon
                case 22:
                    CanEnchant = true;
                    break;
                // off hand frill
                case 23:
                    CanEnchant = true;
                    break;
                // thrown
                case 25:
                    CanEnchant = false;
                    break;
                // ranged
                case 26:
                    CanEnchant = false;
                    break;
                // relic
                case 28:
                    CanEnchant = false;
                    break;
                default:
                    CanEnchant = false;
                    break;
            }

            return CanEnchant;
        }
    }
}
