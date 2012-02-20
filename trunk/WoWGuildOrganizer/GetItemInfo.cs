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
        Boolean Debug = false;
        String DebugFile = @"c:\temp\GetItemInfo.log";

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

                    String DataString = getSiteData.Data;
                    String SearchEnchants = @"""inventoryType"":";                    
                    Int32 InventoryType = -1;
                    String SearchSockets = @"""hasSockets"":";
                    String SocketBoolean = "";
                    String SearchSocketCount = @"""type"":";
                    String SearchItemLevel = @"""itemLevel"":";
                    Int32 ItemLevel = -1;
                    
                    #region DEBUG
                    // this is for debugging only
                    if (Debug)
                    {
                        // Create the output file.
                        using (FileStream fs = File.Create(DebugFile)) { }
                        // Open the stream and write to it
                        using (FileStream fs = File.OpenWrite(DebugFile))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes("   ");
                            // Add some information to the file.
                            fs.Write(info, 0, info.Length);
                            info = new UTF8Encoding(true).GetBytes(DataString);
                            fs.Write(info, 0, info.Length);
                        }
                    }
                    #endregion


                    // Capture the inventory type
                    Int32 i = DataString.IndexOf(SearchEnchants);
                    Int32 j = DataString.IndexOf(",", i);
                    InventoryType = Convert.ToInt32(DataString.Substring(i + SearchEnchants.Length, j - (i + SearchEnchants.Length)));
                    Item.InventoryType = InventoryType;

                    // Find if the Item is enchantable
                    if (DetermineIfCanEnchantItem(InventoryType))
                    {
                        Item.CanEnchant = true;
                    }

                    // Capture the socket boolean
                    i = DataString.IndexOf(SearchSockets);
                    j = DataString.IndexOf(",", i);
                    SocketBoolean = DataString.Substring(i + SearchSockets.Length, j - (i + SearchSockets.Length));


                    // Find if the Item has sockets, and if so then how many and what color(s)
                    if (SocketBoolean.ToUpper() == "TRUE")
                    {
                        Item.CanSocket = true;
                        
                        // Now search for how many sockets...
                        //   capture the number of sockets
                        i = 0;
                        while ((i = DataString.IndexOf(SearchSocketCount, i + 1)) != -1)
                        {
                            Item.SocketCount += 1;
                        }
                    }
                    
                    // Capture the item level
                    i = DataString.IndexOf(SearchItemLevel);
                    j = DataString.IndexOf(",", i);                    
                    Item.ItemLevel = Convert.ToInt32(DataString.Substring(i + SearchItemLevel.Length, j - (i + SearchItemLevel.Length)));

                    Success = true;
                }
            }
            catch (Exception ex)
            {
                String Error = String.Format("ERROR: ", ex.Message);
            }

            return Success;
        }

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
                    CanEnchant = true;
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
                    CanEnchant = false;
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
